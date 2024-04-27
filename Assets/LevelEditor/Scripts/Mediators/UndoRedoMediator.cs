using System;

namespace LevelEditor.Mediators
{
    public class UndoRedoMediator : IDisposable
    {
        private readonly LevelEditor _levelEditor;
        private readonly IUndoRedoSource _source;

        public UndoRedoMediator(LevelEditor levelEditor, IUndoRedoSource undoRedoSource)
        {
            _levelEditor = levelEditor;
            _source = undoRedoSource;

            _source.undo += OnUndoAction;
            _source.redo += OnRedoAction;
        }
        public void Dispose()
        {
            _source.undo -= OnUndoAction;
            _source.redo -= OnRedoAction;
        }

        private void OnUndoAction() => _levelEditor.UndoLastCommand();
        private void OnRedoAction() => _levelEditor.RedoCommand();
    }
}