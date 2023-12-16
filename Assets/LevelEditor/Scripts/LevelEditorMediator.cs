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
        private LevelEditorConfig _levelEditorConfig;

        public LevelEditorMediator(LevelEditor levelEditor, PlayerInput playerInput, Level level, TileController tileController, LevelEditorConfig levelEditorConfig)
        {
            _levelEditor = levelEditor;
            _playerInput = playerInput;
            _level = level;
            _tileController = tileController;
            _levelEditorConfig = levelEditorConfig;
        
            _level.Grid.cellChanged+=OnCellChanged;
            _level.Grid.cellRemoved+=OnCellRemoved;
            _playerInput.mouseLeftClicked+=OnPlayerClickedLeftAt;
            _playerInput.keysCombinationHold+=OnPlayerInputKeysCombination;
        }

        private void OnPlayerClickedLeftAt(Vector2 position)
        {
            Vector2Int cellId = _level.Grid.WorldPositionToGridPosition(position);
            ICommand groundCommand = new AddGroundAtCommand(_level.Grid,cellId);
            _levelEditor.ChangeCurrentCommand(groundCommand);
            _levelEditor.ExecuteCurrentCommand();
        }

        private void OnCellChanged(Vector2Int cellId)
        {
           _tileController.DrawAt(cellId);
        }

        private void OnCellRemoved(Vector2Int cellId)
        {
            _tileController.RempoveAt(cellId);
        }

        private void OnPlayerInputKeysCombination(KeyCode[] keys)
        {


            foreach (KeyCode key in _levelEditorConfig.UndoKeyCodes)
            {
                if (!keys.Contains(key))
                    return;
            }

            

            _levelEditor.UndoLastCommand();
        }

        public void Dispose()
        {
            _playerInput.mouseLeftClicked-=OnPlayerClickedLeftAt;
            _level.Grid.cellChanged-=OnCellChanged;
            _playerInput.keysCombinationHold-=OnPlayerInputKeysCombination;
            _level.Grid.cellRemoved-=OnCellRemoved;
        }
    }
}
