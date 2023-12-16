using System.Collections.Generic;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor
{
    public class FillCommand : ICommand
    {
        private readonly Grid _grid;
        private readonly Vector2Int _startAt;
        private List<Vector2Int>_affectedCells;
        private readonly Vector2Int[] _directions = {Vector2Int.right,Vector2Int.up,Vector2Int.left,Vector2Int.down};

        public FillCommand(Grid grid, Vector2Int startAt)
        {
            _grid = grid;
            _startAt = startAt;
            _affectedCells = new List<Vector2Int>();
        }

        public bool Execute()
        {
            if (!_grid.CreateCellAt(_startAt))
                return false;

            _affectedCells.Add(_startAt);
            FillInDirectionsAround(_startAt);

            return true;
        }

        private void FillInDirectionsAround(Vector2Int center)
        {
            foreach (Vector2Int direction in _directions)
            {
                if (!_affectedCells.Contains(center+direction))
                {
                    if(_grid.CreateCellAt(center+direction))
                    {
                        _affectedCells.Add(center+direction);
                        FillInDirectionsAround(center+direction);
                    }
                }
            }
        }

        public void Undo()
        {
            foreach (Vector2Int affectedCellId in _affectedCells)
                _grid.RemoveCellAt(affectedCellId);
        }
    }
}
