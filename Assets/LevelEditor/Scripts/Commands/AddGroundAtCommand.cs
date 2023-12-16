using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor
{
    public class AddGroundAtCommand : ICommand
    {
        private Grid _grid;
        private Vector2Int _position;

        public AddGroundAtCommand(Grid grid, Vector2Int position)
        {
            _grid = grid;
            _position = position;
        }

        public bool Execute()
        {
            return _grid.CreateCellAt(_position);
        }

        public void Undo()
        {
            _grid.RemoveCellAt(_position);
        }
    }
}
