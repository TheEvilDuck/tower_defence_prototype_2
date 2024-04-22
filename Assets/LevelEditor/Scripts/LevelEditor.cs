using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LevelEditor.Selectors;
using Levels.Logic;
using Levels.Tiles;
using UnityEngine;
using Waves;

namespace LevelEditor
{
    public class LevelEditor: IDisposable
    {
        public event Action<LevelSavingResult> levelSaveTried;

        private const int MAX_COMMANDS_BUFFER = 50;
        private const int MIN_LEVEL_NAME_LENGTH = 3;
        private Level _level;
        private Stack<ICommand>_performedCommands;
        private LevelLoader _levelLoader;
        private LevelIconMaker _levelIconMaker;
        private ISelector _currentSelector;
        private Tool _currentTool;
        private LevelData _currentLevelData;
        private string _currentLevelName;
        private readonly LevelSavingResultFabric _levelSavingResultFabric;
        private bool _placeRoad = false;
        private SpawnerPositions _spawnerPositions;

        public LevelEditor(Level level, LevelIconMaker levelIconMaker, LevelLoader levelLoader, LevelSavingResultFabric levelSavingResultFabric, SpawnerPositions spawnerPositions)
        {
            _level = level;
            _levelLoader = levelLoader;
            _levelIconMaker = levelIconMaker;
            _currentLevelData = new LevelData();
            _levelSavingResultFabric = levelSavingResultFabric;
            _spawnerPositions = spawnerPositions;

            _performedCommands = new Stack<ICommand>(MAX_COMMANDS_BUFFER);
        }

        public void CreateNewMap(Level level)
        {
            _level = level;
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

        private async Task SaveLevel(WaveData[] waveDatas)
        {
            _currentLevelData.waves = waveDatas;

            if (string.IsNullOrWhiteSpace(_currentLevelName))
            {
                levelSaveTried.Invoke(_levelSavingResultFabric.Get(ResultType.EmptyName));
                return;
            }

            if (_currentLevelName.Length < MIN_LEVEL_NAME_LENGTH)
            {
                levelSaveTried.Invoke(_levelSavingResultFabric.Get(ResultType.ShortName));
                return;
            }

            LevelData savingLevelData = _level.ConvertToLevelData(_currentLevelData.startMoney, _currentLevelData.firstWaveDelay, _currentLevelData.waves);
            List<int> spawnerIndexes = new List<int>();

            foreach (Vector2Int cellPosition in _spawnerPositions.Spawners)
            {
                int index = _level.Grid.ConvertVector2IntToIndex(cellPosition);
                Debug.Log($"Trying to save spawner position at {cellPosition}, converted to index: {index}");
                spawnerIndexes.Add(index);
            }
            
            savingLevelData.spawnerPlaces = spawnerIndexes.ToArray();

            await _levelLoader.SaveLevel(
                _currentLevelName,
                savingLevelData,
                OnLevelSaved,
                OnLevelSaveFailed);
        }

        public async void SaveLevel(WaveData[] waveDatas, bool mapOverride)
        {
            if (_levelLoader.LevelExists(_currentLevelName))
            {
                if (!mapOverride)
                {
                    levelSaveTried.Invoke(_levelSavingResultFabric.Get(ResultType.MapOverride));
                    return;
                }
            }

            await SaveLevel(waveDatas);
        }

        private async void OnLevelSaved()
        {
            await _levelLoader.CreateLevelIcon(_currentLevelName,_levelIconMaker.MakeLevelIcon(),OnLevelIconCreated);
        }

        private void OnLevelSaveFailed()
        {
            levelSaveTried.Invoke(_levelSavingResultFabric.Get(ResultType.Error));
        }

        private void OnLevelIconCreated()
        {
            levelSaveTried.Invoke(_levelSavingResultFabric.Get(ResultType.Success));
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

        public void TogglePlacingRoad() => _placeRoad = !_placeRoad;
    }
}
