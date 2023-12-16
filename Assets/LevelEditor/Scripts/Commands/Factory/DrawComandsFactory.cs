using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor
{
    public class DrawCommandsFactory : CommandFactory
    {
        private Grid _grid;

        public DrawCommandsFactory(Grid grid)
        {
            _grid = grid;
        }
        public override ICommand CreateCommandAtCellId(Vector2Int position)
        {
            return new AddGroundAtCommand(_grid,position);
        }
    }
}
