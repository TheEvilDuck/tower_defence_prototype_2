using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LevelEditor.UI.Towers
{
    public class TowerSelectionButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Color _notSelectedColor;
        [SerializeField] private Button _button;
        [SerializeField] private RawImage _rawImage;

        public event Action clicked;
        public event Action selected;

        private Color _selectedColor;

        public bool Selected {get; private set;} = true;

        public void UpdateTexture(Texture2D texture) => _rawImage.texture = texture;

        private void Awake() {
            _selectedColor = _rawImage.color;
            Selected = true;
        }

        private void OnEnable() 
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable() 
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked() => clicked?.Invoke();

        public void ForceDeselect()
        {
            Selected = false;
            _rawImage.color = _notSelectedColor;
        }

        public void ForceSelect()
        {
            Selected = true;
            _rawImage.color = _selectedColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Selected = !Selected;

                if (Selected)
                    _rawImage.color = _selectedColor;
                else
                    _rawImage.color = _notSelectedColor;
                    
                selected?.Invoke();
            }
        }
    }
}
