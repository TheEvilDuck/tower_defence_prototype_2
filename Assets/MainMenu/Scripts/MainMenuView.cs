using System;
using Common.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuView : MonoBehaviour, IMenuParent
    {
        [SerializeField]private Button _playButton;
        [SerializeField]private Button _levelEditorButton;
        [SerializeField]private Button _settingsButton;
        [SerializeField]private Button _exitButton;


        public event Action playButtonPressed;
        public event Action levelEditorButtonPressed;
        public event Action settingsButtonPressed;
        public event Action exitButtonPressed;

        public bool Active => gameObject.activeInHierarchy;

        private void OnEnable() 
        {
            _playButton.onClick.AddListener(OnPlayButtonPressed);
            _levelEditorButton.onClick.AddListener(OnLevelEditorButtonPressed);
            _settingsButton.onClick.AddListener(OnSettingsButtonPressed);
            _exitButton.onClick.AddListener(OnExitButtonPressed);
        }

        private void OnDisable() 
        {
            _playButton.onClick.RemoveListener(OnPlayButtonPressed);
            _levelEditorButton.onClick.RemoveListener(OnLevelEditorButtonPressed);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonPressed);
            _exitButton.onClick.RemoveListener(OnExitButtonPressed);
        }

        public void Hide()=> gameObject.SetActive(false);

        public void Show()=> gameObject.SetActive(true);

        private void OnPlayButtonPressed() => playButtonPressed?.Invoke();
        private void OnLevelEditorButtonPressed() => levelEditorButtonPressed?.Invoke();
        private void OnSettingsButtonPressed() => settingsButtonPressed?.Invoke();
        private void OnExitButtonPressed() => exitButtonPressed?.Invoke();
    }
}
