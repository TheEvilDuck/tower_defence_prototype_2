using System;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI
{
    public class SpawnersMenu : MonoBehaviour, IMenuParent
    {
        [SerializeField] private Button _drawButton;
        [SerializeField] private Button _deleteButton;

        public event Action drawButtonClicked;
        public event Action deleteButtonClicked;

        public bool Active => gameObject.activeInHierarchy;

        private void OnEnable() 
        {
            _drawButton.onClick.AddListener(OnDrawButtonPressed);
            _deleteButton.onClick.AddListener(OnDeletePressed);
        }
        private void OnDisable() 
        {
            _drawButton.onClick.RemoveListener(OnDrawButtonPressed);
            _deleteButton.onClick.RemoveListener(OnDeletePressed);
        }
        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);

        private void OnDrawButtonPressed() => drawButtonClicked?.Invoke();

        private void OnDeletePressed() => deleteButtonClicked?.Invoke();
    }
}