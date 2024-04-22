using Common;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI
{
    public class ToolButtons : MonoBehaviour, IMenuParent
    {
        [SerializeField] private SelectorsToolBar _selectorsToolBar;
        [SerializeField] private DrawTypeToolBar _drawTypeToolBar;
        [SerializeField] private TilesToolBar _tilesToolBar;

        [SerializeField] private Button _selectorToolButton;
        [SerializeField] private Button _drawTypeToolButton;
        [SerializeField] private Button _tilesToolButton;

        private MenuParentsManager _menuParentsManager;

        public bool Active => gameObject.activeInHierarchy;

        public void Init()
        {
            _menuParentsManager = new MenuParentsManager();

            _menuParentsManager.Add(_selectorsToolBar);
            _menuParentsManager.Add(_drawTypeToolBar);
            _menuParentsManager.Add(_tilesToolBar);

            _menuParentsManager.HideAll();
        }

        private void OnEnable() 
        {
            _selectorToolButton.onClick.AddListener(OnSelectorsButtonPressed);
            _drawTypeToolButton.onClick.AddListener(OnDrawTypeToolButtonPressed);
            _tilesToolButton.onClick.AddListener(OnTilesToolButtonPressed);
        }

        private void OnDisable() 
        {
            _selectorToolButton.onClick.RemoveListener(OnSelectorsButtonPressed);
            _drawTypeToolButton.onClick.RemoveListener(OnDrawTypeToolButtonPressed);
            _tilesToolButton.onClick.RemoveListener(OnTilesToolButtonPressed);
        }

        public void Hide()
        {
            _menuParentsManager.HideAll();
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void OnSelectorsButtonPressed() => _menuParentsManager.Show(_selectorsToolBar);
        private void OnDrawTypeToolButtonPressed() => _menuParentsManager.Show(_drawTypeToolBar);
        private void OnTilesToolButtonPressed() => _menuParentsManager.Show(_tilesToolBar);
    }
}
