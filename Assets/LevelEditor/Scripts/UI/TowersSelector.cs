using System;
using System.Collections.Generic;
using Enemies;
using Towers;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI
{
    public class TowersSelector : MonoBehaviour
    {
        [SerializeField] private EnemyButton _enemyViewButtonPrefab;
        [SerializeField] private Transform _contentTransform;
        [SerializeField] private TowersDatabase _towersDatabase;
        [SerializeField] private Button _selectedButton;
        [SerializeField] private Button _cancelButton;

        public event Action<PlacableEnum> towerSelected;
        public event Action canceled;
        private PlacableEnum _selectedTower;
        private Dictionary<EnemyButton, Action> _buttons;
        private GameObjectIconProvider<PlacableEnum> _gameObjectIconProvider;

        public void Init(GameObjectIconProvider<PlacableEnum> gameObjectIconProvider)
        {
            _gameObjectIconProvider = gameObjectIconProvider;

            _buttons = new Dictionary<EnemyButton, Action>();

            foreach (var databaseItem in _towersDatabase.Items)
            {
                EnemyButton button = Instantiate(_enemyViewButtonPrefab, _contentTransform);

                Action OnClick = () =>
                {
                    _selectedTower = databaseItem.Key;
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

        public void ShowWithPreloadedId(PlacableEnum id)
        {
            if (!_towersDatabase.TryGetValue(id, out var config))
                throw new Exception("AAAAAAAAAA");

            _selectedTower = id;
        }

        private void OnSelectedButtonPressed()
        {
            if (!_towersDatabase.TryGetValue(_selectedTower, out var config))
                throw new Exception();

            towerSelected?.Invoke(_selectedTower);
            gameObject.SetActive(false);
        }
    }

}