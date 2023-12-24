using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor
{
    public class DrawCommandsFactory : CommandFactory
    {
        public DrawCommandsFactory(Grid grid) : base(grid)
        {
        }

        public override ICommand CreateCommandAtCell(Vector2Int position)
        {
            return new AddGroundAtCommand(_grid, position);
        }
    }
}
