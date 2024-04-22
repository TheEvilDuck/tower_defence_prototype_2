using System;
using System.Collections.Generic;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LevelEditor.UI
{
    public class EnemiesSelector : MonoBehaviour
    {
        [SerializeField] private EnemyButton _enemyViewButtonPrefab;
        [SerializeField] private Transform _contentTransform;
        [SerializeField] private EnemiesDatabase _enemiesDatabase;
        [SerializeField] private EnemyInfo _enemyInfo;
        [SerializeField] private Button _selectedButton;

        public event Action<EnemyEnum,string> enemySelected;
        private EnemyEnum _selectedEnemy;
        private Dictionary<EnemyButton, Action> _buttons;

        private void Awake()
        {
            _buttons = new Dictionary<EnemyButton, Action>();

            foreach (var databaseItem in _enemiesDatabase.Items)
            {
                EnemyButton button = Instantiate(_enemyViewButtonPrefab, _contentTransform);
                button.UpdateText(databaseItem.Value.Name);

                Action OnClick = () =>
                {
                    _selectedEnemy = databaseItem.Key;
                    _enemyInfo.UpdateInfo(databaseItem.Value);
                };

                button.clicked += OnClick;
                _buttons.Add(button, OnClick);
            }

            _selectedButton.onClick.AddListener(OnSelectedButtonPressed);
        }

        private void OnDestroy() 
        {
            foreach (var keyValuePair in _buttons)
                keyValuePair.Key.clicked -= keyValuePair.Value;

            _selectedButton.onClick.RemoveListener(OnSelectedButtonPressed);
        }

        private void OnSelectedButtonPressed()
        {
            if (!_enemiesDatabase.TryGetValue(_selectedEnemy, out var config))
                throw new Exception();

            enemySelected?.Invoke(_selectedEnemy, config.Name);
            gameObject.SetActive(false);
        }
    }

}