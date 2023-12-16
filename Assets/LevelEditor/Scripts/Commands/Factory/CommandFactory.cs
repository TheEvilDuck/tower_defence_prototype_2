using UnityEngine;

namespace LevelEditor
{
    public abstract class CommandFactory
    {
        public abstract ICommand CreateCommandAtCellId(Vector2Int position);
    }
}
