using System.Collections;
using System.Collections.Generic;
using Levels.Logic;
using UnityEngine;

public class LoadMenu : MonoBehaviour,IMenuParent
{
    
    public bool Active => gameObject.activeInHierarchy;

    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);

    public void CreateIconButtons(LevelLoader levelLoader)
    {
        string[] levelNames = levelLoader.GetAllMapsNames();

        foreach (string levelName in levelNames)
        {

        }
    }
}
