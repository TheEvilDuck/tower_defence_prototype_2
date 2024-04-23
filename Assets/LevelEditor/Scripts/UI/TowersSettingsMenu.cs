using System;
using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;
using TMPro;
using Towers;
using UnityEngine;

namespace LevelEditor.UI
{
    public class TowersSettingsMenu : MonoBehaviour, IMenuParent
    {
        [SerializeField] private EnemyButton _enemyViewButtonPrefab;
        [SerializeField] private Transform _contentTransform;
        [SerializeField] private TowersDatabase _towerDatabase;
        [SerializeField] private TextMeshProUGUI _statsText;
        public bool Active => gameObject.activeInHierarchy;
        private Dictionary<EnemyButton, Action> _buttons;
        private GameObjectIconProvider<PlacableEnum> _gameObjectIconProvider;
        private PlacableStatsTextFactory _statsTextFactory;
        private List<PlacableEnum> _selectedTowers;

        public IEnumerable<PlacableEnum> SelectedTowers => _selectedTowers;

        public void Init(GameObjectIconProvider<PlacableEnum> gameObjectIconProvider)
        {
            _gameObjectIconProvider = gameObjectIconProvider;
            _statsTextFactory = new PlacableStatsTextFactory(_towerDatabase);
            _selectedTowers = new List<PlacableEnum>();

            _buttons = new Dictionary<EnemyButton, Action>();

            foreach (var databaseItem in _towerDatabase.Items)
            {
                if (databaseItem.Key == PlacableEnum.MainBuilding)
                    continue;

                EnemyButton button = Instantiate(_enemyViewButtonPrefab, _contentTransform);

                Action OnClick = () =>
                {
                    _statsText.text = _statsTextFactory.GetText(databaseItem.Key);
                    
                    if (_selectedTowers.Contains(databaseItem.Key))
                        _selectedTowers.Remove(databaseItem.Key);
                    else
                        _selectedTowers.Add(databaseItem.Key);
                };

                button.clicked += OnClick;
                _buttons.Add(button, OnClick);

                button.UpdateTexture(_gameObjectIconProvider.Get(databaseItem.Key));
            }

            


        }
        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);
    }

}