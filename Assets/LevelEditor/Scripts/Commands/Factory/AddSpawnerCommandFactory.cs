using UnityEngine;

namespace LevelEditor.Commands.Factory
{
    public class AddSpawnerCommandFactory : CommandFactory
    {
        private readonly SpawnerPositions _spawnerPositions;
        public AddSpawnerCommandFactory(Levels.Logic.Grid grid, SpawnerPositions spawnerPositions) : base(grid)
        {
            _spawnerPositions = spawnerPositions;
        }

        public override ICommand CreateCommandAtCell(Vector2Int position)
        {
            return new AddSpawnerCommand(_spawnerPositions, position);
        }
    }
}
