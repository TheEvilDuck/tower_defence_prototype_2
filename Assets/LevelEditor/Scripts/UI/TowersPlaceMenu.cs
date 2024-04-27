using System;
using System.Collections.Generic;
using Common;
using Common.Interfaces;
using LevelEditor.UI.EnemiesSelection;
using Levels.Logic;
using TMPro;
using Towers;
using UnityEngine;
using UnityEngine.UI;
using Grid = Levels.Logic.Grid;

namespace LevelEditor.UI
{
    public class TowersPlaceMenu : MonoBehaviour, IMenuParent
    {
        [SerializeField] private EnemyButton _enemyViewButtonPrefab;
        [SerializeField] private Transform _contentTransform;
        [SerializeField] private TowersDatabase _towerDatabase;
        [SerializeField] private TextMeshProUGUI _statsText;
        [SerializeField] private Button _selectedButton;
        [SerializeField] private Button _cancelButton;
        public event Action<PlacableEnum> selected;
        private Dictionary<EnemyButton, Action> _buttonAndClicks;
        private Dictionary<Vector2Int, PlacableEnum> _savedTowers;
        private PlacableEnum _selectedId;
        public bool Active => gameObject.activeInHierarchy;
        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);

        public void Init(GameObjectIconProvider<PlacableEnum> gameObjectIconProvider)
        {
            var statsTextFactory = new PlacableStatsTextFactory(_towerDatabase);
            _buttonAndClicks = new Dictionary<EnemyButton, Action>();
            _savedTowers = new Dictionary<Vector2Int, PlacableEnum>();

            foreach (var databaseItem in _towerDatabase.Items)
            {
                EnemyButton button = Instantiate(_enemyViewButtonPrefab, _contentTransform);

                button.UpdateTexture(gameObjectIconProvider.Get(databaseItem.Key));

                Action onClicked = () =>
                {
                    _statsText.text = statsTextFactory.GetText(databaseItem.Key);
                    _selectedId = databaseItem.Key;
                };

                button.clicked += onClicked;

                _buttonAndClicks.Add(button, onClicked);
            }

            _selectedButton.onClick.AddListener(OnSelectedPressed);
            _cancelButton.onClick.AddListener(OnCancelPressed);
        }

        private void OnDestroy() 
        {
            foreach (var buttonAndDelegate in _buttonAndClicks)
            {
                buttonAndDelegate.Key.clicked -= buttonAndDelegate.Value;
            }

            _selectedButton.onClick.RemoveListener(OnSelectedPressed);
            _cancelButton.onClick.RemoveListener(OnCancelPressed);
        }

        public void SavePlacedTower(PlacableEnum id, Vector2Int position)
        {
            _savedTowers.Add(position, id);
        }

        public void ClearData()
        {
            _savedTowers.Clear();
        }

        public PlacableData[] ConvertToPlacableDatas(Grid grid)
        {
            List<PlacableData> result = new List<PlacableData>();
            
            foreach (var positionAndId in _savedTowers)
            {
                PlacableData placableData = new PlacableData
                {
                    index = grid.ConvertVector2IntToIndex(positionAndId.Key),
                    placable = positionAndId.Value
                };

                result.Add(placableData);

            }

            return result.ToArray();
        }

        public void RemovePlacedTower(PlacableEnum id, Vector2Int position)
        {
            _savedTowers.Remove(position);
        }

        private void OnSelectedPressed()
        {
            selected?.Invoke(_selectedId);
            Hide();
        }

        private void OnCancelPressed()
        {
            Hide();
        }
    }
}
