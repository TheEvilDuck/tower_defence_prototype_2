using Common;
using Common.States;
using GamePlay.UI;

namespace GamePlay.States
{
    public class WinState : GameOverState
    {
        public WinState(StateMachine stateMachine, GameOverView gameOverView, SceneLoader sceneLoader, MenuParentsManager menuParentsManager) : base(stateMachine, gameOverView, sceneLoader,menuParentsManager)
        {
        }

        protected override void ShowGameOverView()
        {
            _gameOverView.ShowWinText();
        }
    }
}