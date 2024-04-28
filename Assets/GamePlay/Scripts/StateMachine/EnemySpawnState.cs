using System.Linq;
using Builder;
using Common.States;
using GamePlay.EnemiesSpawning;
using GamePlay.UI;
using Towers;

namespace GamePlay.States
{
    public class EnemySpawnState : State
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly PlacablesContainer _placablesContainer;
        private readonly IMainBuilderProvider _mainBuilderProvider;
        private readonly TowersPanel _towersPanel;
        public EnemySpawnState(
            StateMachine stateMachine, 
            EnemySpawner enemySpawner, 
            IMainBuilderProvider mainBuilderProvider, 
            PlacablesContainer placablesContainer,
            TowersPanel towersPanel) : base(stateMachine)
        {
            _enemySpawner = enemySpawner;
            _mainBuilderProvider = mainBuilderProvider;
            _placablesContainer = placablesContainer;
            _towersPanel = towersPanel;
        }

        public override void Enter()
        {
            if (_mainBuilderProvider.MainBuilding == null)
                throw new System.Exception("Somehow there is not main building at the start of enemy spawn state!");

            _mainBuilderProvider.MainBuilding.destroyed += OnMainBuildingDestroyed;

            _towersPanel.ShowAllExceptMainBuilding();

            foreach (var placable in _placablesContainer.Placables)
                placable?.OnBuild();

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