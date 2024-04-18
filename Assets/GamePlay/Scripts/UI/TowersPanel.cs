using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Towers;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
    public class TowersPanel : MonoBehaviour
    {
        [SerializeField]private Button _towerButtonPrefab;
        [SerializeField]private float _spacing = 5f;
        [SerializeField]private Transform _parent;

        public event Action<PlacableEnum> placableButtonPressed;

        public void Init(TowersDatabase towersDatabase)
        {
            int countTotal = towersDatabase.Items.Count();

            foreach (var item in towersDatabase.Items)
            {
                Button button = Instantiate(_towerButtonPrefab,_parent);
                button.onClick.AddListener(()=>{
                    placableButtonPressed?.Invoke(item.Key);
                });

                button.GetComponentInChildren<TextMeshProUGUI>().text = $"{item.Value.Name}({item.Value.Cost})";
            }
        }
    }
}
