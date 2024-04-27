using System.Collections;
using System.Linq;
using Builder;
using Common;
using Common.UI;
using Common.UI.InputBlocking;
using Enemies;
using GamePlay;
using GamePlay.EnemiesSpawning;
using GamePlay.Mediators;
using GamePlay.Player;
using LevelEditor.Commands.Factory;
using LevelEditor.LevelSaving;
using LevelEditor.Mediators;
using LevelEditor.Selectors;
using LevelEditor.Tools;
using LevelEditor.UI;
using LevelEditor.UI.EnemiesSelection;
using LevelEditor.UI.Toolbars;
using LevelEditor.UI.WavesEditing;
using Levels.Logic;
using Levels.Tiles;
using Levels.View;
using Services.CameraManipulation;
using Services.PlayerInput;
using Towers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelEditor
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]private TileDatabase _tileDatabase;
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
        [SerializeField]private GameObject _spawerPrefab;
        [SerializeField]private SelectorsToolBar _selectorsToolBar;
        [SerializeField]private TilesToolBar _tilesToolBar;
        [SerializeField]private DrawTypeToolBar _drawTypeToolBar;
        [SerializeField] private ToolButtons _toolButtons;
        [SerializeField] private EnemiesSelector _enemiesSelector;
        [SerializeField] private Camera _renderTextureCamera;
        [SerializeField] private RenderTexture _iconsRenderTexture;
        [SerializeField] private Transform _iconsGameObjectTransform;
        [SerializeField] private EnemiesDatabase _enemiesDatabase;
        [SerializeField] private TowersDatabase _towersDatabase;
        [SerializeField] private TowersMenu _towersMenu;
        [SerializeField] private TowersSettingsMenu _towersSettingsMenu;
        [SerializeField] private PlacablePreview _placablePreviewPrefab;
        [SerializeField] private TowersPlaceMenu _towersPlaceMenu;
        [SerializeField] private UndoRedoButtons _undoRedoButtons;

        private Level _level;
        private LevelEditor _levelEditor;
        private PlayerInput _playerInput;
        private LevelEditorMediator _levelEditorMediator;
        private TileController _tileController;
        private CameraManipulation _cameraManipulation;
        private CameraMediator _cameraMediator;
        private IconsMaker _levelIconMaker;
        private SceneLoader _sceneLoader;
        private BrushSelector _brushSelector;
        private FillSelector _fillSelector;
        private LineSelector _lineSelector;
        private SpawnerPlacamentSelector _spawnerPlacementSelector;
        private Tool _drawTool;
        private Tool _eraseTool;
        private Tool _spawnerPlacer;
        private Tool _placablePlacer;
        private MenuParentsManager _menuParentsManager;
        private InputBlockerMediator _levelEditorInputBlockerMediator;
        private LevelIconsLoader _levelIconsLoader;
        private LevelLoader _levelLoader;
        private LevelIconsAndLevelLoaderMediator _levelIconsAndLevelLoaderMediator;
        private LevelSettingsMediator _levelSettingsMediator;
        private LevelSavingResultFactory _levelSavingResultFabric;
        private DrawCommandsFactory _drawCommandsFactory;
        private SpawnersView _spawnersView;
        private ToolBarMediator _toolBarMediator;
        private GameObjectIconsMaker _gameObjectIconsMaker;
        private GameObjectIconProvider<EnemyEnum> _enemiesIcons;
        private GameObjectIconProvider<PlacableEnum> _towersIcons;
        private ButtonsBarMediator _buttonsBarMediator;
        private PlacableBuilder _placableBuilder;
        private EditorBuilderMediator _builderMediator;
        private UndoRedoMediator _undoRedoMediator;

        private IEnumerator Start() 
        {
            var wait = new WaitForEndOfFrame();

            _gameObjectIconsMaker = new GameObjectIconsMaker(_renderTextureCamera, _iconsRenderTexture, _iconsGameObjectTransform);

            _enemiesIcons = new GameObjectIconProvider<EnemyEnum>();
            _towersIcons = new GameObjectIconProvider<PlacableEnum>();

            foreach (var item in _enemiesDatabase.Items)
            {
                Texture2D texture = _gameObjectIconsMaker.Get(item.Value.Prefab);
                _enemiesIcons.FillWith(item.Key, texture);
                yield return wait;
            }

            RenderTexture.active = _iconsRenderTexture;
            _renderTextureCamera.enabled = true;

            foreach (var item in _towersDatabase.Items)
            {
                Texture2D texture = _gameObjectIconsMaker.Get(item.Value.Prefab);
                _towersIcons.FillWith(item.Key, texture);
                yield return wait;
            }

            RenderTexture.active = _iconsRenderTexture;
            _renderTextureCamera.enabled = true;

            _levelSavingResultFabric = new LevelSavingResultFactory(_levelSavingResultDatabase);
            SpawnerPositions spawnerPositions = new SpawnerPositions();

            _level = new Level(_testLevelData);
            _levelLoader = new LevelLoader();
            _playerInput = new PlayerInput();
            _levelIconMaker = new IconsMaker(_screenShotCamera, _screenShotRenderTexture);
            _levelEditor = new LevelEditor(_level,_levelIconMaker,_levelLoader,_levelSavingResultFabric, spawnerPositions, _towersPlaceMenu);
            _tileController = new TileController(_tileDatabase,_groundTileMap,_roadTileMap);
            _sceneLoader = new SceneLoader();
            _menuParentsManager = new MenuParentsManager();

            _menuParentsManager.Add(_settingsMenu);
            _menuParentsManager.Add(_wavesEditor);
            _menuParentsManager.Add(_loadMenu);
            _menuParentsManager.Add(_toolButtons);
            _menuParentsManager.Add(_towersMenu);

            _drawCommandsFactory = new DrawCommandsFactory(_level.Grid);

            _brushSelector = new BrushSelector(_playerInput, _level.Grid);
            _fillSelector = new FillSelector(_playerInput, _level.Grid);
            _lineSelector = new LineSelector(_playerInput,_level.Grid);
            _spawnerPlacementSelector = new SpawnerPlacamentSelector(_playerInput, _level.Grid);
            _drawTool = new Tool(_drawCommandsFactory);
            _eraseTool = new Tool(new EraseCommandFactory(_level.Grid));
            _spawnerPlacer = new Tool(new AddSpawnerCommandFactory(_level.Grid, spawnerPositions));
            _spawnersView = new SpawnersView(_spawerPrefab, _level.Grid.CellSize, _level.Grid.GridSize);

            _levelEditorMediator = new LevelEditorMediator
            (
                _levelEditor, 
                _level, 
                _tileController,
                _levelSavingUI,
                _drawTool,
                _brushSelector,
                _wavesEditor,
                spawnerPositions,
                _spawnersView,
                _towersSettingsMenu
            );
            _cameraManipulation = new CameraManipulation(0.1f, Camera.main);
            _cameraMediator = new CameraMediator(_playerInput,_cameraManipulation);

            _levelEditorInputBlockerMediator = new InputBlockerMediator(_uIInputBlocker, _playerInput);

            _levelIconsLoader = new LevelIconsLoader(_levelLoader,_loadMenu.ParentToIcons,_levelIconButtonPrefab);

            

            _levelSettingsMediator = new LevelSettingsMediator(_settingsMenu, _levelEditor);

            _wavesEditor.Init(_enemiesIcons);

            _selectorsToolBar.Init(_brushSelector, _fillSelector, _lineSelector);

            _tilesToolBar.Init();
            _drawTypeToolBar.Init(_drawTool, _eraseTool);
            _toolButtons.Init();
            _towersMenu.Init(_towersIcons);

            _toolBarMediator = new ToolBarMediator(_selectorsToolBar, _levelEditor, _drawCommandsFactory, _tilesToolBar, _drawTypeToolBar);
            _enemiesSelector.Init(_enemiesIcons);

            _backGround.position = new Vector3(_testLevelData.gridData.gridSize/2f,_testLevelData.gridData.gridSize/2f);
            _screenShotCamera.transform.Translate(new Vector3(_testLevelData.gridData.gridSize/2f,_testLevelData.gridData.gridSize/2f));
            _backGround.localScale = new Vector3(_testLevelData.gridData.gridSize,_testLevelData.gridData.gridSize);

            

            var enemySpawner = new EnemySpawner(null, 0, 0, null);
            var playerStats = new PlayerStats(0);
            var placableFactory = new PlacableFactory(_towersDatabase, enemySpawner,playerStats);
            var placableContainer = new PlacablesContainer(_level.Grid);
            _placableBuilder = new PlacableBuilder(_towersDatabase.GetAllIds(), placableFactory, false, _placablePreviewPrefab, _towersIcons, placableContainer);

            _levelIconsAndLevelLoaderMediator = new LevelIconsAndLevelLoaderMediator(_levelLoader,_levelIconsLoader, _levelEditor,_level,_wavesEditor,_settingsMenu, spawnerPositions, _towersSettingsMenu, _placableBuilder);
        
            var placableCommandFactory = new AddPlacableCommandFactory(_level.Grid, _placableBuilder, placableContainer);
            _placablePlacer = new Tool(placableCommandFactory);

            _builderMediator = new EditorBuilderMediator(_towersPlaceMenu, _placableBuilder, _level.Grid, _playerInput, placableContainer, placableCommandFactory);

            _buttonsBarMediator = new ButtonsBarMediator(
                _buttonsBar, 
                _sceneLoader,
                _menuParentsManager,
                _settingsMenu,
                _wavesEditor,
                _loadMenu,
                _towersMenu,
                _toolButtons,
                _levelEditor,
                _brushSelector,
                _drawTool,
                _level.Grid,
                _levelSavingUI,
                _spawnerPlacementSelector,
                _spawnerPlacer,
                _towersSettingsMenu,
                _placablePlacer,
                placableContainer,
                _towersPlaceMenu
            );

            _undoRedoMediator = new UndoRedoMediator(_levelEditor, _undoRedoButtons);
        }

        private void OnDestroy() 
        {
            _drawTool.Dispose();
            _eraseTool.Dispose();
            _spawnerPlacer.Dispose();
            _cameraMediator.Dispose();
            _levelEditorMediator.Dispose();
            _brushSelector.Dispose();
            _fillSelector.Dispose();
            _lineSelector.Disable();
            _spawnerPlacementSelector.Disable();
            _levelEditorInputBlockerMediator.Dispose();
            _levelIconsAndLevelLoaderMediator.Dispose();
            _levelSettingsMediator.Dispose();
            _toolBarMediator.Dispose();
            _buttonsBarMediator.Dispose();
            _builderMediator.Dispose();
            _undoRedoMediator.Dispose();
        }

        private void Update() 
        {
            _playerInput?.Update();
        }
    }
}
