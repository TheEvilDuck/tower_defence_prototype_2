using Common;
using Common.Interfaces;
using Towers;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI
{
    public class TowersMenu : MonoBehaviour, IMenuParent
    {
        [SerializeField] private Button _towersSettingsButton;
        [SerializeField] private Button _towersPlaceButton;
        [SerializeField] private TowersSettingsMenu _settingsMenu;
        [SerializeField] private TowersPlaceMenu _placeMenu;

        private MenuParentsManager _menuParentsManager;

        public bool Active => gameObject.activeInHierarchy;

        public void Init(GameObjectIconProvider<PlacableEnum> _icons)
        {
            _settingsMenu.Init(_icons);
            _placeMenu.Init(_icons);

            _menuParentsManager = new MenuParentsManager();
            _menuParentsManager.Add(_settingsMenu);
            _menuParentsManager.Add(_placeMenu);

            _menuParentsManager.HideAll();
        }

        private void OnEnable() 
        {
            _towersSettingsButton.onClick.AddListener(OnTowersSettingsButtonPressed);
            _towersPlaceButton.onClick.AddListener(OnTowersPlaceButtonPressed);
        }
        private void OnDisable() 
        {
            _towersSettingsButton.onClick.RemoveListener(OnTowersSettingsButtonPressed);
            _towersPlaceButton.onClick.RemoveListener(OnTowersPlaceButtonPressed);
        }
        public void Hide()
        {
            _menuParentsManager.HideAll();
            gameObject.SetActive(false);
        }

        public void Show() => gameObject.SetActive(true);

        private void OnTowersSettingsButtonPressed()
        {
            _menuParentsManager.Show(_settingsMenu);
        }

        private void OnTowersPlaceButtonPressed()
        {
            _menuParentsManager.Show(_placeMenu);
        }
    }
}
