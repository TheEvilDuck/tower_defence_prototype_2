using Common;
using Common.UI;
using GamePlay;
using LevelEditor.Selectors;
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
        [SerializeField]private ButtonsBar _buttonsBar;
        [SerializeField]private SettingsMenu _settingsMenu;
        [SerializeField]private WavesEditor _wavesEditor;
        [SerializeField]private LoadMenu _loadMenu;
        [SerializeField]private UIInputBlocker _uIInputBlocker;
        [SerializeField]private LevelIconButton _levelIconButtonPrefab;
        [SerializeField]private LevelSavingResultDatabase _levelSavingResultDatabase;

        private Level _level;
        private LevelEditor _levelEditor;
        private PlayerInput _playerInput;
        private LevelEditorMediator _levelEditorMediator;
        private TileController _tileController;
        private CameraManipulation _cameraManipulation;
        private CameraMediator _cameraMediator;
        private LevelIconMaker _levelIconMaker;
        private SceneLoader _sceneLoader;
        private KeyCombinationHandler _undoKeyCombination;
        private KeyCombinationHandler _saveKeyCombination;
        private KeyHandler _fillKey;
        private KeyHandler _drawKey;
        private KeyHandler _lineKey;
        private KeyHandler _drawToolKey;
        private KeyHandler _eraseToolKey;
        private BrushSelector _brushSelector;
        private FillSelector _fillSelector;
        private LineSelector _lineSelector;
        private Tool _drawTool;
        private Tool _eraseTool;
        private MenuParentsManager _menuParentsManager;
        private LevelEditorInputBlockerMediator _levelEditorInputBlockerMediator;
        private LevelIconsLoader _levelIconsLoader;
        private LevelLoader _levelLoader;
        private LevelIconsAndLevelLoaderMediator _levelIconsAndLevelLoaderMediator;
        private LevelSettingsMediator _levelSettingsMediator;
        private LevelSavingResultFabric _levelSavingResultFabric;

        private void Awake() 
        {
            _levelSavingResultFabric = new LevelSavingResultFabric(_levelSavingResultDatabase);

            _level = new Level(_testLevelData);
            _levelLoader = new LevelLoader();
            _playerInput = new PlayerInput();
            _levelIconMaker = new LevelIconMaker(_screenShotCamera, _screenShotRenderTexture);
            _levelEditor = new LevelEditor(_level,_levelIconMaker,_levelLoader,_levelSavingResultFabric);
            _tileController = new TileController(_tileConfig,_groundTileMap,_roadTileMap);
            _sceneLoader = new SceneLoader();
            _menuParentsManager = new MenuParentsManager();

            _menuParentsManager.Add(_settingsMenu);
            _menuParentsManager.Add(_wavesEditor);
            _menuParentsManager.Add(_loadMenu);

            _undoKeyCombination = new KeyCombinationHandler(_playerInput,_levelEditorConfig.UndoKeyCodes);
            _saveKeyCombination = new KeyCombinationHandler(_playerInput,_levelEditorConfig.SaveKeyCodes);
            _fillKey = new KeyHandler(_playerInput,_levelEditorConfig.FillKeyCode);
            _drawKey = new KeyHandler(_playerInput,_levelEditorConfig.DrawKeyCode);
            _lineKey = new KeyHandler(_playerInput,_levelEditorConfig.LineKeyCode);
            _drawToolKey = new KeyHandler(_playerInput,_levelEditorConfig.AddGroundKeyCode);
            _eraseToolKey = new KeyHandler(_playerInput,_levelEditorConfig.DeleteGroundKeyCode);

            _brushSelector = new BrushSelector(_playerInput, _level.Grid);
            _fillSelector = new FillSelector(_playerInput, _level.Grid);
            _lineSelector = new LineSelector(_playerInput,_level.Grid);
            _drawTool = new Tool(new DrawCommandsFactory(_level.Grid));
            _eraseTool = new Tool(new EraseCommandFactory(_level.Grid));

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
                _lineKey,
                _drawToolKey,
                _eraseToolKey,
                _levelSavingUI,
                _drawTool,
                _eraseTool,
                _fillSelector,
                _brushSelector,
                _lineSelector,
                _buttonsBar,
                _sceneLoader,
                _menuParentsManager,
                _settingsMenu,
                _wavesEditor,
                _loadMenu
                
            );
            _cameraManipulation = new CameraManipulation(0.1f, Camera.main);
            _cameraMediator = new CameraMediator(_playerInput,_cameraManipulation);

            _levelEditorInputBlockerMediator = new LevelEditorInputBlockerMediator(_uIInputBlocker, _playerInput);

            _levelIconsLoader = new LevelIconsLoader(_levelLoader,_loadMenu.ParentToIcons,_levelIconButtonPrefab);

            _levelIconsAndLevelLoaderMediator = new LevelIconsAndLevelLoaderMediator(_levelLoader,_levelIconsLoader, _levelEditor,_level);

            _levelSettingsMediator = new LevelSettingsMediator(_settingsMenu, _levelEditor);

            
        }

        private void Start() 
        {
            _backGround.position = new Vector3(_testLevelData.gridData.gridSize/2f,_testLevelData.gridData.gridSize/2f);
            _screenShotCamera.transform.Translate(new Vector3(_testLevelData.gridData.gridSize/2f,_testLevelData.gridData.gridSize/2f));
            _backGround.localScale = new Vector3(_testLevelData.gridData.gridSize,_testLevelData.gridData.gridSize);
        }

        private void OnDestroy() 
        {
            _drawTool.Dispose();
            _eraseTool.Dispose();
            _undoKeyCombination.Dispose();
            _saveKeyCombination.Dispose();
            _fillKey.Dispose();
            _drawKey.Dispose();
            _lineKey.Dispose();
            _drawToolKey.Dispose();
            _eraseToolKey.Dispose();
            _cameraMediator.Dispose();
            _levelEditorMediator.Dispose();
            _brushSelector.Dispose();
            _fillSelector.Dispose();
            _lineSelector.Disable();
            _levelEditorInputBlockerMediator.Dispose();
            _levelIconsAndLevelLoaderMediator.Dispose();
            _levelSettingsMediator.Dispose();
        }

        private void Update() 
        {
            _playerInput.Update();
        }
    }
}
