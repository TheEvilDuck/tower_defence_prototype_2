using System;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuMediator: IDisposable
    {
        private MainMenuView _mainMenuView;
        private SceneLoader _sceneLoader;
        public MainMenuMediator(MainMenuView mainMenuView, SceneLoader sceneLoader)
        {
            _mainMenuView = mainMenuView;
            _sceneLoader = sceneLoader;

            _mainMenuView.exitButtonPressed+=OnExitButtonPressed;
            _mainMenuView.levelEditorButtonPressed+=OnLevelEditorButtonPressed;
            _mainMenuView.playButtonPressed+=OnPlayButtonPressed;
        }

        public void Dispose()
        {
            _mainMenuView.exitButtonPressed-=OnExitButtonPressed;
            _mainMenuView.levelEditorButtonPressed-=OnLevelEditorButtonPressed;
            _mainMenuView.playButtonPressed-=OnPlayButtonPressed;
        }

        private void OnExitButtonPressed() => Application.Quit();
        private void OnLevelEditorButtonPressed() => _sceneLoader.LoadLevelEditor();
        private void OnPlayButtonPressed() => _sceneLoader.LoadGameplay();
    }
}
