using System;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI.Toolbars
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public class TileIconButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;

        public event Action clicked;

        private void OnEnable() 
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable() 
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        public void Init(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        private void OnClicked() => clicked?.Invoke();
    }

}