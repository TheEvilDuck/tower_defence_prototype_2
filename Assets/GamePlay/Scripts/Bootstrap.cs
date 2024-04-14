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
using Levels.Tiles;
using Enemies.AI;
using Levels.View;

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
        private SpawnerMediator _spawnerMediator;
        private void Awake() 
        {
            _playerInput = new PlayerInput();
            _cameraManipulation = new CameraManipulation(_cameraSpeed, Camera.main);
            _cameraMediator = new CameraMediator(_playerInput,_cameraManipulation);
            _levelLoader = new LevelLoader();

            if (!_levelLoader.TryLoadLevel(_testLevelName, out LevelData levelData))
                throw new System.Exception("No level to load");

            _level = new Level(levelData);

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

            _enemyFactory = new EnemyFactory(_enemiesDatabase,_pathFinder, _level.Grid, spawners, _pathFindMultiplierDatabase);
            List<Wave> waves = new List<Wave>();

            foreach (WaveData waveData in levelData.waves)
            {
                waves.Add(new Wave(waveData));
            }

            _enemySpawner = new EnemySpawner(waves.ToArray(), levelData.firstWaveDelay,1f,_enemyFactory);

            _placableFactory = new PlacableFactory(_towersDatabase,_enemySpawner);

            //TODO this must be loaded from level
            AvailablePlacables availablePlacables = new AvailablePlacables()
            {
                placableIds = Enum.GetValues(typeof(PlacableEnum)).ConvertTo<PlacableEnum[]>()
            };

            _builder = new PlacableBuilder(availablePlacables, _placableFactory);

            _spawnerMediator = new SpawnerMediator(_enemySpawner, _pathFinder, _builder, spawners, _level.Grid.GridSize);


            _towersPanel.Init(_towersDatabase);

            _builderMediator = new BuilderMediator(_playerInput,_builder, _level.Grid,_towersPanel);
        }

        private void OnDestroy() 
        {
            _cameraMediator.Dispose();
            _levelAndTilesMediator.Dispose();
            _builderMediator.Dispose();
            _spawnerMediator.Dispose();
        }

        void Update()
        {
            _playerInput.Update();
            _enemySpawner.Update();
        }
    }
}
