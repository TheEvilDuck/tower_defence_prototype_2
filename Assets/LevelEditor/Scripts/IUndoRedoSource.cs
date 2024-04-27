using System;

namespace LevelEditor
{
    public interface IUndoRedoSource
    {
        public event Action undo;
        public event Action redo;
    }
}