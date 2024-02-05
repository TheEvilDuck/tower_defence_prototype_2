using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LevelEditor.Selectors;
using Levels.Logic;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditor: IDisposable
    {
        public event Action LevelSaved;

        private const int MAX_COMMANDS_BUFFER = 50;
        private Level _level;
        private Stack<ICommand>_performedCommands;
        private LevelLoader _levelLoader;
        private LevelIconMaker _levelIconMaker;
        private ISelector _currentSelector;
        private Tool _currentTool;

        public LevelEditor(Level level, LevelIconMaker levelIconMaker, LevelLoader levelLoader)
        {
            _level = level;
            _levelLoader = levelLoader;
            _levelIconMaker = levelIconMaker;

            _performedCommands = new Stack<ICommand>(MAX_COMMANDS_BUFFER);
        }

        public void ChangeSelector(ISelector newSelector)
        {
            _currentSelector?.Disable();
            _currentSelector = newSelector;
            _currentSelector?.Enable();
            _currentTool?.ChangeSelector(_currentSelector);
        }

        public void ChangeTool(Tool newTool)
        {
            if (_currentTool!=null)
                _currentTool.usingCompleted-=OnToolUsingCompleted;
            
            _currentTool?.ChangeSelector(null);
            _currentTool = newTool;
           
           if (_currentTool!=null)
           {
                _currentTool.usingCompleted+=OnToolUsingCompleted;
                _currentTool.ChangeSelector(_currentSelector);
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

        private void OnToolUsingCompleted(ICommand resultCommand)
        {
            _performedCommands.Push(resultCommand);
        }

        public void Dispose()
        {
            if (_currentTool!=null)
                _currentTool.usingCompleted-=OnToolUsingCompleted;
        }
    }
}
