using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesEditor : MonoBehaviour, IMenuParent
{
    public bool Active => gameObject.activeInHierarchy;

    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);
}
