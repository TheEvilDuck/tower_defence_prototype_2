using System;
using LevelEditor.Selectors;
using LevelEditor.UI;
using Levels.Logic;
using Levels.TileControl;
using Services.PlayerInput;
using UnityEngine;

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
        private KeyHandler _drawToolKey;
        private KeyHandler _eraseToolKey;
        private LevelSavingUI _levelSavingUI;
        private Tool _drawTool;
        private Tool _eraseTool;
        private Tool _currentTool;
        private FillSelector _fillSelector;
        private BrushSelector _brushSelector;
        private ISelector _currentSelector;

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
            KeyHandler drawToolKey,
            KeyHandler eraseToolKey,
            LevelSavingUI levelSavingUI,
            Tool drawTool,
            Tool eraseTool,
            FillSelector fillSelector,
            BrushSelector brushSelector
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
            _drawToolKey = drawToolKey;
            _eraseToolKey = eraseToolKey;
            _levelSavingUI = levelSavingUI;
            _drawTool = drawTool;
            _eraseTool = eraseTool;
            _fillSelector = fillSelector;
            _brushSelector = brushSelector;

            _currentTool = _drawTool;
            _currentTool.usingCompleted+=OnToolUsingCompleted;
            _currentSelector = _brushSelector;
            _currentSelector.Enable();
            _currentTool.ChangeSelector(_currentSelector);
        
            _level.Grid.cellChanged+=OnCellChanged;
            _level.Grid.cellRemoved+=OnCellRemoved;
            _level.Grid.cellAdded+=OnCellAdded;
            _levelEditor.LevelSaved+=OnLevelSaved;
            _undoKeyCombination.Down+=OnUndoKeyCombinationDown;
            _saveKeyCombination.Down+=OnSaveKeyCombinationDown;
            _drawKey.Down+=OnDrawKeyDown;
            _fillKey.Down+=OnFillKeyDown;
            _drawToolKey.Down+=OnDrawToolKeyDown;
            _eraseToolKey.Down+=OnEraseToolKeyDown;
        }

        public void Dispose()
        {
            _level.Grid.cellChanged-=OnCellChanged;
            _undoKeyCombination.Down-=OnUndoKeyCombinationDown;
            _saveKeyCombination.Down-=OnSaveKeyCombinationDown;
            _level.Grid.cellRemoved-=OnCellRemoved;
            _level.Grid.cellAdded-=OnCellAdded;
            _levelEditor.LevelSaved-=OnLevelSaved;
            _drawKey.Down-=OnDrawKeyDown;
            _fillKey.Down-=OnFillKeyDown;
            _drawToolKey.Down-=OnDrawToolKeyDown;
            _eraseToolKey.Down-=OnEraseToolKeyDown;
            _currentTool.usingCompleted-=OnToolUsingCompleted;
        }

        private void OnFillKeyDown()
        {
            _currentSelector.Disable();
            _currentSelector = _fillSelector;
            _currentSelector.Enable();
            _currentTool.ChangeSelector(_currentSelector);
        }

        private void OnDrawKeyDown()
        {
            _currentSelector.Disable();
            _currentSelector = _brushSelector;
            _currentSelector.Enable();
            _currentTool.ChangeSelector(_currentSelector);
        } 

        private void OnDrawToolKeyDown()
        {
            _currentTool.usingCompleted-=OnToolUsingCompleted;
            _currentTool.ChangeSelector(null);
            _currentTool = _drawTool;
            _currentTool.usingCompleted+=OnToolUsingCompleted;
            _currentTool.ChangeSelector(_currentSelector);
        }

        private void OnEraseToolKeyDown()
        {
            _currentTool.usingCompleted-=OnToolUsingCompleted;
            _currentTool.ChangeSelector(null);
            _currentTool = _eraseTool;
            _currentTool.usingCompleted+=OnToolUsingCompleted;
            _currentTool.ChangeSelector(_currentSelector);
        }

        private void OnToolUsingCompleted(ICommand resultCommand)
        {
            _levelEditor.AddExecutedCommand(resultCommand);
        }

        private void OnCellChanged(Vector2Int cellId, Cell cell)
        {
            if (cell.HasRoad)
                _tileController.DrawRoadAt(cellId);
            else
                _tileController.RemoveRoadAt(cellId);
        }

        private void OnCellAdded(Vector2Int cellId)
        {
            _tileController.DrawAt(cellId);
        }

        private void OnCellRemoved(Vector2Int cellId)
        {
            _tileController.RempoveAt(cellId);
            _tileController.RemoveRoadAt(cellId);
        }

        private void OnLevelSaved()
        {
            _levelSavingUI.OnSaveComplete();
        }

        private void OnUndoKeyCombinationDown() => _levelEditor.UndoLastCommand();
        private void OnSaveKeyCombinationDown()
        {
            _levelEditor.SaveLevel();
            _levelSavingUI.Show();
        }
    }
}
