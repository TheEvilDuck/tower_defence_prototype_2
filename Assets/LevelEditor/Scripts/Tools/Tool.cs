using System;
using System.Collections.Generic;
using LevelEditor.Commands;
using LevelEditor.Commands.Factory;
using LevelEditor.Selectors;
using UnityEngine;

namespace LevelEditor.Tools
{
    public class Tool: IDisposable
    {
        public event Action<ICommand>usingCompleted;

        private ISelector _currentSelector;
        private CommandSequence _currentCommandSequence;
        private CommandFactory _commandFactory;
        private Dictionary<Vector2Int,ICommand> _affectedCells;

        public Tool(CommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
            _affectedCells = new Dictionary<Vector2Int, ICommand>();
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
            _affectedCells.Clear();
        }

        private void OnSelectionStartedAt(Vector2Int cellPosition)
        {
            _affectedCells.Clear();
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

        private void OnSelectionChanged(Vector2Int cellPosition, bool isAdded)
        {
            if (isAdded)
                CreateCommandAndExecute(cellPosition);
            else
                UndoCommandAt(cellPosition);
        }

        private void CreateCommandAndExecute(Vector2Int cellPosition)
        {
            ICommand command = _commandFactory.CreateCommandAtCell(cellPosition);
            
            if (command.Execute())
            {
                Debug.Log("Command executed");

                _currentCommandSequence.AddCommand(command);
                _affectedCells.TryAdd(cellPosition,command);
            }
        }

        private void UndoCommandAt(Vector2Int cellPosition)
        {
            if (_affectedCells.TryGetValue(cellPosition, out ICommand command))
            {
                _currentCommandSequence.RemoveCommand(command);
                command.Undo();
                _affectedCells.Remove(cellPosition);
            }
        }

        public void Dispose()
        {
            if (_currentSelector==null)
                return;

            _currentSelector.cellsSelected-=OnSelectionDone;
            _currentSelector.selectedCellsChanged-=OnSelectionChanged;
            _currentSelector.selectionStarted-=OnSelectionStartedAt;
        }
    }
}
