using Common;
using Common.States;
using GamePlay.UI;

namespace GamePlay.States
{
    public class LoseState : GameOverState
    {
        public LoseState(StateMachine stateMachine, GameOverView gameOverView, SceneLoader sceneLoader, MenuParentsManager menuParentsManager) : base(stateMachine, gameOverView, sceneLoader,menuParentsManager)
        {
        }

        protected override void ShowGameOverView()
        {
            _gameOverView.ShowLoseText();
        }
    }
}