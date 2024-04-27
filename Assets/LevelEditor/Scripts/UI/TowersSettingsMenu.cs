using System;
using System.Collections.Generic;
using Common;
using Common.Interfaces;
using LevelEditor.UI.Towers;
using TMPro;
using Towers;
using UnityEngine;

namespace LevelEditor.UI
{
    public class TowersSettingsMenu : MonoBehaviour, IMenuParent
    {
        [SerializeField] private TowerSelectionButton _towerViewButtonPrefab;
        [SerializeField] private Transform _contentTransform;
        [SerializeField] private TowersDatabase _towerDatabase;
        [SerializeField] private TextMeshProUGUI _statsText;

        public bool Active => gameObject.activeInHierarchy;
        private Dictionary<TowerSelectionButton, Action> _buttonsClicks;
        private Dictionary<TowerSelectionButton, Action> _buttonsSelections;
        private Dictionary<PlacableEnum, TowerSelectionButton> _buttons;
        private GameObjectIconProvider<PlacableEnum> _gameObjectIconProvider;
        private PlacableStatsTextFactory _statsTextFactory;
        private List<PlacableEnum> _selectedTowers;

        public IEnumerable<PlacableEnum> SelectedTowers => _selectedTowers;

        public void Init(GameObjectIconProvider<PlacableEnum> gameObjectIconProvider)
        {
            _gameObjectIconProvider = gameObjectIconProvider;
            _statsTextFactory = new PlacableStatsTextFactory(_towerDatabase);
            _selectedTowers = new List<PlacableEnum>();
            _buttonsSelections = new Dictionary<TowerSelectionButton, Action>();
            _buttons = new Dictionary<PlacableEnum, TowerSelectionButton>();

            _buttonsClicks = new Dictionary<TowerSelectionButton, Action>();

            foreach (var databaseItem in _towerDatabase.Items)
            {
                if (databaseItem.Key == PlacableEnum.MainBuilding)
                    continue;

                TowerSelectionButton button = Instantiate(_towerViewButtonPrefab, _contentTransform);

                Action onClick = () =>
                {
                    _statsText.text = _statsTextFactory.GetText(databaseItem.Key);
                };

                Action onSelect = () => 
                {
                    Debug.Log($"Selected: {button.Selected}");
                    SelectedTower(databaseItem.Key, button.Selected);

                };

                button.clicked += onClick;
                button.selected += onSelect;
                _buttonsClicks.Add(button, onClick);
                _buttonsSelections.Add(button, onSelect);
                _buttons.Add(databaseItem.Key, button);

                button.UpdateTexture(_gameObjectIconProvider.Get(databaseItem.Key));

                _selectedTowers.Add(databaseItem.Key);
            }

            foreach (var id in _selectedTowers)
                Debug.Log(id);
        }

        private void OnDestroy() 
        {
            foreach (var buttonAndDelegate in _buttonsClicks)
                buttonAndDelegate.Key.clicked -= buttonAndDelegate.Value;

            foreach (var buttonAndDelegate in _buttonsSelections)
                buttonAndDelegate.Key.selected -= buttonAndDelegate.Value;
        }
        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);

        public void PreloadWith(PlacableEnum[] availableTowers)
        {
            _selectedTowers.Clear();

            foreach(var idAndButton in _buttons)
                idAndButton.Value.ForceDeselect();

            foreach(var id in availableTowers)
            {
                if (!_buttons.ContainsKey(id))
                    continue;

                _selectedTowers.Add(id);
                _buttons[id].ForceSelect();
            }
        }

        public void ClearData() => PreloadWith(_towerDatabase.GetAllIds());

        private void SelectedTower(PlacableEnum id, bool selected)
        {
            Debug.Log($"SESESEKECRED: {selected}");
            Debug.Log(id);
            Debug.Log("JEFIJOIFJOW");

            if (selected)
            {
                if (!_selectedTowers.Contains(id))
                    _selectedTowers.Add(id);
            }
            else
            {
                Debug.Log($"Removing {id}");
                _selectedTowers.Remove(id);
            }

            foreach (var ids in _selectedTowers)
                Debug.Log(ids);
        }
    }

}