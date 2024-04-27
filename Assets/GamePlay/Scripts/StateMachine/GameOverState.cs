using Common;
using Common.States;
using GamePlay.UI;

namespace GamePlay.States
{
    public abstract class GameOverState : State
    {
        protected readonly GameOverView _gameOverView;
        private readonly SceneLoader _sceneLoader;
        private readonly MenuParentsManager _pauseMenus;
        public GameOverState(StateMachine stateMachine, GameOverView gameOverView, SceneLoader sceneLoader, MenuParentsManager menuParentsManager) : base(stateMachine)
        {
            _gameOverView = gameOverView;
            _sceneLoader = sceneLoader;
            _pauseMenus = menuParentsManager;
        }

        public override void Enter()
        {
            ShowGameOverView();
            _pauseMenus.HideAll();

            _gameOverView.restartButtonPressed += OnRestartButtonPressed;
            _gameOverView.exitButtonPressed += OnExitButtonPressed;
        }

        public override void Exit()
        {
            if (_gameOverView == null)
                return;

            _gameOverView.Hide();

            _gameOverView.restartButtonPressed -= OnRestartButtonPressed;
            _gameOverView.exitButtonPressed -= OnExitButtonPressed;
        }

        private void OnExitButtonPressed()
        {
            _sceneLoader.LoadMainMenu();
        }

        private void OnRestartButtonPressed()
        {
            _sceneLoader.LoadGameplay();
        }

        protected abstract void ShowGameOverView();
    }
}