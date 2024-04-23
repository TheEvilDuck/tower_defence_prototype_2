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
        [SerializeField] private Button _newButton;
        [SerializeField] private Button _spawnerButton;
        [SerializeField] private Button _toolsButton;
        [SerializeField] private Button _towersButton;
        public event Action deleteButtonPressed;
        public event Action saveButtonPressed;
        public event Action loadButtonPressed;
        public event Action settingsButtonPressed;
        public event Action exitButtonPressed;
        public event Action wavesButtonPressed;
        public event Action newButtonPressed;
        public event Action spawnerButtonPressed;
        public event Action toolsButtonPressed;
        public event Action towersButtonPressed;

        private void OnEnable() 
        {
            _deleteButton.onClick.AddListener(OnDeleteButtonPressed);
            _saveButton.onClick.AddListener(OnSaveButtonPressed);
            _loadButton.onClick.AddListener(OnLoadButtonPressed);
            _settingsButton.onClick.AddListener(OnSettingsButtonPressed);
            _exitButton.onClick.AddListener(OnExitButtonPressed);
            _wavesButton.onClick.AddListener(OnWavesButtonPressed);
            _newButton.onClick.AddListener(OnNewButtonPressed);
            _spawnerButton.onClick.AddListener(OnSpawnerButtonPressed);
            _toolsButton.onClick.AddListener(OnToolsButtonPressed);
            _towersButton.onClick.AddListener(OnTowersButtonPressed);
        }

        private void OnDisable() 
        {
            _deleteButton.onClick.RemoveListener(OnDeleteButtonPressed);
            _saveButton.onClick.RemoveListener(OnSaveButtonPressed);
            _loadButton.onClick.RemoveListener(OnLoadButtonPressed);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonPressed);
            _exitButton.onClick.RemoveListener(OnExitButtonPressed);
            _wavesButton.onClick.RemoveListener(OnWavesButtonPressed);
            _newButton.onClick.RemoveListener(OnNewButtonPressed);
            _spawnerButton.onClick.RemoveListener(OnSpawnerButtonPressed);
            _toolsButton.onClick.RemoveListener(OnToolsButtonPressed);
            _towersButton.onClick.RemoveListener(OnTowersButtonPressed);
        }

        private void OnDeleteButtonPressed() => deleteButtonPressed?.Invoke();
        private void OnSaveButtonPressed() => saveButtonPressed?.Invoke();
        private void OnLoadButtonPressed() => loadButtonPressed?.Invoke();
        private void OnSettingsButtonPressed() => settingsButtonPressed?.Invoke();
        private void OnExitButtonPressed() => exitButtonPressed?.Invoke();
        private void OnWavesButtonPressed() => wavesButtonPressed?.Invoke();
        private void OnNewButtonPressed() => newButtonPressed?.Invoke();
        private void OnSpawnerButtonPressed() => spawnerButtonPressed?.Invoke();
        private void OnToolsButtonPressed() => toolsButtonPressed?.Invoke();
        private void OnTowersButtonPressed() => towersButtonPressed?.Invoke();

    }
}
