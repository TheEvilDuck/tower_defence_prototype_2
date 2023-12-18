using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay;
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
        private CommandFactory _currentCommandFactory;
        private LevelSavingUI _levelSavingUI;

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
            LevelSavingUI levelSavingUI
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
            _levelSavingUI = levelSavingUI;

            _currentCommandFactory = new DrawCommandsFactory(_level.Grid);
        
            _level.Grid.cellChanged+=OnCellChanged;
            _level.Grid.cellRemoved+=OnCellRemoved;
            _level.Grid.cellAdded+=OnCellAdded;
            _levelEditor.LevelSaved+=OnLevelSaved;
            _playerInput.mouseLeftHold+=OnPlayerLeftMouseHoldAt;
            _playerInput.mouseRightHold+=OnPlayerRightMouseHoldAt;
            _undoKeyCombination.Down+=OnUndoKeyCombinationDown;
            _saveKeyCombination.Down+=OnSaveKeyCombinationDown;
            _fillKey.Down+=OnFillKeyDown;
            _drawKey.Down+=OnDrawKeyDown;
        }

        public void Dispose()
        {
            _playerInput.mouseLeftHold-=OnPlayerLeftMouseHoldAt;
            _playerInput.mouseRightHold-=OnPlayerRightMouseHoldAt;
            _level.Grid.cellChanged-=OnCellChanged;
            _undoKeyCombination.Down-=OnUndoKeyCombinationDown;
            _saveKeyCombination.Down-=OnSaveKeyCombinationDown;
            _level.Grid.cellRemoved-=OnCellRemoved;
            _fillKey.Down-=OnFillKeyDown;
            _drawKey.Down-=OnDrawKeyDown;
            _level.Grid.cellAdded-=OnCellAdded;
            _levelEditor.LevelSaved-=OnLevelSaved;
        }

        private void OnPlayerLeftMouseHoldAt(Vector2 position)
        {
            Vector2Int cellId = _level.Grid.WorldPositionToGridPosition(position);
            ICommand groundCommand = _currentCommandFactory.CreateCommandAtCellId(cellId);
            _levelEditor.ChangeCurrentCommand(groundCommand);
            _levelEditor.ExecuteCurrentCommand();
        }

        private void OnPlayerRightMouseHoldAt(Vector2 position)
        {
            Vector2Int cellId = _level.Grid.WorldPositionToGridPosition(position);
            ICommand groundCommand = new DeleteGroundCommand(_level.Grid,cellId);
            _levelEditor.ChangeCurrentCommand(groundCommand);
            _levelEditor.ExecuteCurrentCommand();
        }

        private void OnFillKeyDown() => _currentCommandFactory = new FillCommandsFactory(_level.Grid);

        private void OnDrawKeyDown() =>  _currentCommandFactory = new DrawCommandsFactory(_level.Grid);

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
