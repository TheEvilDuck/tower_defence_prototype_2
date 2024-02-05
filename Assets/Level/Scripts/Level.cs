using System;
using UnityEngine;

namespace Levels.Logic
{
    public class Level
    {
        public Grid Grid {get; private set;}

        public Level(LevelData levelData)
        {
            Grid = new Grid(levelData.gridData.gridSize, 1f);
        }

        public void UpdateGridData(GridData newGridData)
        {
            Grid.FillFromGridData(newGridData);
        }

        public LevelData ConvertToLevelData(int startMoney, float timeToTheFirstWave)
        {
            LevelData levelData = new LevelData();
            levelData.startMoney = startMoney;
            levelData.firstWaveDelay = timeToTheFirstWave;
            levelData.gridData = Grid.ConvertToGridData();

            return levelData;
        }
    }
}
