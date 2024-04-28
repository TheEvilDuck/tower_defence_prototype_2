using UnityEngine;

namespace LevelEditor.Commands.Factory
{
    public class RemoveSpawnerCommandFactory : CommandFactory
    {
        private readonly SpawnerPositions _spawnerPositions;
        public RemoveSpawnerCommandFactory(Levels.Logic.Grid grid, SpawnerPositions spawnerPositions) : base(grid)
        {
            _spawnerPositions = spawnerPositions;
        }

        public override ICommand CreateCommandAtCell(Vector2Int position)
        {
            return new RemoveSpawnerCommand(_spawnerPositions, position);
        }
    }
}
