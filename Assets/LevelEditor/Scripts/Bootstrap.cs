using GamePlay;
using LevelEditor.UI;
using Levels.Logic;
using Levels.TileControl;
using Services.CameraManipulation;
using Services.PlayerInput;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelEditor
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]private TileConfig _tileConfig;
        [SerializeField]private LevelEditorConfig _levelEditorConfig;
        [SerializeField]private Tilemap _groundTileMap;
        [SerializeField]private Tilemap _roadTileMap;
        [SerializeField]private LevelData _testLevelData;
        [SerializeField]private Transform _backGround;
        [SerializeField]private Camera _screenShotCamera;
        [SerializeField]private RenderTexture _screenShotRenderTexture;
        [SerializeField]private LevelSavingUI _levelSavingUI;

        private Level _level;
        private LevelEditor _levelEditor;
        private PlayerInput _playerInput;
        private LevelEditorMediator _levelEditorMediator;
        private TileController _tileController;
        private CameraManipulation _cameraManipulation;
        private CameraMediator _cameraMediator;
        private LevelIconMaker _levelIconMaker;
        private KeyCombinationHandler _undoKeyCombination;
        private KeyCombinationHandler _saveKeyCombination;
        private KeyHandler _fillKey;
        private KeyHandler _drawKey;

        private void Awake() 
        {
            _level = new Level(_testLevelData);
            _playerInput = new PlayerInput();
            _levelIconMaker = new LevelIconMaker(_screenShotCamera, _screenShotRenderTexture);
            _levelEditor = new LevelEditor(_level,_levelIconMaker);
            _tileController = new TileController(_tileConfig,_groundTileMap,_roadTileMap);

            _undoKeyCombination = new KeyCombinationHandler(_playerInput,_levelEditorConfig.UndoKeyCodes);
            _saveKeyCombination = new KeyCombinationHandler(_playerInput,_levelEditorConfig.SaveKeyCodes);
            _fillKey = new KeyHandler(_playerInput,_levelEditorConfig.FillKeyCode);
            _drawKey = new KeyHandler(_playerInput,_levelEditorConfig.DrawKeyCode);

            _levelEditorMediator = new LevelEditorMediator
            (
                _levelEditor,
                _playerInput, 
                _level, 
                _tileController,
                _undoKeyCombination, 
                _saveKeyCombination,
                _fillKey,
                _drawKey,
                _levelSavingUI
            );
            _cameraManipulation = new CameraManipulation(0.1f, Camera.main);
            _cameraMediator = new CameraMediator(_playerInput,_cameraManipulation);
        }

        private void Start() 
        {
            _backGround.position = new Vector3(_testLevelData.gridData.gridSize/2f,_testLevelData.gridData.gridSize/2f);
            _screenShotCamera.transform.Translate(new Vector3(_testLevelData.gridData.gridSize/2f,_testLevelData.gridData.gridSize/2f));
            _backGround.localScale = new Vector3(_testLevelData.gridData.gridSize,_testLevelData.gridData.gridSize);
        }

        private void OnDestroy() 
        {
            _undoKeyCombination.Dispose();
            _saveKeyCombination.Dispose();
            _fillKey.Dispose();
            _drawKey.Dispose();
            _cameraMediator.Dispose();
            _levelEditorMediator.Dispose();
        }

        private void Update() 
        {
            _playerInput.Update();
        }
    }
}
