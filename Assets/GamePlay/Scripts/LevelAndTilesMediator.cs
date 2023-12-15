using System;
using System.Collections;
using System.Collections.Generic;
using Levels.Logic;
using Levels.TileControl;
using UnityEngine;

namespace GamePlay
{
    public class LevelAndTilesMediator: IDisposable
    {
        private TileController _tileController;
        private Level _level;

        public LevelAndTilesMediator(TileController tileController, Level level)
        {
            _tileController = tileController;
            _level = level;

            _level.Grid.cellChanged+=OnLevelCellChanged;
        }
        public void Dispose()
        {
            _level.Grid.cellChanged-=OnLevelCellChanged;
        }

        private void OnLevelCellChanged(Vector2Int cellPosition)
        {
            _tileController.DrawAt(cellPosition);
        }
    }
}
