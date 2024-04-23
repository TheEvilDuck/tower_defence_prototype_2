using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;
using UnityEngine;

namespace LevelEditor.UI
{
    public class TowersPlaceMenu : MonoBehaviour, IMenuParent
    {
        public bool Active => gameObject.activeInHierarchy;
        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);
    }
}
