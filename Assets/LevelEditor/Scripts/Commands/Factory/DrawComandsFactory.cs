using System;
using Levels.Tiles;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor
{
    public class DrawCommandsFactory : CommandFactory
    {
        private TileType _tileType;
        public DrawCommandsFactory(Grid grid) : base(grid)
        {
            _tileType = TileType.Dirt;
        }

        public void ChangeTileType(TileType tileType)
        {
            if (tileType == TileType.Empty)
                throw new ArgumentException("Draw command must not use empty tile type");

            _tileType = tileType;
        }

        public override ICommand CreateCommandAtCell(Vector2Int position)
        {
            return new AddGroundAtCommand(_grid, position, _tileType);
        }
    }
}
