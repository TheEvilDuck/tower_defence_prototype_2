using Common;
using Common.States;
using GamePlay.UI;

namespace GamePlay
{
    public class WinState : GameOverState
    {
        public WinState(StateMachine stateMachine, GameOverView gameOverView, SceneLoader sceneLoader, BuilderMediator builderMediator, MenuParentsManager menuParentsManager) : base(stateMachine, gameOverView, sceneLoader,builderMediator,menuParentsManager)
        {
        }

        protected override void ShowGameOverView()
        {
            _gameOverView.ShowWinText();
        }
    }
}