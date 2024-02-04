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
        private void Awake() 
        {
            _playerInput = new PlayerInput();
            _cameraManipulation = new CameraManipulation(_cameraSpeed, Camera.main);
            _cameraMediator = new CameraMediator(_playerInput,_cameraManipulation);
            _levelLoader = new LevelLoader();

            if (!_levelLoader.TryLoadLevel("test", out LevelData levelData))
                throw new System.Exception("No level to load");

            _level = new Level(levelData);

            _tileController = new TileController(_tileConfig,_groundTileMap,_roadTileMap);

            _levelAndTilesMediator = new LevelAndTilesMediator(_tileController,_level);

            _level.Grid.FillFromGridData(levelData.gridData);

            _enemyFactory = new EnemyFactory(_enemiesDatabase);
            //TO DO load wavedata from leveldata
            Wave[] waves = new Wave[1];
            EnemyData testEnemyData = new EnemyData();
            testEnemyData.id = EnemyEnum.Gray;
            WaveEnemyData testWaveEnemyData = new WaveEnemyData();
            testWaveEnemyData.count = 1;
            testWaveEnemyData.enemyData = testEnemyData;
            WaveEnemyData[] waveEnemyDatas = new WaveEnemyData[1];
            waveEnemyDatas[0] = testWaveEnemyData;
            waves[0] = new Wave(1,waveEnemyDatas);
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
        }

        private void OnDestroy() 
        {
            _cameraMediator.Dispose();
            _levelAndTilesMediator.Dispose();
            _builderMediator.Dispose();
        }

        void Update()
        {
            _playerInput.Update();
            _enemySpawner.Update();
        }
    }
}
