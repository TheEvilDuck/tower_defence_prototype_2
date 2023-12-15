using System;
using UnityEngine;

namespace Levels.Logic
{
    public class Level
    {
        public Grid Grid {get; private set;}

        public Level(LevelData levelData)
        {
            Grid = new Grid(levelData.gridSize, 1f);
        }

        public void Test()
        {
            Grid.CreateCellAt(new Vector2Int(0,0));
            Grid.CreateCellAt(new Vector2Int(1,0));
            Grid.CreateCellAt(new Vector2Int(2,0));
        }
    }
}
