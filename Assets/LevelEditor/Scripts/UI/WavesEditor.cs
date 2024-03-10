using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Waves;

namespace LevelEditor.UI
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
        private List<WaveData> _waveDatas;
        private Dictionary<WaveData, List<EnemySettings>>_enemySettingsView;
        private int _currentWaveId = 0;
        public bool Active => gameObject.activeInHierarchy;
        public IEnumerable<WaveData> WaveDatas => _waveDatas;

        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);

        private void Awake() 
        {
            _waveDatas = new List<WaveData>();
            _enemySettingsView = new Dictionary<WaveData, List<EnemySettings>>();
            _addEnemiesButton.gameObject.SetActive(false);
            UpdateCounter();
        }

        private void OnEnable() 
        {
            _previousWave.onClick.AddListener(OnPreviosButtonPressed);
            _nextWave.onClick.AddListener(OnNextButtonPressed);
            _deleteCurrentWave.onClick.AddListener(OnDeleteButtonPressed);
            _addNewWave.onClick.AddListener(OnAddButtonPressed);
            _addEnemiesButton.onClick.AddListener(OnAddEnemiesButtonPressed);
        }

        private void OnDisable() 
        {
            _previousWave.onClick.RemoveListener(OnPreviosButtonPressed);
            _nextWave.onClick.RemoveListener(OnNextButtonPressed);
            _deleteCurrentWave.onClick.RemoveListener(OnDeleteButtonPressed);
            _addNewWave.onClick.RemoveListener(OnAddButtonPressed);
            _addEnemiesButton.onClick.RemoveListener(OnAddEnemiesButtonPressed);
        }

        private void OnPreviosButtonPressed()
        {
            if (_waveDatas.Count==0)
                return;

            if (_enemySettingsView.TryGetValue(_waveDatas[_currentWaveId], out List<EnemySettings> enemySettingsOfPreviousWave))
            {
                foreach(EnemySettings enemySettings in enemySettingsOfPreviousWave)
                    enemySettings.Hide();
            }

            _currentWaveId--;

            if (_currentWaveId<0)
                _currentWaveId = 0;

            if (_enemySettingsView.TryGetValue(_waveDatas[_currentWaveId], out List<EnemySettings> enemySettingsOfWave))
            {
                foreach(EnemySettings enemySettings in enemySettingsOfWave)
                    enemySettings.Show();
            }

            UpdateCounter();
        }
        private void OnNextButtonPressed()
        {
            if (_waveDatas.Count==0)
                return;

            if (_enemySettingsView.TryGetValue(_waveDatas[_currentWaveId], out List<EnemySettings> enemySettingsOfPreviousWave))
            {
                foreach(EnemySettings enemySettings in enemySettingsOfPreviousWave)
                    enemySettings.Hide();
            }

            _currentWaveId++;

            if (_currentWaveId>=_waveDatas.Count)
                _currentWaveId = _waveDatas.Count-1;

            if (_enemySettingsView.TryGetValue(_waveDatas[_currentWaveId], out List<EnemySettings> enemySettingsOfWave))
            {
                foreach(EnemySettings enemySettings in enemySettingsOfWave)
                    enemySettings.Show();
            }

            UpdateCounter();
        }

        private void OnDeleteButtonPressed()
        {
            if (_waveDatas.Count == 0)
                return;

            if (_enemySettingsView.TryGetValue(_waveDatas[_currentWaveId], out List<EnemySettings> enemySettingsOfDeletedWave))
            {   
                for (int i = enemySettingsOfDeletedWave.Count-1; i>=0;i--)
                    enemySettingsOfDeletedWave[i].Delete();

                _enemySettingsView.Remove(_waveDatas[_currentWaveId]);
            }

            _waveDatas.RemoveAt(_currentWaveId);

            if (_currentWaveId>=_waveDatas.Count&&_waveDatas.Count!=0&&_currentWaveId>0)
            {   
                _currentWaveId--;
            }

            if (_waveDatas.Count==0)
                _addEnemiesButton.gameObject.SetActive(false);
            else
            {
                if (_enemySettingsView.TryGetValue(_waveDatas[_currentWaveId], out List<EnemySettings> enemySettingsOfWave))
                {
                    foreach(EnemySettings enemySettings in enemySettingsOfWave)
                        enemySettings.Show();
                }
            }
            

            UpdateCounter();
        }
        private void OnAddButtonPressed()
        {
            if (_waveDatas.Count>=MAX_WAVES_COUNT)
                return;
            
            _waveDatas.Add(new WaveData());
            _enemySettingsView.Add(_waveDatas[_waveDatas.Count-1], new List<EnemySettings>());
            _addEnemiesButton.gameObject.SetActive(true);
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            int currentIdView = _currentWaveId;

            if (_waveDatas.Count>0)
                currentIdView = _currentWaveId+1;

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

            
        }

        private void OnEnemySettingsDeleted(EnemySettings enemySettings)
        {
            enemySettings.deleteButtonPressed-=OnEnemySettingsDeleted;

            if (!_enemySettingsView.TryGetValue(_waveDatas[_currentWaveId], out List<EnemySettings> enemySettingsOfWave))
                throw new System.Exception($"You're trying to access wrong wavedata at {_currentWaveId}");

            enemySettingsOfWave.Remove(enemySettings);
        }
    }
}
