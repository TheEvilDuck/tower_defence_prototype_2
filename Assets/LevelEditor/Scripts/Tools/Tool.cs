using System;
using System.Collections;
using System.Collections.Generic;
using LevelEditor.Selectors;
using Services.PlayerInput;
using UnityEngine;

namespace LevelEditor
{
    public class Tool: IDisposable
    {
        public event Action<ICommand>usingCompleted;

        private ISelector _currentSelector;
        private CommandSequence _currentCommandSequence;
        private CommandFactory _commandFactory;

        public Tool(CommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public void ChangeSelector(ISelector newSelector)
        {
            if (_currentSelector!=null)
            {
                _currentSelector.cellsSelected-=OnSelectionDone;
                _currentSelector.selectedCellsChanged-=OnSelectionChanged;
                _currentSelector.selectionStarted-=OnSelectionStartedAt;
            }

            _currentSelector = newSelector;

            if (newSelector==null)
                return;

            _currentSelector.cellsSelected+=OnSelectionDone;
            _currentSelector.selectedCellsChanged+=OnSelectionChanged;
            _currentSelector.selectionStarted+=OnSelectionStartedAt;
        }

        private void OnSelectionStartedAt(Vector2Int cellPosition)
        {
            _currentCommandSequence = new CommandSequence();
            CreateCommandAndExecute(cellPosition);
        }

        private void OnSelectionDone()
        {
            if (_currentCommandSequence==null)
                return;

            if (_currentCommandSequence.CommandsCounts==0)
                return;

            usingCompleted?.Invoke(_currentCommandSequence);
        }

        private void OnSelectionChanged(Vector2Int cellPosition)
        {
            CreateCommandAndExecute(cellPosition);
        }

        private void CreateCommandAndExecute(Vector2Int cellPosition)
        {
            ICommand command = _commandFactory.CreateCommandAtCell(cellPosition);
            
            if (command.Execute())
            {
                _currentCommandSequence.AddCommand(command);
            }
        }

        public void Dispose()
        {
            _currentSelector.cellsSelected-=OnSelectionDone;
            _currentSelector.selectedCellsChanged-=OnSelectionChanged;
            _currentSelector.selectionStarted-=OnSelectionStartedAt;
        }
    }
}
