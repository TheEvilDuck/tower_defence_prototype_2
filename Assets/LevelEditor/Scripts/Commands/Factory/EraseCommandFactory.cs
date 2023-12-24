using Grid = Levels.Logic.Grid;
using UnityEngine;

namespace LevelEditor
{
    public class EraseCommandFactory : CommandFactory
    {
        public EraseCommandFactory(Grid grid) : base(grid)
        {
        }

        public override ICommand CreateCommandAtCell(Vector2Int position)
        {
            return new DeleteGroundCommand(_grid, position);
        }
    }
}
