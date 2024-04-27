using System;
using LevelEditor.UI;

namespace LevelEditor.Mediators
{
    public class LevelSettingsMediator : IDisposable
    {
        private SettingsMenu _settings;
        private LevelEditor _levelEditor;

        public LevelSettingsMediator(SettingsMenu settingsMenu, LevelEditor levelEditor)
        {
            _settings = settingsMenu;
            _levelEditor = levelEditor;

            _settings.StartMoney.changed+=OnStartMoneyChanged;
            _settings.TimeToTheFirstWave.changed+=OnFirstWaveDelayChanged;
            _settings.mapNameChanged+=OnLevelNameChanged;
        }
        public void Dispose()
        {
            _settings.StartMoney.changed-=OnStartMoneyChanged;
            _settings.TimeToTheFirstWave.changed-=OnFirstWaveDelayChanged;
            _settings.mapNameChanged-=OnLevelNameChanged;
        }

        private void OnStartMoneyChanged(int value) => _levelEditor.UpdateLevelStartMoney(value);
        private void OnFirstWaveDelayChanged(int value) => _levelEditor.UpdateLevelFirstWaveDelay(value);
        private void OnLevelNameChanged(string levelName) => _levelEditor.UpdateLevelName(levelName);
    }
}
