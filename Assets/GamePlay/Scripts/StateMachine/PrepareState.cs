using Builder;
using Common.States;
using GamePlay.EnemiesSpawning;
using UnityEngine;

namespace GamePlay.States
{
    public class PrepareState : State
    {
        private readonly PlacableBuilder _placableBuilder;
        private readonly Spawners _spawners;
        private readonly int _gridSize;
        private readonly bool _useDiagonal;
        public PrepareState(StateMachine stateMachine, PlacableBuilder placableBuilder, Spawners spawners, bool useDiagonal, int gridSize) : base(stateMachine)
        {
            _placableBuilder = placableBuilder;
            _spawners = spawners;
            _useDiagonal = useDiagonal;
            _gridSize = gridSize;
        }

        public override void Enter()
        {
            _placableBuilder.mainBuildingBuilt += OnMainBuildingBuilt;
            _placableBuilder.checkCanBuildMainBuilding += CanBuildMainBuilding;
        }

        public override void Exit()
        {
            _placableBuilder.mainBuildingBuilt -= OnMainBuildingBuilt;
            _placableBuilder.checkCanBuildMainBuilding -= CanBuildMainBuilding;
        }

        private void OnMainBuildingBuilt(Vector2Int cellPosition)
        {
           _stateMachine.ChangeState<EnemySpawnState>();
        }

        private bool CanBuildMainBuilding(Vector2Int cellPosition)
        {
            _spawners.CalculateAvailablePositions(cellPosition, _useDiagonal, _gridSize);
            return _spawners.IsAnyPathToMainBuildingAvailable();
        }
    }

}