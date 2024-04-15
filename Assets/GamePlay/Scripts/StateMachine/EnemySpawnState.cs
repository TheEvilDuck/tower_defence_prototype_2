using System.Linq;
using Common.States;
using Towers;

namespace GamePlay
{
    public class EnemySpawnState : State
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly IMainBuilderProvider _mainBuilderProvider;
        public EnemySpawnState(StateMachine stateMachine, EnemySpawner enemySpawner, IMainBuilderProvider mainBuilderProvider) : base(stateMachine)
        {
            _enemySpawner = enemySpawner;
            _mainBuilderProvider = mainBuilderProvider;
        }

        public override void Enter()
        {
            if (_mainBuilderProvider.MainBuilding == null)
                throw new System.Exception("Somehow there is not main building at the start of enemy spawn state!");

            _mainBuilderProvider.MainBuilding.destroyed += OnMainBuildingDestroyed;

            _enemySpawner.Start();
        }

        public override void Update()
        {
            _enemySpawner.Update();

            if (_enemySpawner.IsLastWave)
            {
                //Logic for LAST WAVE popup

                if (_enemySpawner.Enemies.Count() == 0)
                {
                    _stateMachine.ChangeState<WinState>();
                }
            }
        }

        public override void Exit()
        {
            if (_mainBuilderProvider.MainBuilding != null)
            {
                _mainBuilderProvider.MainBuilding.destroyed -= OnMainBuildingDestroyed;
            }
        }

        private void OnMainBuildingDestroyed(Placable mainBuilding)
        {
            mainBuilding.destroyed -= OnMainBuildingDestroyed;
            _stateMachine.ChangeState<LoseState>();
        }
    }
}