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

        public LevelData ConvertToLevelData()
        {
            LevelData levelData = new LevelData
            {
                gridData = Grid.ConvertToGridData(),
            };

            return levelData;
        }
    }
}
