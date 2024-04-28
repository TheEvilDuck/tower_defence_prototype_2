using Builder;
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
        private readonly PlacableBuilder _builder;
        private readonly TowersPanel _towersPanel;
        public GameOverState(
            StateMachine stateMachine, 
            GameOverView gameOverView, 
            SceneLoader sceneLoader, 
            MenuParentsManager menuParentsManager,
            PlacableBuilder placableBuilder,
            TowersPanel towersPanel
            ) : base(stateMachine)
        {
            _gameOverView = gameOverView;
            _sceneLoader = sceneLoader;
            _pauseMenus = menuParentsManager;
            _builder = placableBuilder;
            _towersPanel = towersPanel;
        }

        public override void Enter()
        {
            ShowGameOverView();
            _pauseMenus.HideAll();
            _towersPanel.gameObject.SetActive(false);
            _builder.DisablePreview();

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