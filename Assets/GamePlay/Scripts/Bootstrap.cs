using System.Collections.Generic;
using UnityEngine;
using Services.PlayerInput;
using Services.CameraManipulation;
using Levels.TileControl;
using UnityEngine.Tilemaps;
using Levels.Logic;
using Enemies;
using Waves;
using Builder;
using Towers;
using System;
using Levels.Tiles;
using Enemies.AI;
using Unity.VisualScripting;
using StateMachine = Common.States.StateMachine;
using GamePlay.UI;
using Common;
using Common.UI;
using LevelEditor.UI;

namespace GamePlay
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]private float _cameraSpeed = 4f;
        [SerializeField]private TileDatabase _tileDatabase;
        [SerializeField]private PathFindMultipliersDatabase _pathFindMultiplierDatabase;
        [SerializeField]private EnemiesDatabase _enemiesDatabase;
        [SerializeField]private Tilemap _groundTileMap;
        [SerializeField]private Tilemap _roadTileMap;
        [SerializeField]private TowersDatabase _towersDatabase;
        [SerializeField]private TowersPanel _towersPanel;
        [SerializeField] private GameOverView _gameOverView;
        [SerializeField] private PauseView _pauseView;
        [SerializeField] private PauseButton _pauseButton;
        [SerializeField] private UIInputBlocker _inputBlocker;
        [SerializeField] private MoneyView _moneyView;
        [SerializeField] private MainBuildingHealth _healthUI;
        private PlayerInput _playerInput;
        private CameraManipulation _cameraManipulation;
        private CameraMediator _cameraMediator;
        private Level _level;
        private TileController _tileController;
        private LevelAndTilesMediator _levelAndTilesMediator;
        private LevelLoader _levelLoader;
        private EnemySpawner _enemySpawner;
        private EnemyFactory _enemyFactory;
        private PlacableBuilder _builder;
        private BuilderMediator _builderMediator;
        private PlacableFactory _placableFactory;
        private PathFinder _pathFinder;
        private StateMachine _gamePlayerStateMachine;
        private PausableManager _pausableManager;
        private LevelEditorInputBlockerMediator _inputBlockerMediator;
        private PauseMediator _pauseMediator;
        private PlayerStatsUpdater _playerStatsUpdater;
        private BuildPossibilityChecker _buildPossibilityChecker;
        private MoneyMediator _moneyMediator;
        private MainBuldingMediator _mainBuildingMediator;
        private void Awake() 
        {
            _pausableManager = new PausableManager();
            _playerInput = new PlayerInput();
            _cameraManipulation = new CameraManipulation(_cameraSpeed, Camera.main);
            _cameraMediator = new CameraMediator(_playerInput,_cameraManipulation);
            _levelLoader = new LevelLoader();

            if (!_levelLoader.TryLoadLevel(PlayerSettingsData.LoadChosenMapName(), out LevelData levelData))
                throw new System.Exception("No level to load");

            _level = new Level(levelData);
            PlayerStats playerStats = new PlayerStats(levelData.startMoney);

            _tileController = new TileController(_tileDatabase,_groundTileMap,_roadTileMap);
            _levelAndTilesMediator = new LevelAndTilesMediator(_tileController,_level);

            List<Vector2Int> spawnerPositions = new List<Vector2Int>();

            foreach(int spawnerIndex in levelData.spawnerPlaces)
            {
                Vector2Int spawnerPosition = _level.Grid.ConvertIntToVector2Int(spawnerIndex);
                Debug.Log($"Registered spawner at {spawnerPosition}");
                spawnerPositions.Add(spawnerPosition);
            }

            _level.UpdateGridData(levelData.gridData);
            _pathFinder = new PathFinder(_level.Grid,_pathFindMultiplierDatabase);

            Spawners spawners = new Spawners(spawnerPositions.ToArray(), _pathFinder);

            _enemyFactory = new EnemyFactory(_enemiesDatabase, _level.Grid, spawners, _pathFindMultiplierDatabase);
            List<Wave> waves = new List<Wave>();

            foreach (WaveData waveData in levelData.waves)
            {
                waves.Add(new Wave(waveData));
            }

            _enemySpawner = new EnemySpawner(waves.ToArray(), levelData.firstWaveDelay,1f,_enemyFactory);

            _placableFactory = new PlacableFactory(_towersDatabase,_enemySpawner, playerStats);

            //TODO this must be loaded from level
            AvailablePlacables availablePlacables = new AvailablePlacables()
            {
                placableIds = Enum.GetValues(typeof(PlacableEnum)).ConvertTo<PlacableEnum[]>()
            };

            _builder = new PlacableBuilder(availablePlacables, _placableFactory, true);

            _towersPanel.Init(_towersDatabase);

            _builderMediator = new BuilderMediator(_playerInput,_builder, _level.Grid,_towersPanel);

            SceneLoader sceneLoader = new SceneLoader();

            _gamePlayerStateMachine = new StateMachine();
            

            _inputBlockerMediator = new LevelEditorInputBlockerMediator(_inputBlocker, _playerInput);

            MenuParentsManager pauseMenus = new MenuParentsManager();
            pauseMenus.Add(_pauseButton);
            pauseMenus.Add(_pauseView);

            _pausableManager.Add(_enemySpawner);
            _pausableManager.Add(_playerInput);
            _pausableManager.Add(_builder);
            _pausableManager.Add(_level.Grid);

            _pauseMediator = new PauseMediator(_pausableManager, _pauseView, _pauseButton, pauseMenus, sceneLoader);

            pauseMenus.HideAll();
            pauseMenus.Show(_pauseButton);

            PrepareState prepareState = new PrepareState(_gamePlayerStateMachine, _builder, spawners, false, _level.Grid.GridSize);
            EnemySpawnState enemySpawnState = new EnemySpawnState(_gamePlayerStateMachine, _enemySpawner, _builder);
            LoseState loseState = new LoseState(_gamePlayerStateMachine, _gameOverView, sceneLoader, _builderMediator, pauseMenus);
            WinState winState = new WinState(_gamePlayerStateMachine, _gameOverView, sceneLoader, _builderMediator, pauseMenus);

            _gamePlayerStateMachine.AddState(prepareState);
            _gamePlayerStateMachine.AddState(enemySpawnState);
            _gamePlayerStateMachine.AddState(loseState);
            _gamePlayerStateMachine.AddState(winState);

            _playerStatsUpdater = new PlayerStatsUpdater(playerStats, _builder, _towersDatabase);
            _buildPossibilityChecker = new BuildPossibilityChecker(playerStats, _builder, _towersDatabase);

            _moneyMediator = new MoneyMediator(playerStats, _moneyView);

            _mainBuildingMediator = new MainBuldingMediator(_builder, _healthUI);
        }

        private void OnDestroy() 
        {
            _cameraMediator?.Dispose();
            _levelAndTilesMediator?.Dispose();
            _builderMediator?.Dispose();
            _gamePlayerStateMachine?.Dispose();
            _inputBlockerMediator?.Dispose();
            _pauseMediator?.Dispose();
            _playerStatsUpdater?.Dispose();
            _buildPossibilityChecker?.Dispose();
            _moneyMediator?.Dispose();
            _mainBuildingMediator?.Dispose();
        }

        void Update()
        {
            _playerInput?.Update();
            _gamePlayerStateMachine?.Update();
            _builder.Update();
        }
    }
}
