using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor
{
    public abstract class CommandFactory
    {
        protected Grid _grid;

        public CommandFactory(Grid grid)
        {
            _grid = grid;
        }
        public abstract ICommand CreateCommandAtCell(Vector2Int position);
    }
}
