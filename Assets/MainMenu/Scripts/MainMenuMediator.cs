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
        }

        public void Dispose()
        {
            _mainMenuView.exitButtonPressed-=OnExitButtonPressed;
            _mainMenuView.levelEditorButtonPressed-=OnLevelEditorButtonPressed;
        }

        private void OnExitButtonPressed() => Application.Quit();
        private void OnLevelEditorButtonPressed() => _sceneLoader.LoadLevelEditor();
    }
}
