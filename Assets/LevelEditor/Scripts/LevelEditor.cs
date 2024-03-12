using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LevelEditor.Selectors;
using Levels.Logic;
using UnityEngine;
using Waves;

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
        private LevelData _currentLevelData;
        private string _currentLevelName;

        public LevelEditor(Level level, LevelIconMaker levelIconMaker, LevelLoader levelLoader)
        {
            _level = level;
            _levelLoader = levelLoader;
            _levelIconMaker = levelIconMaker;
            _currentLevelData = new LevelData();

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

        public void CleaerCommandsBuffer() => _performedCommands.Clear();

        public void UndoLastCommand()
        {
            if (_performedCommands.Count==0)
                return;
            
            ICommand command = _performedCommands.Pop();

            if (command!=null)
                command.Undo();
        }

        public void UpdateLevelName(string newName) => _currentLevelName = newName;
        public void UpdateLevelStartMoney(int startMoney) => _currentLevelData.startMoney = startMoney;
        public void UpdateLevelFirstWaveDelay(float delay) => _currentLevelData.firstWaveDelay = delay;

        public async void SaveLevel(WaveData[] waveDatas)
        {
            _currentLevelData.waves = waveDatas;

            Debug.Log("Level is saving...");
            await _levelLoader.SaveLevel(
                _currentLevelName,
                _level.ConvertToLevelData(_currentLevelData.startMoney, _currentLevelData.firstWaveDelay, _currentLevelData.waves),
                OnLevelSaved,
                OnLevelSaveFailed);
        }

        private async void OnLevelSaved()
        {
            Debug.Log("Level saved");
            await _levelLoader.CreateLevelIcon(_currentLevelName,_levelIconMaker.MakeLevelIcon(),OnLevelIconCreated);
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
