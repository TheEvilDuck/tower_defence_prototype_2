using Common.States;
using GamePlay.UI;

namespace GamePlay
{
    public abstract class GameOverState : State
    {
        protected readonly GameOverView _gameOverView;
        private readonly SceneLoader _sceneLoader;
        private readonly BuilderMediator _builderMediator;
        public GameOverState(StateMachine stateMachine, GameOverView gameOverView, SceneLoader sceneLoader, BuilderMediator builderMediator) : base(stateMachine)
        {
            _gameOverView = gameOverView;
            _sceneLoader = sceneLoader;
            _builderMediator = builderMediator;
        }

        public override void Enter()
        {
            _builderMediator.Dispose();
            ShowGameOverView();

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