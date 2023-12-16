using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay;
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
        private KeyCombinationHandler _undoKeyCombination;
        private KeyHandler _fillKey;
        private KeyHandler _drawKey;
        private CommandFactory _currentCommandFactory;

        public LevelEditorMediator
        (
            LevelEditor levelEditor, 
            PlayerInput playerInput, 
            Level level, 
            TileController tileController,
            KeyCombinationHandler undoKeyCombination, 
            KeyHandler fillKey,
            KeyHandler drawKey
        )
        {
            _levelEditor = levelEditor;
            _playerInput = playerInput;
            _level = level;
            _tileController = tileController;
            _undoKeyCombination = undoKeyCombination;
            _fillKey = fillKey;
            _drawKey = drawKey;

            _currentCommandFactory = new DrawCommandsFactory(_level.Grid);
        
            _level.Grid.cellChanged+=OnCellChanged;
            _level.Grid.cellRemoved+=OnCellRemoved;
            _playerInput.mouseLeftHold+=OnPlayerLeftMouseHoldAt;
            _playerInput.mouseRightHold+=OnPlayerRightMouseHoldAt;
            _undoKeyCombination.Down+=OnUndoKeyCombinationDown;
            _fillKey.Down+=OnFillKeyDown;
            _drawKey.Down+=OnDrawKeyDown;
        }

        public void Dispose()
        {
            _playerInput.mouseLeftHold-=OnPlayerLeftMouseHoldAt;
            _playerInput.mouseRightHold-=OnPlayerRightMouseHoldAt;
            _level.Grid.cellChanged-=OnCellChanged;
            _undoKeyCombination.Down-=OnUndoKeyCombinationDown;
            _level.Grid.cellRemoved-=OnCellRemoved;
            _fillKey.Down-=OnFillKeyDown;
            _drawKey.Down-=OnDrawKeyDown;
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

        private void OnCellChanged(Vector2Int cellId) => _tileController.DrawAt(cellId);

        private void OnCellRemoved(Vector2Int cellId) => _tileController.RempoveAt(cellId);

        private void OnUndoKeyCombinationDown() => _levelEditor.UndoLastCommand();
    }
}
