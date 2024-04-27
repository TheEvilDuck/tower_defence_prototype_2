using System;
using Builder;
using Common.UI;
using LevelEditor.UI;
using LevelEditor.UI.WavesEditing;
using Levels.Logic;

namespace LevelEditor.Mediators
{
    public class LevelIconsAndLevelLoaderMediator: IDisposable
    {
        private LevelLoader _levelLoader;
        private LevelIconsLoader _levelIconsLoader;
        private LevelEditor _levelEditor;
        private Level _level;
        private WavesEditor _wavesEditor;
        private SettingsMenu _settingsMenu;
        private SpawnerPositions _spawnerPositions;
        private TowersSettingsMenu _towersSettingsMenu;
        private PlacableBuilder _placableBuilder;

        public LevelIconsAndLevelLoaderMediator(
            LevelLoader levelLoader, 
            LevelIconsLoader levelIconsLoader, 
            LevelEditor levelEditor, 
            Level level, 
            WavesEditor wavesEditor, 
            SettingsMenu settingsMenu,
            SpawnerPositions spawnerPositions,
            TowersSettingsMenu towersSettingsMenu,
            PlacableBuilder placableBuilder)
        {
            _levelLoader = levelLoader;
            _levelIconsLoader = levelIconsLoader;
            _levelEditor = levelEditor;
            _level = level;
            _wavesEditor = wavesEditor;
            _settingsMenu = settingsMenu;
            _spawnerPositions = spawnerPositions;
            _towersSettingsMenu = towersSettingsMenu;
            _placableBuilder = placableBuilder;

            _levelIconsLoader.mapIconPressed+=OnIconPressed;
        }

        public void Dispose()
        {
            _levelIconsLoader.mapIconPressed-=OnIconPressed;
        }

        private void OnIconPressed(string mapName)
        {
            if (_levelLoader.TryLoadLevel(mapName, out LevelData levelData))
            {
                _levelEditor.CleaerCommandsBuffer();
                _level.UpdateGridData(levelData.gridData);
                _wavesEditor.LoadFromLevelData(levelData);
                _settingsMenu.LoadLevelName(mapName);
                _settingsMenu.LoadFromLevelData(levelData);
                _spawnerPositions.LoadFromLevelData(levelData.spawnerPlaces, _level.Grid);
                _towersSettingsMenu.PreloadWith(levelData.allowedPlacables);
                _placableBuilder.BuildFromPlacableDatas(levelData.placables, _level.Grid);
            }
        }
    }
}
