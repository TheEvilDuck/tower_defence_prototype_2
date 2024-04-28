using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Common.UI;
using Enemies;
using LevelEditor.UI.EnemiesSelection;
using Levels.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Waves;

namespace LevelEditor.UI.WavesEditing
{
    public class WavesEditor : MonoBehaviour, IMenuParent
    {
        private const int MAX_WAVES_COUNT = 20;
        private const int MAX_ENEMIES_IN_WAVE = 7;
        private const int MAX_ENEMY_COUNT_IN_ENEMY_SETTINGS = 20;
        [SerializeField]private Button _previousWave;
        [SerializeField]private Button _nextWave;
        [SerializeField]private Button _deleteCurrentWave;
        [SerializeField]private Button _addNewWave;
        [SerializeField]private Button _addEnemiesButton;
        [SerializeField]private SliderWithText _timeToTheNextWaveSlider;
        [SerializeField]private TextMeshProUGUI _counter;
        [SerializeField]private EnemySettings _enemySettingsPrefab;
        [SerializeField]private Transform _enemySettingsParent;
        [SerializeField]private EnemiesSelector _enemiesSelector;
        private List<WaveData> _waveDatas;
        private Dictionary<WaveData, List<EnemySettings>>_enemySettingsView;
        private int _currentWaveId = 0;
        private bool _inited = false;
        private GameObjectIconProvider<EnemyEnum> _gameObjectIconProvider;
        public bool Active => gameObject.activeInHierarchy;
        public IEnumerable<WaveData> WaveDatas => _waveDatas;
        private int CurrentWaveId
        {
            get
            {
                if (_waveDatas.Count>0)
                    _currentWaveId = Mathf.Clamp(_currentWaveId,0,_waveDatas.Count-1);
                else
                    _currentWaveId = 0;

                return _currentWaveId;
            }

            set
            {
                if (_waveDatas.Count > 0)
                    _currentWaveId = Mathf.Clamp(value,0,_waveDatas.Count-1);
                else
                    _currentWaveId = 0;
            }
        }

        public void Hide()
        {
            _enemiesSelector.Hide();
            gameObject.SetActive(false);
        }

        public void Show() => gameObject.SetActive(true);

        private void OnEnable() 
        {
            _previousWave.onClick.AddListener(OnPreviosButtonPressed);
            _nextWave.onClick.AddListener(OnNextButtonPressed);
            _deleteCurrentWave.onClick.AddListener(OnDeleteButtonPressed);
            _addNewWave.onClick.AddListener(OnAddButtonPressed);
            _addEnemiesButton.onClick.AddListener(OnAddEnemiesButtonPressed);
            _timeToTheNextWaveSlider.changed+=OnTimeToTheNextWaveSliderChanged;
        }

        private void OnDisable() 
        {
            _previousWave.onClick.RemoveListener(OnPreviosButtonPressed);
            _nextWave.onClick.RemoveListener(OnNextButtonPressed);
            _deleteCurrentWave.onClick.RemoveListener(OnDeleteButtonPressed);
            _addNewWave.onClick.RemoveListener(OnAddButtonPressed);
            _addEnemiesButton.onClick.RemoveListener(OnAddEnemiesButtonPressed);
            _timeToTheNextWaveSlider.changed-=OnTimeToTheNextWaveSliderChanged;
        }

        public void Init(GameObjectIconProvider<EnemyEnum> gameObjectIconProvider)
        {
            _gameObjectIconProvider = gameObjectIconProvider;
            _waveDatas = new List<WaveData>();
            _enemySettingsView = new Dictionary<WaveData, List<EnemySettings>>();
            _addEnemiesButton.gameObject.SetActive(false);
            UpdateCounter();
            _inited = true;
        }

        public void DeleteCurrentData()
        {
            if (_waveDatas.Count == 0)
                return;

            for (_currentWaveId = 0; _currentWaveId<_waveDatas.Count;_currentWaveId++)
            {
                if (_enemySettingsView.TryGetValue(_waveDatas[_currentWaveId], out List<EnemySettings> enemySettingsList))
                {
                    for (int i = enemySettingsList.Count-1;i>=0;i--)
                    {
                        enemySettingsList[i].Delete();
                    }
                }
            }

            _enemySettingsView.Clear();
            _waveDatas.Clear();
            _currentWaveId = 0;

            UpdateCounter();
            _addEnemiesButton.gameObject.SetActive(false);
        }

        public void LoadFromLevelData(LevelData levelData)
        {
            DeleteCurrentData();
            CurrentWaveId = 0;

            foreach (WaveData waveData in levelData.waves)
            {
                if (!TryAddNewWave(waveData))
                    break;

                if (!_enemySettingsView.TryGetValue(waveData, out List<EnemySettings> enemySettingsOfWave))
                    continue;

                foreach (WaveEnemyData waveEnemyData in waveData.waveEnemyData)
                {
                    EnemySettings enemySettings = Instantiate(_enemySettingsPrefab, _enemySettingsParent);
                    enemySettingsOfWave.Add(enemySettings);
                    enemySettings.deleteButtonPressed+=OnEnemySettingsDeleted;

                    enemySettings.Init(MAX_ENEMY_COUNT_IN_ENEMY_SETTINGS, _enemiesSelector, _gameObjectIconProvider);
                    enemySettings.FillFromWaveEnemyData(waveEnemyData);

                    enemySettings.Hide();
                }

                CurrentWaveId++;

                _timeToTheNextWaveSlider.SetValue((int)waveData.timeToTheNextWave);
            }

            CurrentWaveId = 0;

            if (levelData.waves.Length>0)
            {
                CurrentWaveId = levelData.waves.Length - 1;
                _addEnemiesButton.gameObject.SetActive(true);

                ShowCurrentEnemySettings();
            }
            
            UpdateCounter();
        }

        public void FillWaveDatasWithEnemyDatas()
        {
            if (_waveDatas.Count == 0)
                return;

            foreach (var waveDataAndSettingsList in _enemySettingsView)
            {
                if (waveDataAndSettingsList.Value==null)
                    continue;

                if (waveDataAndSettingsList.Value.Count==0)
                    continue;

                List<WaveEnemyData> waveEnemyDatas = new List<WaveEnemyData>();

                foreach (EnemySettings enemySettings in waveDataAndSettingsList.Value)
                {
                    WaveEnemyData waveEnemyData = new WaveEnemyData
                    {
                        count = enemySettings.Count,
                        enemyData = enemySettings.EnemyId
                    };
                    
                    waveEnemyDatas.Add(waveEnemyData);
                }

                waveDataAndSettingsList.Key.waveEnemyData = waveEnemyDatas.ToArray();
            }
        }

        private void OnPreviosButtonPressed() => OffsetWave(-1);
        private void OnNextButtonPressed() => OffsetWave(1);

        private void OffsetWave(int offset)
        {
             if (_waveDatas.Count==0)
                return;

            if (CurrentWaveId+offset==CurrentWaveId)
                return;

            HideCurrentEnemySettings();

            CurrentWaveId+=offset;

            ShowCurrentEnemySettings();

            UpdateCounter();
            _timeToTheNextWaveSlider.SetValue((int)_waveDatas[CurrentWaveId].timeToTheNextWave);
        }

        private void HideCurrentEnemySettings()
        {
            if (_waveDatas.Count==0)
                return;

            if (_enemySettingsView.TryGetValue(_waveDatas[CurrentWaveId], out List<EnemySettings> enemySettingsOfPreviousWave))
            {
                foreach(EnemySettings enemySettings in enemySettingsOfPreviousWave)
                    enemySettings.Hide();
            }
        }

        private void ShowCurrentEnemySettings()
        {
            if (_waveDatas.Count==0)
                return;

            if (_enemySettingsView.TryGetValue(_waveDatas[CurrentWaveId], out List<EnemySettings> enemySettingsOfPreviousWave))
            {
                foreach(EnemySettings enemySettings in enemySettingsOfPreviousWave)
                    enemySettings.Show();
            }
        }

        private void OnDeleteButtonPressed()
        {
            if (_waveDatas.Count == 0)
                return;

            if (_enemySettingsView.TryGetValue(_waveDatas[CurrentWaveId], out List<EnemySettings> enemySettingsOfDeletedWave))
            {   
                for (int i = enemySettingsOfDeletedWave.Count-1; i>=0;i--)
                    enemySettingsOfDeletedWave[i].Delete();

                _enemySettingsView.Remove(_waveDatas[CurrentWaveId]);
            }

            _waveDatas.RemoveAt(CurrentWaveId);

            CurrentWaveId--;

            if (_waveDatas.Count==0)
                _addEnemiesButton.gameObject.SetActive(false);
            else
            {
                ShowCurrentEnemySettings();
            }
            

            UpdateCounter();
        }

        private bool TryAddNewWave(WaveData waveData)
        {
            if (_waveDatas.Count>=MAX_WAVES_COUNT)
                return false;
            
            _waveDatas.Add(waveData);
            _enemySettingsView.Add(waveData, new List<EnemySettings>());
            _addEnemiesButton.gameObject.SetActive(true);
            UpdateCounter();

            return true;
        }
        private void OnAddButtonPressed() => TryAddNewWave(new WaveData());

        private void UpdateCounter()
        {
            int currentIdView = CurrentWaveId;

            if (_waveDatas.Count>0)
            {
                currentIdView = CurrentWaveId+1;
                _timeToTheNextWaveSlider.SetValue((int)_waveDatas[CurrentWaveId].timeToTheNextWave);
            }

            _counter.text = $"Wave: {currentIdView}/{_waveDatas.Count}";
        }

        private void OnAddEnemiesButtonPressed()
        {
            if (_waveDatas.Count==0)
                return;

            List<EnemySettings> enemySettingsOfWave;

            if (_enemySettingsView.TryGetValue(_waveDatas[_currentWaveId], out enemySettingsOfWave))
            {
                if (enemySettingsOfWave.Count>MAX_ENEMIES_IN_WAVE)
                    return;
            }
            else
            {
                enemySettingsOfWave = new List<EnemySettings>();
                _enemySettingsView.TryAdd(_waveDatas[_currentWaveId],enemySettingsOfWave);
            }
            
            EnemySettings enemySettings = Instantiate(_enemySettingsPrefab, _enemySettingsParent);
            enemySettingsOfWave.Add(enemySettings);
            enemySettings.deleteButtonPressed+=OnEnemySettingsDeleted;

            enemySettings.Init(MAX_ENEMY_COUNT_IN_ENEMY_SETTINGS, _enemiesSelector, _gameObjectIconProvider);

            
        }

        private void OnEnemySettingsDeleted(EnemySettings enemySettings)
        {
            enemySettings.deleteButtonPressed-=OnEnemySettingsDeleted;

            if (!_enemySettingsView.TryGetValue(_waveDatas[_currentWaveId], out List<EnemySettings> enemySettingsOfWave))
                throw new System.Exception($"You're trying to access wrong wavedata at {_currentWaveId}");

            enemySettingsOfWave.Remove(enemySettings);
        }

        private void OnTimeToTheNextWaveSliderChanged(int value)
        {
            if (_waveDatas.Count>0)
            {
                _waveDatas[_currentWaveId].timeToTheNextWave = value;
            }
        }
    }
}
