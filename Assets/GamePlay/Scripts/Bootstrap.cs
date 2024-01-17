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

namespace GamePlay
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]private float _cameraSpeed = 4f;
        [SerializeField]private TileConfig _tileConfig;
        [SerializeField]private EnemyConfig _enemyConfig;
        [SerializeField]Tilemap _groundTileMap;
        [SerializeField]Tilemap _roadTileMap;
        [SerializeField]Enemy _testEnemyPrefab;
        private PlayerInput _playerInput;
        private CameraManipulation _cameraManipulation;
        private CameraMediator _cameraMediator;
        private Level _level;
        private TileController _tileController;
        private LevelAndTilesMediator _levelAndTilesMediator;
        private LevelLoader _levelLoader;
        private EnemySpawner _enemySpawner;
        private EnemyFactory _enemyFactory;
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

            _enemyFactory = new EnemyFactory(_testEnemyPrefab,_enemyConfig);
        }

        private void Start() 
        {
            
            
        }

        private void OnDestroy() 
        {
            _cameraMediator.Dispose();
            _levelAndTilesMediator.Dispose();
        }

        void Update()
        {
            _playerInput.Update();
            _enemySpawner.Update();
        }
    }
}
