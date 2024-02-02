using UnityEngine.SceneManagement;

public class SceneLoader
{
    private const int MAIN_MENU_SCENE = 0;
    private const int LEVEL_EDITOR_SCENE = 1;
    private const int GAMEPLAY_SCENE = 2;

    public void LoadLevelEditor() => SceneManager.LoadScene(LEVEL_EDITOR_SCENE);
    public void LoadGameplay() => SceneManager.LoadScene(GAMEPLAY_SCENE);
    public void LoadMainMenu() => SceneManager.LoadScene(MAIN_MENU_SCENE);
}
