using System;
using System.Collections;
using System.Collections.Generic;
using Common.UI;
using Levels.Logic;
using UnityEngine;

namespace LevelEditor
{
    public class LevelIconsAndLevelLoaderMediator: IDisposable
    {
        private LevelLoader _levelLoader;
        private LevelIconsLoader _levelIconsLoader;
        private LevelEditor _levelEditor;
        private Level _level;

        public LevelIconsAndLevelLoaderMediator(LevelLoader levelLoader, LevelIconsLoader levelIconsLoader, LevelEditor levelEditor, Level level)
        {
            _levelLoader = levelLoader;
            _levelIconsLoader = levelIconsLoader;
            _levelEditor = levelEditor;
            _level = level;

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

            }
        }
    }
}
