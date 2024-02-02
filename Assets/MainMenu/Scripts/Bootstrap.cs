using UnityEngine;

namespace MainMenu
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] MainMenuView _mainMenuView;

        private MainMenuMediator _mainMenuMediator;
        private SceneLoader _sceneLoader;

        private void Awake() 
        {
            _sceneLoader = new SceneLoader();
            _mainMenuMediator = new MainMenuMediator(_mainMenuView,_sceneLoader);
        }

        private void OnDestroy() 
        {
            _mainMenuMediator.Dispose();
        }
    }
}
