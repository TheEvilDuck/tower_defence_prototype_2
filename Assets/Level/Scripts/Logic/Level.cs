using Waves;

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

        public LevelData ConvertToLevelData(int startMoney, float timeToTheFirstWave, WaveData[] waveDatas)
        {
            LevelData levelData = new LevelData
            {
                startMoney = startMoney,
                firstWaveDelay = timeToTheFirstWave,
                gridData = Grid.ConvertToGridData(),
                waves = waveDatas
            };

            return levelData;
        }
    }
}
