using System;
using Levels.Logic;
using Levels.Tiles;
using UnityEngine;

namespace GamePlay.Mediators
{
    public class LevelAndTilesMediator: IDisposable
    {
        private TileController _tileController;
        private Level _level;

        public LevelAndTilesMediator(TileController tileController, Level level)
        {
            _tileController = tileController;
            _level = level;

            _level.Grid.cellAdded+=OnLevelCellAdded;
        }
        public void Dispose()
        {
            _level.Grid.cellAdded-=OnLevelCellAdded;
        }

        private void OnLevelCellAdded(Vector2Int cellPosition, TileType tileType)
        {
            _tileController.DrawAt(cellPosition, tileType);
        }
    }
}
