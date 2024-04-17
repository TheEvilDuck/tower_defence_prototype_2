using Common;
using Common.UI;
using LevelEditor;
using Levels.Logic;
using UnityEngine;

namespace MainMenu
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] MainMenuView _mainMenuView;
        [SerializeField] LevelSelectorView _levelSelectorView;
        [SerializeField] LevelIconButton _levelIconsPrefab;

        private MainMenuMediator _mainMenuMediator;
        private SceneLoader _sceneLoader;
        private LevelIconsLoader _levelIconsLoader;
        private LevelLoader _levelLoader;
        private LevelIconsMediator _levelIconsMediator;
        private MenuParentsManager _menuParentsManager;

        private void Awake() 
        {
            _sceneLoader = new SceneLoader();

            _menuParentsManager = new MenuParentsManager();
            _menuParentsManager.Add(_mainMenuView);
            _menuParentsManager.Add(_levelSelectorView);
            _menuParentsManager.HideAll();
            
            _levelLoader = new LevelLoader();
            _levelIconsLoader = new LevelIconsLoader(_levelLoader, _mainMenuView.LevelIconsParent, _levelIconsPrefab);
            _levelIconsMediator = new LevelIconsMediator(_levelIconsLoader, _sceneLoader);
            _mainMenuMediator = new MainMenuMediator(_mainMenuView,_sceneLoader, _menuParentsManager, _levelSelectorView);

            _menuParentsManager.Show(_mainMenuView);
            
        }

        private void OnDestroy() 
        {
            _mainMenuMediator.Dispose();
            _levelIconsMediator.Dispose();
        }
    }
}
