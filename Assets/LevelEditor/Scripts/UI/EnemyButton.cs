using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI
{
    public class EnemyButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _buttonText;

        public event Action clicked;

        public void UpdateText(string enemyName) => _buttonText.text = enemyName;

        private void OnEnable() 
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable() 
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked() => clicked?.Invoke();
    }
}
