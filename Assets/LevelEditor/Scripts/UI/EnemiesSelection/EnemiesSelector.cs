using System;
using System.Collections.Generic;
using Common;
using Enemies;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI.EnemiesSelection
{
    public class EnemiesSelector : MonoBehaviour
    {
        [SerializeField] private EnemyButton _enemyViewButtonPrefab;
        [SerializeField] private Transform _contentTransform;
        [SerializeField] private EnemiesDatabase _enemiesDatabase;
        [SerializeField] private EnemyInfo _enemyInfo;
        [SerializeField] private Button _selectedButton;
        [SerializeField] private Button _cancelButton;

        public event Action<EnemyEnum> enemySelected;
        public event Action canceled;
        private EnemyEnum _selectedEnemy;
        private Dictionary<EnemyButton, Action> _buttons;
        private GameObjectIconProvider<EnemyEnum> _gameObjectIconProvider;

        public void Init(GameObjectIconProvider<EnemyEnum> gameObjectIconProvider)
        {
            _gameObjectIconProvider = gameObjectIconProvider;

            _buttons = new Dictionary<EnemyButton, Action>();

            foreach (var databaseItem in _enemiesDatabase.Items)
            {
                EnemyButton button = Instantiate(_enemyViewButtonPrefab, _contentTransform);

                Action OnClick = () =>
                {
                    _selectedEnemy = databaseItem.Key;
                    _enemyInfo.UpdateInfo(databaseItem.Value);
                };

                button.clicked += OnClick;
                _buttons.Add(button, OnClick);

                button.UpdateTexture(_gameObjectIconProvider.Get(databaseItem.Key));
            }

            _selectedButton.onClick.AddListener(OnSelectedButtonPressed);
            _cancelButton.onClick.AddListener(Hide);


        }

        private void OnDestroy() 
        {
            foreach (var keyValuePair in _buttons)
                keyValuePair.Key.clicked -= keyValuePair.Value;

            _selectedButton.onClick.RemoveListener(OnSelectedButtonPressed);
            _cancelButton.onClick.RemoveListener(Hide);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            canceled?.Invoke();
        }

        public void ShowWithPreloadedId(EnemyEnum id)
        {
            if (!_enemiesDatabase.TryGetValue(id, out var config))
                throw new Exception("AAAAAAAAAA");

            _selectedEnemy = id;
            _enemyInfo.UpdateInfo(config);
        }

        private void OnSelectedButtonPressed()
        {
            if (!_enemiesDatabase.TryGetValue(_selectedEnemy, out var config))
                throw new Exception();

            enemySelected?.Invoke(_selectedEnemy);
            gameObject.SetActive(false);
        }
    }

}