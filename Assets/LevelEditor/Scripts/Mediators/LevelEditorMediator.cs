using System;
using System.Linq;
using Common;
using LevelEditor.Selectors;
using LevelEditor.UI;
using Levels.Logic;
using Levels.TileControl;
using Levels.Tiles;
using Levels.View;
using Services.PlayerInput;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LevelEditorMediator: IDisposable
    {
        private LevelEditor _levelEditor;
        private PlayerInput _playerInput;
        private Level _level;
        private TileController _tileController;
        private LevelIconMaker _levelIconMaker;
        private KeyCombinationHandler _undoKeyCombination;
        private KeyCombinationHandler _saveKeyCombination;
        private KeyHandler _fillKey;
        private KeyHandler _drawKey;
        private KeyHandler _lineKey;
        private KeyHandler _drawToolKey;
        private KeyHandler _eraseToolKey;
        private KeyHandler _switchTile;
        private LevelSavingUI _levelSavingUI;
        private Tool _drawTool;
        private Tool _eraseTool;
        private FillSelector _fillSelector;
        private BrushSelector _brushSelector;
        private LineSelector _lineSelector;
        private ButtonsBar _buttonsBar;
        private SceneLoader _sceneLoader;
        private MenuParentsManager _menuParentsManager;
        private SettingsMenu _settingsMenu;
        private WavesEditor _waveEditor;
        private LoadMenu _loadMenu;
        private DrawCommandsFactory _drawCommandsFactory;
        private KeyHandler _toggleRoadPlacingKey;
        private Tool _spawnerPlacer;
        private SpawnerPlacamentSelector _spawnerPlacementSelector;
        private SpawnerPositions _spawnerPositions;
        private SpawnersView _spawnersView;
        private ToolButtons _toolButtons;

        public LevelEditorMediator
        (
            LevelEditor levelEditor, 
            PlayerInput playerInput, 
            Level level, 
            TileController tileController,
            KeyCombinationHandler undoKeyCombination, 
            KeyCombinationHandler saveKeyCombination,
            KeyHandler fillKey,
            KeyHandler drawKey,
            KeyHandler lineKey,
            KeyHandler drawToolKey,
            KeyHandler eraseToolKey,
            KeyHandler switchTile,
            LevelSavingUI levelSavingUI,
            Tool drawTool,
            Tool eraseTool,
            FillSelector fillSelector,
            BrushSelector brushSelector,
            LineSelector lineSelector,
            ButtonsBar buttonsBar,
            SceneLoader sceneLoader,
            MenuParentsManager menuParentsManager,
            SettingsMenu settingsMenu,
            WavesEditor wavesEditor,
            LoadMenu loadMenu,
            DrawCommandsFactory drawCommandsFactory,
            KeyHandler toggleRoadPlacingKey,
            Tool spawnerPlacer,
            SpawnerPlacamentSelector spawnerPlacementSelector,
            SpawnerPositions spawnerPositions,
            SpawnersView spawnersView,
            ToolButtons toolButtons
        )
        {
            _levelEditor = levelEditor;
            _playerInput = playerInput;
            _level = level;
            _tileController = tileController;
            _undoKeyCombination = undoKeyCombination;
            _saveKeyCombination = saveKeyCombination;
            _fillKey = fillKey;
            _drawKey = drawKey;
            _lineKey = lineKey;
            _drawToolKey = drawToolKey;
            _eraseToolKey = eraseToolKey;
            _levelSavingUI = levelSavingUI;
            _drawTool = drawTool;
            _eraseTool = eraseTool;
            _fillSelector = fillSelector;
            _brushSelector = brushSelector;
            _lineSelector = lineSelector;
            _buttonsBar = buttonsBar;
            _sceneLoader = sceneLoader;
            _menuParentsManager = menuParentsManager;
            _settingsMenu = settingsMenu;
            _waveEditor = wavesEditor;
            _loadMenu = loadMenu;
            _switchTile = switchTile;
            _drawCommandsFactory = drawCommandsFactory;
            _toggleRoadPlacingKey = toggleRoadPlacingKey;
            _spawnerPlacer = spawnerPlacer;
            _spawnerPlacementSelector = spawnerPlacementSelector;
            _spawnerPositions = spawnerPositions;
            _spawnersView = spawnersView;
            _toolButtons = toolButtons;

            _levelEditor.ChangeSelector(_brushSelector);
            _levelEditor.ChangeTool(_drawTool);
        
            _level.Grid.cellChanged+=OnCellChanged;
            _level.Grid.cellRemoved+=OnCellRemoved;
            _level.Grid.cellAdded+=OnCellAdded;
            _levelEditor.levelSaveTried+=OnLevelSaveTried;
            _undoKeyCombination.Down+=OnUndoKeyCombinationDown;
            _saveKeyCombination.Down+=OnSaveKeyCombinationDown;
            _drawKey.Down+=OnDrawKeyDown;
            _fillKey.Down+=OnFillKeyDown;
            _lineKey.Down+=OnLineKeyDown;
            _drawToolKey.Down+=OnDrawToolKeyDown;
            _eraseToolKey.Down+=OnEraseToolKeyDown;
            _buttonsBar.saveButtonPressed+=OnSaveKeyCombinationDown;
            _buttonsBar.exitButtonPressed+=OnExitButtonPressed;
            _buttonsBar.settingsButtonPressed+=OnSettingsButtonPressed;
            _buttonsBar.wavesButtonPressed+=OnWavesButtonPressed;
            _buttonsBar.loadButtonPressed+=OnLoadButtonPressed;
            _buttonsBar.deleteButtonPressed+=OnDeleteButtonPressed;
            _buttonsBar.newButtonPressed+=OnNewButtonPressed;
            _buttonsBar.spawnerButtonPressed+=OnSpawnerButtonPressed;
            _buttonsBar.toolsButtonPressed += OnToolButtonPresed;
            _toggleRoadPlacingKey.Down+=OnTogglePlacingRoadPressed;
            _spawnerPositions.placed+=OnSpawnerAddedAt;
            _spawnerPositions.removed+=OnSpawnerRemovedAt;
        }


        public void Dispose()
        {
            _level.Grid.cellChanged-=OnCellChanged;
            _undoKeyCombination.Down-=OnUndoKeyCombinationDown;
            _saveKeyCombination.Down-=OnSaveKeyCombinationDown;
            _level.Grid.cellRemoved-=OnCellRemoved;
            _level.Grid.cellAdded-=OnCellAdded;
            _levelEditor.levelSaveTried-=OnLevelSaveTried;
            _drawKey.Down-=OnDrawKeyDown;
            _fillKey.Down-=OnFillKeyDown;
            _lineKey.Down-=OnLineKeyDown;
            _drawToolKey.Down-=OnDrawToolKeyDown;
            _eraseToolKey.Down-=OnEraseToolKeyDown;
            _buttonsBar.saveButtonPressed-=OnSaveKeyCombinationDown;
            _buttonsBar.exitButtonPressed-=OnExitButtonPressed;
            _buttonsBar.settingsButtonPressed-=OnSettingsButtonPressed;
            _buttonsBar.wavesButtonPressed-=OnWavesButtonPressed;
            _buttonsBar.loadButtonPressed-=OnLoadButtonPressed;
            _buttonsBar.deleteButtonPressed-=OnDeleteButtonPressed;
            _buttonsBar.newButtonPressed-=OnNewButtonPressed;
            _buttonsBar.toolsButtonPressed -= OnToolButtonPresed;
            _buttonsBar.spawnerButtonPressed-=OnSpawnerButtonPressed;
            _toggleRoadPlacingKey.Down-=OnTogglePlacingRoadPressed;
            _spawnerPositions.placed-=OnSpawnerAddedAt;
            _spawnerPositions.removed-=OnSpawnerRemovedAt;
        }

        private void OnFillKeyDown() => _levelEditor.ChangeSelector(_fillSelector);
        private void OnDrawKeyDown() => _levelEditor.ChangeSelector(_brushSelector);
        private void OnLineKeyDown() => _levelEditor.ChangeSelector(_lineSelector);

        private void OnDrawToolKeyDown() => _levelEditor.ChangeTool(_drawTool);

        private void OnEraseToolKeyDown() => _levelEditor.ChangeTool(_eraseTool);

        private void OnCellChanged(Vector2Int cellId, CellData cell)
        {
            if (cell.HasRoad)
                _tileController.DrawRoadAt(cellId);
            else
                _tileController.RemoveRoadAt(cellId);
        }

        private void OnCellAdded(Vector2Int cellId, TileType tileType)
        {
            _tileController.DrawAt(cellId, tileType);
        }

        private void OnCellRemoved(Vector2Int cellId)
        {
            _tileController.RempoveAt(cellId);
            _tileController.RemoveRoadAt(cellId);
        }

        private void OnLevelSaveTried(LevelSavingResult levelSavingResult)
        {
            if (levelSavingResult.Type == ResultType.MapOverride)
            {
                _levelSavingUI.ShowCancelButton();
                _levelSavingUI.OkButtonPressed.AddListener(OnOkSaveButtonClickedWhileOverriding);
            }

            _levelSavingUI.Show();
            _levelSavingUI.OnLevelSaveTried(levelSavingResult.message);
        }

        private void OnUndoKeyCombinationDown() => _levelEditor.UndoLastCommand();
        private void OnSaveKeyCombinationDown()
        {
            _waveEditor.FillWaveDatasWithEnemyDatas();
            _levelSavingUI.Show();
            _levelEditor.SaveLevel(_waveEditor.WaveDatas.ToArray(), false);
        }
        private void OnExitButtonPressed() => _sceneLoader.LoadMainMenu();
        private void OnSettingsButtonPressed() => _menuParentsManager.Show(_settingsMenu);
        private void OnWavesButtonPressed() => _menuParentsManager.Show(_waveEditor);
        private void OnLoadButtonPressed() => _menuParentsManager.Show(_loadMenu);
        private void OnToolButtonPresed()
        {
            _menuParentsManager.Show(_toolButtons);
            _levelEditor.ChangeSelector(_brushSelector);
            _levelEditor.ChangeTool(_drawTool);
        }
        private void OnDeleteButtonPressed()
        {
            _settingsMenu.RestoreDefaultValues();
            _levelEditor.CleaerCommandsBuffer();
            _level.Grid.Clear();
            _waveEditor.DeleteCurrentData();
        }

        private void OnOkSaveButtonClickedWhileOverriding()
        {
            _levelSavingUI.OkButtonPressed.RemoveListener(OnOkSaveButtonClickedWhileOverriding);
            
            _levelEditor.SaveLevel(_waveEditor.WaveDatas.ToArray(),true);
        }

        private void OnTogglePlacingRoadPressed() => _levelEditor.TogglePlacingRoad();
        private void OnNewButtonPressed()
        {
            _settingsMenu.RestoreDefaultValues();
            _levelEditor.CleaerCommandsBuffer();
            _level.Grid.Clear();
            _waveEditor.DeleteCurrentData();
        }
        private void OnSpawnerButtonPressed()
        {
            _levelEditor.ChangeTool(_spawnerPlacer);
            _levelEditor.ChangeSelector(_spawnerPlacementSelector);
            _menuParentsManager.HideAll();
        }

        private void OnSpawnerAddedAt(Vector2Int position)
        {
            _spawnersView.CreateViewAt(position);
        }

        private void OnSpawnerRemovedAt(Vector2Int position)
        {
            _spawnersView.RemoveViewAt(position);
        }
    }
}
