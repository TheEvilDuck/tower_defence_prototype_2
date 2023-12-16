using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor
{
    public class DeleteGroundCommand : ICommand
    {
        private Grid _grid;
        private Vector2Int _position;

        public DeleteGroundCommand(Grid grid, Vector2Int position)
        {
            _grid = grid;
            _position = position;
        }

        public bool Execute()
        {
            return _grid.RemoveCellAt(_position);
        }

        public void Undo()
        {
            _grid.CreateCellAt(_position);
        }
    }
}
