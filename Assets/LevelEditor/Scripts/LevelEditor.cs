using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Levels.Logic;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditor
    {
        public event Action LevelSaved;

        private const int MAX_COMMANDS_BUFFER = 50;
        private Level _level;
        private Stack<ICommand>_performedCommands;
        private ICommand _currentCommand;
        private LevelLoader _levelLoader;
        private LevelIconMaker _levelIconMaker;

        public LevelEditor(Level level, LevelIconMaker levelIconMaker)
        {
            _level = level;
            _levelLoader = new LevelLoader();
            _levelIconMaker = levelIconMaker;

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

        public void AddExecutedCommand(ICommand command)
        {
            _performedCommands.Push(command);
        }

        public async void SaveLevel()
        {
            Debug.Log("Level is saving...");
            await _levelLoader.SaveLevel("test",_level.ConvertToLevelData(),OnLevelSaved,OnLevelSaveFailed);
        }

        private async void OnLevelSaved()
        {
            Debug.Log("Level saved");
            await _levelLoader.CreateLevelIcon("test",_levelIconMaker.MakeLevelIcon(),OnLevelIconCreated);
        }

        private void OnLevelSaveFailed()
        {
            Debug.Log("Level save failed");
        }

        private void OnLevelIconCreated()
        {
            Debug.Log("Icon created");
            LevelSaved?.Invoke();
        }
    }
}
