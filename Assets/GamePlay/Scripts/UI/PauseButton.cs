using System;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
    public class PauseButton : MonoBehaviour, IMenuParent
    {
        [SerializeField] private Button _pauseButton;
        public event Action pauseButtonClicked;
        public bool Active => gameObject.activeInHierarchy;
        private void OnEnable() 
        {
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        private void OnDisable() 
        {
            _pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
        }
        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        private void OnPauseButtonClicked() => pauseButtonClicked?.Invoke();
    }
}
