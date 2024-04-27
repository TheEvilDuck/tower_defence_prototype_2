using System;
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

        public void Init(TowersDatabase towersDatabase, GameObjectIconProvider<PlacableEnum> iconsProvider, PlacableEnum[] availableTowers)
        {
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
            }
        }
    }
}
