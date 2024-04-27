using Levels.Logic;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor.Commands
{
    public class AddGroundAtCommand : ICommand
    {
        private Grid _grid;
        private Vector2Int _cellPosition;
        private readonly TileType _tileType;

        public AddGroundAtCommand(Grid grid, Vector2Int cellPosition, TileType tileType)
        {
            _grid = grid;
            _cellPosition = cellPosition;
            _tileType = tileType;
        }

        public bool Execute()
        {
            return _grid.CreateCellAt(_cellPosition, _tileType);
        }

        public void Undo()
        {
            _grid.RemoveCellAt(_cellPosition);
        }
    }
}
