using UnityEngine;

public class SettingsMenu : MonoBehaviour, IMenuParent
{
    public bool Active => gameObject.activeInHierarchy;
    public void Hide() => gameObject.SetActive(false);

    public void Show()=> gameObject.SetActive(true);
}
