using Common.States;
using GamePlay.UI;

namespace GamePlay
{
    public class LoseState : GameOverState
    {
        public LoseState(StateMachine stateMachine, GameOverView gameOverView, SceneLoader sceneLoader, BuilderMediator builderMediator) : base(stateMachine, gameOverView, sceneLoader, builderMediator)
        {
        }

        protected override void ShowGameOverView()
        {
            _gameOverView.ShowLoseText();
        }
    }
}