using Builder;
using Common.States;
using UnityEngine;

namespace GamePlay
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
        }

        public override void Exit()
        {
            _placableBuilder.mainBuildingBuilt -= OnMainBuildingBuilt;
        }

        private void OnMainBuildingBuilt(Vector2Int cellPosition)
        {
            _spawners.CalculateAvailablePositions(cellPosition, _useDiagonal, _gridSize);
            _stateMachine.ChangeState<EnemySpawnState>();
        }
    }

}