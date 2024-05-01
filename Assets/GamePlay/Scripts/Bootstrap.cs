using System.Collections.Generic;
using UnityEngine;
using Services.PlayerInput;
using Services.CameraManipulation;
using UnityEngine.Tilemaps;
using Levels.Logic;
using Enemies;
using Waves;
using Builder;
using Towers;
using Levels.Tiles;
using Enemies.AI;
using StateMachine = Common.States.StateMachine;
using GamePlay.UI;
using Common;
using System.Collections;
using GamePlay.Mediators;
using GamePlay.Player;
using GamePlay.EnemiesSpawning;
using Common.UI.InputBlocking;
using GamePlay.States;
using System.Linq;

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
        [SerializeField] private Camera _renderCamera;
        [SerializeField] private RenderTexture _renderTexture;
        [SerializeField] private Transform _placeForRender;
        [SerializeField] private PlacablePreview _placablePreviewPrefab;
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
        private InputBlockerMediator _inputBlockerMediator;
        private PauseMediator _pauseMediator;
        private PlayerStatsUpdater _playerStatsUpdater;
        private BuildPossibilityChecker _buildPossibilityChecker;
        private MoneyMediator _moneyMediator;
        private MainBuldingMediator _mainBuildingMediator;
        private GameObjectIconProvider<PlacableEnum> _towersIcons;
        private GameObjectIconsMaker _iconsMaker;

        private void Awake() {
            Application.targetFrameRate = 60;
        }
        private IEnumerator Start() 
        {
            var wait = new WaitForEndOfFrame();
            _towersIcons = new GameObjectIconProvider<PlacableEnum>();

            _iconsMaker = new GameObjectIconsMaker(_renderCamera, _renderTexture, _placeForRender);

            foreach (var idAndConfig in _towersDatabase.Items)
            {
                Texture2D texture = _iconsMaker.Get<Placable>(idAndConfig.Value.Prefab);
                _towersIcons.FillWith(idAndConfig.Key, texture);
                yield return wait;
            }

            _renderCamera.enabled = false;

            _pausableManager = new PausableManager();
            _playerInput = new PlayerInput();
            _cameraManipulation = new CameraManipulation(_cameraSpeed, Camera.main);
            _cameraMediator = new CameraMediator(_playerInput,_cameraManipulation);
            _levelLoader = new LevelLoader();

            if (!_levelLoader.TryLoadLevel(PlayerSettingsData.LoadChosenMapName(), out LevelData levelData))
                throw new System.Exception("No level to load");

            _level = new Level(levelData);
            PlacablesContainer placablesContainer = new PlacablesContainer(_level.Grid);
            PlayerStats playerStats = new PlayerStats(levelData.startMoney);

            _tileController = new TileController(_tileDatabase,_groundTileMap,_roadTileMap);
            _levelAndTilesMediator = new LevelAndTilesMediator(_tileController,_level);

            List<Vector2Int> spawnerPositions = new List<Vector2Int>();
            _level.UpdateGridData(levelData.gridData);

            foreach(int spawnerIndex in levelData.spawnerPlaces)
            {
                Vector2Int spawnerPosition = _level.Grid.ConvertIntToVector2Int(spawnerIndex);
                Debug.Log($"Registered spawner at {spawnerPosition}");
                spawnerPositions.Add(spawnerPosition);
            }

            
            _pathFinder = new PathFinder(_level.Grid,_pathFindMultiplierDatabase);

            Spawners spawners = new Spawners(spawnerPositions.ToArray(), _pathFinder);

            

            _enemyFactory = new EnemyFactory(_enemiesDatabase, _level.Grid, spawners, _pathFindMultiplierDatabase,placablesContainer);
            List<Wave> waves = new List<Wave>();

            foreach (WaveData waveData in levelData.waves)
            {
                waves.Add(new Wave(waveData));
            }

            

            _enemySpawner = new EnemySpawner(waves.ToArray(), levelData.firstWaveDelay,1f,_enemyFactory);

            _placableFactory = new PlacableFactory(_towersDatabase,_enemySpawner, playerStats);

            List<PlacableEnum> availablePlacables = new List<PlacableEnum>();

            foreach (var id in levelData.allowedPlacables)
                availablePlacables.Add(id);

            availablePlacables.Add(PlacableEnum.MainBuilding);

            _builder = new PlacableBuilder(availablePlacables.ToArray(), _placableFactory, true, _placablePreviewPrefab, _towersIcons, placablesContainer);

            _towersPanel.Init(_towersDatabase, _towersIcons, availablePlacables.ToArray());

            _builderMediator = new BuilderMediator(_playerInput,_builder, _level.Grid,_towersPanel, placablesContainer);

            SceneLoader sceneLoader = new SceneLoader();

            _gamePlayerStateMachine = new StateMachine();
            

            _inputBlockerMediator = new InputBlockerMediator(_inputBlocker, _playerInput);

            MenuParentsManager pauseMenus = new MenuParentsManager();
            pauseMenus.Add(_pauseButton);
            pauseMenus.Add(_pauseView);

            _pausableManager.Add(_enemySpawner);
            _pausableManager.Add(_playerInput);
            _pausableManager.Add(_builder);

            _pauseMediator = new PauseMediator(_pausableManager, _pauseView, _pauseButton, pauseMenus, sceneLoader);

            pauseMenus.HideAll();
            pauseMenus.Show(_pauseButton);

            PrepareState prepareState = new PrepareState(_gamePlayerStateMachine, _builder, spawners, false, _level.Grid.GridSize);
            EnemySpawnState enemySpawnState = new EnemySpawnState(_gamePlayerStateMachine, _enemySpawner, _builder, placablesContainer, _towersPanel);
            LoseState loseState = new LoseState(_gamePlayerStateMachine, _gameOverView, sceneLoader, pauseMenus, _builder, _towersPanel);
            WinState winState = new WinState(_gamePlayerStateMachine, _gameOverView, sceneLoader, pauseMenus, _builder, _towersPanel);

            _gamePlayerStateMachine.AddState(prepareState);
            _gamePlayerStateMachine.AddState(enemySpawnState);
            _gamePlayerStateMachine.AddState(loseState);
            _gamePlayerStateMachine.AddState(winState);

            _playerStatsUpdater = new PlayerStatsUpdater(playerStats, _builder, _towersDatabase);
            _buildPossibilityChecker = new BuildPossibilityChecker(playerStats, _builder, _towersDatabase);

            _moneyMediator = new MoneyMediator(playerStats, _moneyView);

            _mainBuildingMediator = new MainBuldingMediator(_builder, _healthUI);

            MainBuildingAndSpawnerMediator mainBuildingAndSpawnerMediator = new MainBuildingAndSpawnerMediator(_builder, spawners, levelData.gridData.gridSize);
            _towersPanel.HideAllExceptMainBuilding();

            _builder.BuildFromPlacableDatas(levelData.placables, _level.Grid);

            
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
            _pathFinder.Dispose();
        }

        void Update()
        {
            _playerInput?.Update();
            _gamePlayerStateMachine?.Update();
            _builder?.Update();
        }
    }
}
