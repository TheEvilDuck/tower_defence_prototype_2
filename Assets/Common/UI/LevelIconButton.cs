using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class LevelIconButton : MonoBehaviour
    {
        [SerializeField] private Button _mapIcon;
        [SerializeField] private TextMeshProUGUI _mapName;

        public event Action<LevelIconButton> pressed;

        public string Name {get; private set;}

        private void OnEnable()  => _mapIcon.onClick.AddListener(OnIconPressed);

        private void OnDisable() => _mapIcon.onClick.RemoveListener(OnIconPressed);

        public void UpdateContent(string name, Texture2D newTexture)
        {
            Sprite sprite = Sprite.Create(
                newTexture, 
                new Rect(0, 0, newTexture.width, newTexture.height), 
                new Vector2(newTexture.width / 2, newTexture.height / 2));
            _mapIcon.image.sprite = sprite;
            _mapName.text = name;
            Name = name;
        }

        private void OnIconPressed() => pressed?.Invoke(this);
    }
}
