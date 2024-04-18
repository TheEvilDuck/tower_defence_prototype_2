using System;
using System.Collections.Generic;
using Levels.Logic;
using UnityEngine;

namespace Common.UI
{
    public class LevelIconsLoader
    {
        public Action<string> mapIconPressed;

        private LevelLoader _levelLoader;
        private Transform _parent;
        private LevelIconButton _prefab;
        private List<LevelIconButton>_currentIcons;

        public LevelIconsLoader(LevelLoader levelLoader, Transform parent, LevelIconButton prefab)
        {
            _currentIcons = new List<LevelIconButton>();

            _levelLoader = levelLoader;
            _parent = parent;
            _prefab = prefab;

            Load();
        }

        public void Load()
        {
            string[] names = _levelLoader.GetAllMapsNames();

            foreach (string name in names)
            {
                if (_levelLoader.TryLoadLevelIcon(name, out Texture2D iconTexture))
                {
                    LevelIconButton icon = UnityEngine.Object.Instantiate(_prefab, _parent);
                    icon.UpdateContent(name, iconTexture);
                    _currentIcons.Add(icon);

                    icon.pressed+=OnIconPressed;
                }
            }
        }

        public void Delete()
        {
            for (int i = _currentIcons.Count-1; i>0; i--)
            {
                if (_currentIcons[i]!=null)
                {
                    _currentIcons[i].pressed-=OnIconPressed;
                    UnityEngine.Object.Destroy(_currentIcons[i]);
                    _currentIcons.RemoveAt(i);
                }
            }
        }

        public void Reload()
        {
            Delete();
            Load();
        }

        private void OnIconPressed(LevelIconButton icon) => mapIconPressed?.Invoke(icon.Name);
    }
}
