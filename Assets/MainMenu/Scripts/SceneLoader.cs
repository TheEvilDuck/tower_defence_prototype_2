using UnityEngine.SceneManagement;

public class SceneLoader
{
    private const int MAIN_MENU_SCENE = 0;
    private const int LEVEL_EDITOR_SCENE = 1;

    public void LoadLevelEditor()
    {
        SceneManager.LoadScene(LEVEL_EDITOR_SCENE);
    }
}
