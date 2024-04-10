using Levels.Logic;
using Levels.Tiles;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Grid = Levels.Logic.Grid;

namespace LevelEditor
{
    public class DeleteGroundCommand : ICommand
    {
        private Grid _grid;
        private Vector2Int _cellPosition;
        private TileType _deletedTile = TileType.Empty;

        public DeleteGroundCommand(Grid grid, Vector2Int cellPosition)
        {
            _grid = grid;
            _cellPosition = cellPosition;

            if (grid.TryGetCellDataAt(cellPosition, out CellData cellData))
            {
                _deletedTile = cellData.Type;
            }
        }

        public bool Execute()
        {
            return  _grid.RemoveCellAt(_cellPosition);
        }

        public void Undo()
        {
            _grid.CreateCellAt(_cellPosition, _deletedTile);
        }
    }
}
