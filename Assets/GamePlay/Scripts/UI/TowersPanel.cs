using System;
using TMPro;
using Towers;
using UnityEngine;

namespace GamePlay.UI
{
    public class TowersPanel : MonoBehaviour
    {
        [SerializeField]private TowerButton _towerButtonPrefab;
        [SerializeField]private float _spacing = 5f;
        [SerializeField]private Transform _parent;

        public event Action<PlacableEnum> placableButtonPressed;

        public void Init(TowersDatabase towersDatabase, GameObjectIconProvider<PlacableEnum> iconsProvider)
        {
            foreach (var item in towersDatabase.Items)
            {
                TowerButton button = Instantiate(_towerButtonPrefab,_parent);
                button.onClick.AddListener(()=>{
                    placableButtonPressed?.Invoke(item.Key);
                });

                button.GetComponentInChildren<TextMeshProUGUI>().text = $"{item.Value.Name}({item.Value.Cost})";
                button.UpdateInfo($"{item.Value.Name}({item.Value.Cost})", iconsProvider.Get(item.Key));
            }
        }
    }
}
