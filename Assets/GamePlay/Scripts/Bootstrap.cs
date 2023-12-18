using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services.PlayerInput;
using Services.CameraManipulation;
using Levels.TileControl;
using UnityEngine.Tilemaps;
using Levels.Logic;

namespace GamePlay
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]private float _cameraSpeed = 4f;
        [SerializeField]private LevelData _testLevelData;
        [SerializeField]private TileConfig _tileConfig;
        [SerializeField]Tilemap _groundTileMap;
        [SerializeField]Tilemap _roadTileMap;
        private PlayerInput _playerInput;
        private CameraManipulation _cameraManipulation;
        private CameraMediator _cameraMediator;
        private Level _level;
        private TileController _tileController;
        private LevelAndTilesMediator _levelAndTilesMediator;
        private void Awake() 
        {
            _playerInput = new PlayerInput();
            _cameraManipulation = new CameraManipulation(_cameraSpeed, Camera.main);
            _cameraMediator = new CameraMediator(_playerInput,_cameraManipulation);

            _level = new Level(_testLevelData);

            _tileController = new TileController(_tileConfig,_groundTileMap,_roadTileMap);

            _levelAndTilesMediator = new LevelAndTilesMediator(_tileController,_level);
        }

        private void Start() 
        {
            _level.Test();
        }

        private void OnDestroy() 
        {
            _cameraMediator.Dispose();
            _levelAndTilesMediator.Dispose();
        }

        void Update()
        {
            _playerInput.Update();
        }
    }
}
