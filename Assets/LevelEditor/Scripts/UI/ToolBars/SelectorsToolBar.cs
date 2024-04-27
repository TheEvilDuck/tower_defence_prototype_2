using System;
using LevelEditor.Selectors;
using UnityEngine;
using UnityEngine.UI;
using Common.Interfaces;

namespace LevelEditor.UI.Toolbars
{
    public class SelectorsToolBar : MonoBehaviour, ISelectorsProvider, IMenuParent
    {
        [SerializeField] private Button _brushButton;
        [SerializeField] private Button _fillButton;
        [SerializeField] private Button _lineButton;
        [SerializeField] private GameObject _buttonsParent;
        private BrushSelector _brushSelector;
        private FillSelector _fillSelector;
        private LineSelector _lineSelector;
        private ISelector _currentSelector;

        public event Action<ISelector> selectorChanged;

        public ISelector CurrentSelector 
        {
            get => _currentSelector;
            private set
            {
                _currentSelector = value;
                selectorChanged?.Invoke(_currentSelector);
            }
        }

        public bool Active => _buttonsParent.activeInHierarchy;

        public void Init(BrushSelector brushSelector, FillSelector fillSelector, LineSelector lineSelector)
        {
            _brushSelector = brushSelector;
            _fillSelector = fillSelector;
            _lineSelector = lineSelector;
        }

        private void OnEnable() 
        {
            _brushButton.onClick.AddListener(OnBrushButtonPressed);
            _fillButton.onClick.AddListener(OnFillButtonPressed);
            _lineButton.onClick.AddListener(OnLineButtonPressed);
        }

        private void OnDisable() 
        {
            _brushButton.onClick.RemoveListener(OnBrushButtonPressed);
            _fillButton.onClick.RemoveListener(OnFillButtonPressed);
            _lineButton.onClick.RemoveListener(OnLineButtonPressed);
        }

        public void Show() => _buttonsParent.SetActive(true);

        public void Hide() => _buttonsParent.SetActive(false);

        private void OnBrushButtonPressed() => CurrentSelector = _brushSelector;
        private void OnFillButtonPressed() => CurrentSelector = _fillSelector;
        private void OnLineButtonPressed() => CurrentSelector = _lineSelector;
    }
}
