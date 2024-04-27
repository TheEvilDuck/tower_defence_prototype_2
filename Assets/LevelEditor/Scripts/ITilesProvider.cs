using System;
using Levels.Logic;

namespace LevelEditor
{
    public interface ITilesProvider
    {
        public event Action<TileType> tileChanged;
    }
}