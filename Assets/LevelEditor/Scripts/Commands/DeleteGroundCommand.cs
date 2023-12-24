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
            bool t = _grid.RemoveCellAt(_cellPosition);
            Debug.Log(t);
            return t;
        }

        public void Undo()
        {
            _grid.CreateCellAt(_cellPosition);
        }
    }
}
