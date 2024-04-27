using System;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI
{
    public class UndoRedoButtons : MonoBehaviour, IUndoRedoSource
    {
        [SerializeField] private Button _undoButton;
        [SerializeField] private Button _redoButton;

        public event Action undo;
        public event Action redo;

        private void Awake() 
        {
            _undoButton.onClick.AddListener(OnUndoButtonPressed);
            _redoButton.onClick.AddListener(OnRedoButtonPressed);
        }

        private void OnDestroy() 
        {
            _undoButton.onClick.RemoveListener(OnUndoButtonPressed);
            _redoButton.onClick.RemoveListener(OnRedoButtonPressed);
        }

        private void OnUndoButtonPressed() => undo?.Invoke();
        private void OnRedoButtonPressed() => redo?.Invoke();
    }
}
