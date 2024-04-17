using System;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class LevelSelectorView : MonoBehaviour, IMenuParent
    {
        [SerializeField] private Button _backButton;

        public event Action backButtonPressed;
        public bool Active => gameObject.activeInHierarchy;

        private void OnEnable() 
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnDisable() 
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);
        private void OnBackButtonClicked() => backButtonPressed?.Invoke();
    }
}
