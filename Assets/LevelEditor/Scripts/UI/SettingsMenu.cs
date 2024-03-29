using System;
using Common.Interfaces;
using Levels.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI
{
    public class SettingsMenu : MonoBehaviour, IMenuParent
    {
        [SerializeField] private SliderWithText _startMoney;
        [SerializeField] private SliderWithText _timeToTheFirstWave;
        [SerializeField] private TMP_InputField _mapName;

        public event Action<string> mapNameChanged;
        public bool Active => gameObject.activeInHierarchy;
        public IObservableValue<int> StartMoney => _startMoney;
        public IObservableValue<int> TimeToTheFirstWave => _timeToTheFirstWave;


        private void OnEnable() 
        {
            _mapName.onEndEdit.AddListener(OnInputEditingEnd);
        }
        private void OnDisable() 
        {
            _mapName.onEndEdit.RemoveListener(OnInputEditingEnd);
        }
        public void Hide() => gameObject.SetActive(false);

        public void Show()=> gameObject.SetActive(true);
        public void RestoreDefaultValues()
        {
            _mapName.text = string.Empty;
            mapNameChanged?.Invoke(string.Empty);

            _timeToTheFirstWave.RestoreDefaultValue();
            _startMoney.RestoreDefaultValue();
        }

        public void LoadLevelName(string mapName)
        {
            _mapName.text = mapName;
            mapNameChanged?.Invoke(_mapName.text);
        }

        public void LoadFromLevelData(LevelData levelData)
        {
            _timeToTheFirstWave.SetValue((int)levelData.firstWaveDelay);
            _startMoney.SetValue((int)levelData.startMoney);
        }

        private void OnInputEditingEnd(string newName) => mapNameChanged?.Invoke(newName);
    }
}
