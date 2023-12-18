using System;
using UnityEngine;

namespace Levels.Logic
{
    public class Level
    {
        public Grid Grid {get; private set;}

        public Level(LevelData levelData)
        {
            Grid = new Grid(levelData.gridData, 1f);
        }

        public LevelData ConvertToLevelData()
        {
            LevelData levelData = new LevelData();
            levelData.startMoney = 100;
            levelData.gridData = Grid.ConvertToGridData();

            return levelData;
        }
    }
}
