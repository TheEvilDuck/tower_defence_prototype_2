using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI
{
    public class ButtonsBar : MonoBehaviour
    {
        [SerializeField] private Button _deleteButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _wavesButton;
        public event Action deleteButtonPressed;
        public event Action saveButtonPressed;
        public event Action loadButtonPressed;
        public event Action settingsButtonPressed;
        public event Action exitButtonPressed;
        public event Action wavesButtonPressed;

        private void OnEnable() 
        {
            _deleteButton.onClick.AddListener(OnDeleteButtonPressed);
            _saveButton.onClick.AddListener(OnSaveButtonPressed);
            _loadButton.onClick.AddListener(OnLoadButtonPressed);
            _settingsButton.onClick.AddListener(OnSettingsButtonPressed);
            _exitButton.onClick.AddListener(OnExitButtonPressed);
            _wavesButton.onClick.AddListener(OnWavesButtonPressed);
        }

        private void OnDisable() 
        {
            _deleteButton.onClick.RemoveListener(OnDeleteButtonPressed);
            _saveButton.onClick.RemoveListener(OnSaveButtonPressed);
            _loadButton.onClick.RemoveListener(OnLoadButtonPressed);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonPressed);
            _exitButton.onClick.RemoveListener(OnExitButtonPressed);
            _wavesButton.onClick.RemoveListener(OnWavesButtonPressed);
        }

        private void OnDeleteButtonPressed() => deleteButtonPressed?.Invoke();
        private void OnSaveButtonPressed() => saveButtonPressed?.Invoke();
        private void OnLoadButtonPressed() => loadButtonPressed?.Invoke();
        private void OnSettingsButtonPressed() => settingsButtonPressed?.Invoke();
        private void OnExitButtonPressed() => exitButtonPressed?.Invoke();
        private void OnWavesButtonPressed() => wavesButtonPressed?.Invoke();

    }
}
