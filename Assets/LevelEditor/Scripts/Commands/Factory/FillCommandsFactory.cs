using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor
{
    public class FillCommandsFactory : CommandFactory
    {
        private Grid _grid;

        public FillCommandsFactory(Grid grid)
        {
            _grid = grid;
        }
        public override ICommand CreateCommandAtCellId(Vector2Int position)
        {
            return new FillCommand(_grid,position);
        }
    }
}
