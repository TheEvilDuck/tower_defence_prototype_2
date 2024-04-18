using System;
using Common;
using GamePlay.UI;

namespace GamePlay
{
    public class PauseMediator: IDisposable
    {
        private readonly PausableManager _pausableManager;
        private readonly PauseView _pauseView;
        private readonly PauseButton _pauseButton;
        private readonly MenuParentsManager _menuParentsManager;
        private readonly SceneLoader _sceneLoader;
        
        public PauseMediator(PausableManager pausableManager, PauseView pauseView, PauseButton pauseButton, MenuParentsManager menuParentsManager, SceneLoader sceneLoader)
        {
            _pausableManager = pausableManager;
            _pauseView = pauseView;
            _pauseButton = pauseButton;
            _sceneLoader = sceneLoader;

            _pauseView.resumeButtonPressed += OnResumeButtonPressed;
            _pauseButton.pauseButtonClicked += OnPauseButtonClicked;
            _pauseView.exitButtonPressed += OnExitButtonPressed;
            _pauseView.restartButtonPressed += OnRestartButtonPressed;

            _menuParentsManager = menuParentsManager;
        }

        public void Dispose()
        {
            _pauseView.resumeButtonPressed -= OnResumeButtonPressed;
            _pauseButton.pauseButtonClicked -= OnPauseButtonClicked;
            _pauseView.exitButtonPressed -= OnExitButtonPressed;
            _pauseView.restartButtonPressed -= OnRestartButtonPressed;
        }

        private void OnResumeButtonPressed()
        {
            _pausableManager.UnPause();
            _menuParentsManager.Show(_pauseButton);
        }

        private void OnPauseButtonClicked()
        {
            _pausableManager.Pause();
            _menuParentsManager.Show(_pauseView);
        }

        private void OnRestartButtonPressed() => _sceneLoader.LoadGameplay();
        private void OnExitButtonPressed() => _sceneLoader.LoadMainMenu();
    }
}