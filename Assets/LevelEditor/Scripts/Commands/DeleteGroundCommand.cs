using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor
{
    public class DeleteGroundCommand : ICommand
    {
        private Grid _grid;
        private Vector2Int _cellPosition;

        public DeleteGroundCommand(Grid grid, Vector2Int cellPosition)
        {
            _grid = grid;
            _cellPosition = cellPosition;
        }

        public bool Execute()
        {
            return  _grid.RemoveCellAt(_cellPosition);
        }

        public void Undo()
        {
            _grid.CreateCellAt(_cellPosition);
        }
    }
}
