using System;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI.EnemiesSelection
{
    public class EnemyButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private RawImage _rawImage;

        public event Action clicked;

        public void UpdateTexture(Texture2D texture) => _rawImage.texture = texture;

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
