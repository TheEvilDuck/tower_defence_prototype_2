using System;
using System.Collections.Generic;
using System.Linq;
using Levels.Tiles;
using UnityEngine;

namespace LevelEditor.UI
{
    [CreateAssetMenu(menuName = "Level editor/New tile icons factory", fileName = "Tile icons factory")]
    public class TileIconButtonsFactory: ScriptableObject
    {
        [SerializeField] private TileIconButton _buttonPrefab;
        [SerializeField] private List<TileIconData> _icons;

        public TileIconButton Get(TileType tileType)
        {
            TileIconData tileIconData = _icons.First((x) => x.Tile == tileType);

            if (tileIconData == null)
                throw new ArgumentException($"Tile icons buttons factory doesn't contain icon of {tileType}!");

            TileIconButton tileIconButton = Instantiate(_buttonPrefab);
            tileIconButton.Init(tileIconData.Icon);

            return tileIconButton;
        }

        [Serializable]
        private class TileIconData
        {
            [field:SerializeField] public TileType Tile {get; private set;}
            [field:SerializeField] public Sprite Icon {get; private set;}
        }
    }
}