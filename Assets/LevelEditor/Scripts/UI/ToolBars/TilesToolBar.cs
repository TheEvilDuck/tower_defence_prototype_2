using System;
using LevelEditor.Selectors;
using UnityEngine;
using UnityEngine.UI;
using LevelEditor;
using Levels.Tiles;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Common.Interfaces;

namespace LevelEditor.UI
{
    public class TilesToolBar : MonoBehaviour, ITilesProvider, IMenuParent
    {
        [SerializeField] private TileDatabase _tileDatabase;
        [SerializeField] private TileIconButtonsFactory _tileIconButtonsFactory;
        [SerializeField] private Transform _buttonsParent;
        
        private Dictionary<TileIconButton, Action> _onClickDelegates;

        public event Action<TileType> tileChanged;

        public bool Active => _buttonsParent.gameObject.activeInHierarchy;

        public void Init()
        {
            _onClickDelegates = new Dictionary<TileIconButton, Action>();

            foreach (var item in _tileDatabase.Items)
            {
                TileIconButton tileIconButton = _tileIconButtonsFactory.Get(item.Key);
                tileIconButton.transform.SetParent(_buttonsParent);

                Action onClick = () => tileChanged?.Invoke(item.Key);
                tileIconButton.clicked += onClick;

                _onClickDelegates.Add(tileIconButton, onClick);
            }
        }

        private void OnDestroy() 
        {
            foreach (var keyValuePair in _onClickDelegates)
            {
                keyValuePair.Key.clicked -= keyValuePair.Value;
            }
        }

        public void Show() => _buttonsParent.gameObject.SetActive(true);

        public void Hide() => _buttonsParent.gameObject.SetActive(false);
    }
}