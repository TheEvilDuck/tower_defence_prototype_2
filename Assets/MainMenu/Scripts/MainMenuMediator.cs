using System;
using Common;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuMediator: IDisposable
    {
        private MainMenuView _mainMenuView;
        private SceneLoader _sceneLoader;
        private MenuParentsManager _menuParentManager;
        private LevelSelectorView _levelSelectorView;
        public MainMenuMediator(MainMenuView mainMenuView, SceneLoader sceneLoader, MenuParentsManager menuParentsManager, LevelSelectorView levelSelectorView)
        {
            _mainMenuView = mainMenuView;
            _sceneLoader = sceneLoader;
            _menuParentManager = menuParentsManager;
            _levelSelectorView = levelSelectorView;

            _mainMenuView.exitButtonPressed+=OnExitButtonPressed;
            _mainMenuView.levelEditorButtonPressed+=OnLevelEditorButtonPressed;
            _mainMenuView.playButtonPressed+=OnPlayButtonPressed;
            _levelSelectorView.backButtonPressed += OnBackInLevelSelectorPressed;
        }

        public void Dispose()
        {
            _mainMenuView.exitButtonPressed-=OnExitButtonPressed;
            _mainMenuView.levelEditorButtonPressed-=OnLevelEditorButtonPressed;
            _mainMenuView.playButtonPressed-=OnPlayButtonPressed;
            _levelSelectorView.backButtonPressed -= OnBackInLevelSelectorPressed;
        }

        private void OnExitButtonPressed() => Application.Quit();
        private void OnLevelEditorButtonPressed() => _sceneLoader.LoadLevelEditor();
        private void OnPlayButtonPressed()
        {
            _menuParentManager.Show(_levelSelectorView);
        }

        private void OnBackInLevelSelectorPressed()
        {
            _menuParentManager.Show(_mainMenuView);
        }
    }
}
