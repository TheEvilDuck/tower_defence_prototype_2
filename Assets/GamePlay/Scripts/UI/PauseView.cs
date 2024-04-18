using System;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
    public class PauseView : MonoBehaviour, IMenuParent
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _resumeButton;
        public event Action restartButtonPressed;
        public event Action exitButtonPressed;
        public event Action resumeButtonPressed;
        public bool Active => gameObject.activeInHierarchy;

        private void OnEnable() 
        {
            _restartButton.onClick.AddListener(OnRestartButtonPressed);
            _exitButton.onClick.AddListener(OnExitButtonPressed);
            _resumeButton.onClick.AddListener(OnResumeButtonPressed);
        }

        private void OnDisable() 
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonPressed);
            _exitButton.onClick.RemoveListener(OnExitButtonPressed);
            _resumeButton.onClick.RemoveListener(OnResumeButtonPressed);
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        private void OnRestartButtonPressed() => restartButtonPressed?.Invoke();
        private void OnExitButtonPressed() => exitButtonPressed?.Invoke();
        private void OnResumeButtonPressed() => resumeButtonPressed?.Invoke();
    }
}