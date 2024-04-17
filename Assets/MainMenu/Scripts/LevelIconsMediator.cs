using System;
using Common;
using Common.UI;

namespace MainMenu
{
    public class LevelIconsMediator : IDisposable
    {
        private LevelIconsLoader _levelIconsLoader;
        private SceneLoader _sceneLoader;

        public LevelIconsMediator(LevelIconsLoader levelIconsLoader, SceneLoader sceneLoader)
        {
            _levelIconsLoader = levelIconsLoader;
            _sceneLoader = sceneLoader;

            _levelIconsLoader.mapIconPressed += OnLevelIconClicked;
        }

        public void Dispose()
        {
            _levelIconsLoader.mapIconPressed -= OnLevelIconClicked;
        }

        private void OnLevelIconClicked(string levelName)
        {
            PlayerSettingsData.SaveChosenMapName(levelName);
            _sceneLoader.LoadGameplay();
        }
    }
}
