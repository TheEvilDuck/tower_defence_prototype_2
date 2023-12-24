using System.Collections.Generic;
using LevelEditor.Selectors;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor
{
    public class AddGroundAtCommand : ICommand
    {
        private Grid _grid;
        private Vector2Int _cellPosition;

        public AddGroundAtCommand(Grid grid, Vector2Int cellPosition)
        {
            _grid = grid;
            _cellPosition = cellPosition;
        }

        public bool Execute()
        {
            Debug.Log("G");
            return _grid.CreateCellAt(_cellPosition);
        }

        public void Undo()
        {
            _grid.RemoveCellAt(_cellPosition);
        }
    }
}
