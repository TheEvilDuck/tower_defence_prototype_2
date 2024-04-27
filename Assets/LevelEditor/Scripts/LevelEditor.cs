using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using LevelEditor.Commands;
using LevelEditor.LevelSaving;
using LevelEditor.Selectors;
using LevelEditor.Tools;
using LevelEditor.UI;
using Levels.Logic;
using Towers;
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
        private Stack<ICommand>_canceledCommands;
        private LevelLoader _levelLoader;
        private IconsMaker _levelIconMaker;
        private ISelector _currentSelector;
        private Tool _currentTool;
        private LevelData _currentLevelData;
        private string _currentLevelName;
        private readonly LevelSavingResultFactory _levelSavingResultFactory;
        private bool _placeRoad = false;
        private SpawnerPositions _spawnerPositions;
        private TowersPlaceMenu _towersPlaceMenu;

        public LevelEditor(Level level, IconsMaker levelIconMaker, LevelLoader levelLoader, LevelSavingResultFactory levelSavingResultFactory, SpawnerPositions spawnerPositions, TowersPlaceMenu towersPlaceMenu)
        {
            _level = level;
            _levelLoader = levelLoader;
            _levelIconMaker = levelIconMaker;
            _currentLevelData = new LevelData();
            _levelSavingResultFactory = levelSavingResultFactory;
            _spawnerPositions = spawnerPositions;
            _towersPlaceMenu = towersPlaceMenu;

            _performedCommands = new Stack<ICommand>(MAX_COMMANDS_BUFFER);
            _canceledCommands = new Stack<ICommand>(MAX_COMMANDS_BUFFER);
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

        public void CleaerCommandsBuffer()
        {
            _performedCommands.Clear();
            _canceledCommands.Clear();
        }

        public void UndoLastCommand()
        {
            if (_performedCommands.Count==0)
                return;
            
            ICommand command = _performedCommands.Pop();

            if (command!=null)
            {   
                command.Undo();
                _canceledCommands.Push(command);
            }
        }

        public void RedoCommand()
        {
            if (_canceledCommands.Count==0)
                return;
            
            ICommand command = _canceledCommands.Pop();

            if (command!=null)
            {   
                command.Execute();
                _performedCommands.Push(command);
            }
        }

        public void UpdateLevelName(string newName) => _currentLevelName = newName;
        public void UpdateLevelStartMoney(int startMoney) => _currentLevelData.startMoney = startMoney;
        public void UpdateLevelFirstWaveDelay(float delay) => _currentLevelData.firstWaveDelay = delay;

        private async Task SaveLevel(WaveData[] waveDatas, PlacableEnum[] availableTowers)
        {
            _currentLevelData.waves = waveDatas;

            if (string.IsNullOrWhiteSpace(_currentLevelName))
            {
                levelSaveTried.Invoke(_levelSavingResultFactory.Get(ResultType.EmptyName));
                return;
            }

            if (_currentLevelName.Length < MIN_LEVEL_NAME_LENGTH)
            {
                levelSaveTried.Invoke(_levelSavingResultFactory.Get(ResultType.ShortName));
                return;
            }

            LevelData savingLevelData = _level.ConvertToLevelData(_currentLevelData.startMoney, _currentLevelData.firstWaveDelay, _currentLevelData.waves);
            savingLevelData.allowedPlacables = availableTowers;
            List<int> spawnerIndexes = new List<int>();

            foreach (Vector2Int cellPosition in _spawnerPositions.Spawners)
            {
                int index = _level.Grid.ConvertVector2IntToIndex(cellPosition);
                Debug.Log($"Trying to save spawner position at {cellPosition}, converted to index: {index}");
                spawnerIndexes.Add(index);
            }
            
            savingLevelData.spawnerPlaces = spawnerIndexes.ToArray();
            savingLevelData.placables = _towersPlaceMenu.ConvertToPlacableDatas(_level.Grid);

            await _levelLoader.SaveLevel(
                _currentLevelName,
                savingLevelData,
                OnLevelSaved,
                OnLevelSaveFailed);
        }

        public async void SaveLevel(WaveData[] waveDatas, PlacableEnum[] availableTowers, bool mapOverride)
        {
            if (_levelLoader.LevelExists(_currentLevelName))
            {
                if (!mapOverride)
                {
                    levelSaveTried.Invoke(_levelSavingResultFactory.Get(ResultType.MapOverride));
                    return;
                }
            }

            await SaveLevel(waveDatas, availableTowers);
        }

        private async void OnLevelSaved()
        {
            await _levelLoader.CreateLevelIcon(_currentLevelName,_levelIconMaker.Get(),OnLevelIconCreated);
        }

        private void OnLevelSaveFailed()
        {
            levelSaveTried.Invoke(_levelSavingResultFactory.Get(ResultType.Error));
        }

        private void OnLevelIconCreated()
        {
            levelSaveTried.Invoke(_levelSavingResultFactory.Get(ResultType.Success));
        }

        private void OnToolUsingCompleted(ICommand resultCommand)
        {
            _canceledCommands.Clear();
            _performedCommands.Push(resultCommand);

            Debug.Log("Command added to performed");
        }

        public void Dispose()
        {
            if (_currentTool!=null)
                _currentTool.usingCompleted-=OnToolUsingCompleted;
        }

        public void TogglePlacingRoad() => _placeRoad = !_placeRoad;
    }
}
