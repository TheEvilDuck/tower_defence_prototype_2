using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class CommandSequence : ICommand
    {
        List<ICommand> _commands;

        public int CommandsCounts => _commands.Count;

        public CommandSequence()
        {
            _commands = new List<ICommand>();
        }

        public bool Execute()
        {
            bool executedAtLeastOne = false;

            foreach(ICommand command in _commands)
                if (command.Execute())
                    executedAtLeastOne = true;

            return executedAtLeastOne;
                
        }

        public void Undo()
        {
            
            foreach(ICommand command in _commands)
                command.Undo();
        }

        public void AddCommand(ICommand command)
        {
            if (!_commands.Contains(command))
                _commands.Add(command);
        }

        public void RemoveCommand(ICommand command)
        {
            _commands.Remove(command);
        }
    }
}
