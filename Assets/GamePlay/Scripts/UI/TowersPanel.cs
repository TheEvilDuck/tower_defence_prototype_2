using System;
using System.Collections.Generic;
using Common;
using TMPro;
using Towers;
using UnityEngine;

namespace GamePlay.UI
{
    public class TowersPanel : MonoBehaviour
    {
        [SerializeField]private TowerButton _towerButtonPrefab;
        [SerializeField]private Transform _parent;

        public event Action<PlacableEnum> placableButtonPressed;

        private Dictionary<TowerButton, PlacableEnum> _buttons;

        public void Init(TowersDatabase towersDatabase, GameObjectIconProvider<PlacableEnum> iconsProvider, PlacableEnum[] availableTowers)
        {
            _buttons = new Dictionary<TowerButton, PlacableEnum>();

            foreach (var tower in availableTowers)
            {
                TowerButton button = Instantiate(_towerButtonPrefab,_parent);
                button.onClick.AddListener(()=>{
                    placableButtonPressed?.Invoke(tower);
                });

                if (!towersDatabase.TryGetValue(tower, out var config))
                    throw new ArgumentException($"Can't init towers panel with tower of type {tower}");

                button.GetComponentInChildren<TextMeshProUGUI>().text = $"{config.Name}({config.Cost})";
                button.UpdateInfo($"{config.Name}({config.Cost})", iconsProvider.Get(tower));

                _buttons.Add(button, tower);
            }
        }

        public void HideAllExceptMainBuilding()
        {
            foreach (var buttonAndId in _buttons)
            {
                if (buttonAndId.Value != PlacableEnum.MainBuilding)
                    buttonAndId.Key.gameObject.SetActive(false);
                else
                    buttonAndId.Key.gameObject.SetActive(true);
            }
        }

        public void ShowAllExceptMainBuilding()
        {
            foreach (var buttonAndId in _buttons)
            {
                if (buttonAndId.Value == PlacableEnum.MainBuilding)
                    buttonAndId.Key.gameObject.SetActive(false);
                else
                    buttonAndId.Key.gameObject.SetActive(true);
            }
        }
    }
}
