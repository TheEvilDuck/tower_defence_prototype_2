using System;
using Levels.Tiles;

namespace LevelEditor
{
    public interface ITilesProvider
    {
        public event Action<TileType> tileChanged;
    }
}