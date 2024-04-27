using Common.Interfaces;
using Levels.Logic;
using UnityEngine;

namespace LevelEditor.UI
{
    public class LoadMenu : MonoBehaviour,IMenuParent
    {
        [field: SerializeField] public Transform ParentToIcons {get; private set;}
        public bool Active => gameObject.activeInHierarchy;

        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);

        public void CreateIconButtons(LevelLoader levelLoader)
        {
            
        }
    }
}
