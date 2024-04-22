using System;
using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI
{
    public class DrawTypeToolBar : MonoBehaviour, IToolsProvider, IMenuParent
    {
        [SerializeField] private Button _drawToolButton;
        [SerializeField] private Button _eraseToolButton;
        [SerializeField] private GameObject _buttonsParent;

        private Tool _drawTool;
        private Tool _eraseTool;

        public event Action<Tool> toolChanged;

        public bool Active => _buttonsParent.activeInHierarchy;

        public void Init(Tool drawTool, Tool eraseTool)
        {
            _drawTool = drawTool;
            _eraseTool = eraseTool;
        }

        private void OnEnable() 
        {
            _drawToolButton.onClick.AddListener(OnDrawToolButtonPressed);
            _eraseToolButton.onClick.AddListener(OnEraseToolButtonPressed);
        }

        private void OnDisable() 
        {
            _drawToolButton.onClick.RemoveListener(OnDrawToolButtonPressed);
            _eraseToolButton.onClick.RemoveListener(OnEraseToolButtonPressed);
        }

        public void Show() => _buttonsParent.SetActive(true);

        public void Hide() => _buttonsParent.SetActive(false);

        private void OnDrawToolButtonPressed() => toolChanged?.Invoke(_drawTool);
        private void OnEraseToolButtonPressed() => toolChanged?.Invoke(_eraseTool);
    }
}
