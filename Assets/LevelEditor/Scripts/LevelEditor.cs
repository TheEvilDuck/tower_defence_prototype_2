using System.Collections;
using System.Collections.Generic;
using Levels.Logic;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditor
    {
        private const int MAX_COMMANDS_BUFFER = 50;
        private Level _level;
        private Stack<ICommand>_performedCommands;
        private ICommand _currentCommand;

        public LevelEditor(Level level)
        {
            _level = level;

            _performedCommands = new Stack<ICommand>(MAX_COMMANDS_BUFFER);
        }

        public void ExecuteCurrentCommand()
        {
            if (_currentCommand!=null)
            {
                if (_currentCommand.Execute())
                {
                    _performedCommands.Push(_currentCommand);
                }
            }
        }

        public void UndoLastCommand()
        {
            if (_performedCommands.Count==0)
                return;
            
            ICommand command = _performedCommands.Pop();

            if (command!=null)
                command.Undo();
        }

        public void ChangeCurrentCommand(ICommand newCommand)
        {
            _currentCommand = newCommand;
        }
    }
}
