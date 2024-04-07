using System.Collections;
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
using Unity.VisualScripting;

namespace GamePlay
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]private float _cameraSpeed = 4f;
        [SerializeField]private TileConfig _tileConfig;
        [SerializeField]private EnemiesDatabase _enemiesDatabase;
        [SerializeField]private Tilemap _groundTileMap;
        [SerializeField]private Tilemap _roadTileMap;
        [SerializeField]private TowersDatabase _towersDatabase;
        [SerializeField]private TowersPanel _towersPanel;
        [SerializeField]private string _testLevelName;
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
        private void Awake() 
        {
            _playerInput = new PlayerInput();
            _cameraManipulation = new CameraManipulation(_cameraSpeed, Camera.main);
            _cameraMediator = new CameraMediator(_playerInput,_cameraManipulation);
            _levelLoader = new LevelLoader();

            if (!_levelLoader.TryLoadLevel(_testLevelName, out LevelData levelData))
                throw new System.Exception("No level to load");

            _level = new Level(levelData);

            _tileController = new TileController(_tileConfig,_groundTileMap,_roadTileMap);

            _levelAndTilesMediator = new LevelAndTilesMediator(_tileController,_level);

            _level.Grid.FillFromGridData(levelData.gridData);

            _pathFinder = new PathFinder(_level.Grid);

            _enemyFactory = new EnemyFactory(_enemiesDatabase,_pathFinder);
            //TO DO load wavedata from leveldata
            Wave[] waves = new Wave[1];
            EnemyData testEnemyData = new EnemyData();
            testEnemyData.id = EnemyEnum.Gray;
            WaveEnemyData testWaveEnemyData = new WaveEnemyData();
            testWaveEnemyData.count = 1;
            testWaveEnemyData.enemyData = testEnemyData;
            WaveEnemyData[] waveEnemyDatas = new WaveEnemyData[1];
            waveEnemyDatas[0] = testWaveEnemyData;
            WaveData waveData = new WaveData();
            waveData.timeToTheNextWave = 10;
            waveData.waveEnemyData = waveEnemyDatas;
            waves[0] = new Wave(waveData);
            //TO DO remove magic values
            _enemySpawner = new EnemySpawner(waves, levelData.firstWaveDelay,1f,_enemyFactory);
            _placableFactory = new PlacableFactory(_towersDatabase,_enemySpawner);

            //TODO this must be loaded from level
            AvailablePlacables availablePlacables = new AvailablePlacables()
            {
                placableIds = Enum.GetValues(typeof(PlacableEnum)).ConvertTo<PlacableEnum[]>()
            };

            _builder = new PlacableBuilder(availablePlacables, _placableFactory);

            _towersPanel.Init(_towersDatabase);

            _builderMediator = new BuilderMediator(_playerInput,_builder, _level.Grid,_towersPanel);

            
            _playerInput.mouseRightClicked+=OnPlayerInputRightClicked;
        }

        private void OnDestroy() 
        {
            _cameraMediator.Dispose();
            _levelAndTilesMediator.Dispose();
            _builderMediator.Dispose();
            _playerInput.mouseRightClicked-=OnPlayerInputRightClicked;
        }

        void Update()
        {
            _playerInput.Update();
            _enemySpawner.Update();
        }

        private void OnPlayerInputRightClicked(Vector2 position)
        {
            Vector2Int gridPosition = _level.Grid.WorldPositionToGridPosition(position);
        

            foreach (Enemy enemy in _enemySpawner.Enemies)
            {
                Vector2Int enemyGridPosition = _level.Grid.WorldPositionToGridPosition(enemy.Position);

                if (_pathFinder.TryFindPath(enemyGridPosition,gridPosition,out List<Vector2Int> result, true))
                {
                    List<Vector2> path = new List<Vector2>();

                    foreach (Vector2Int gridPoint in result)
                    {
                        Vector2 worldPoint = _level.Grid.GridPositionToWorldPosition(gridPoint);
                        path.Add(worldPoint);
                    }

                    enemy.UpdatePath(path);
                }
            }
        }
    }
}
