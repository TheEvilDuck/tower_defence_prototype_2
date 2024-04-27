using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.Tiles
{
    [CreateAssetMenu(fileName = "TileConfig", menuName = "Configs/New tile config")]
    public class TileConfig : ScriptableObject
    {
        [field: SerializeField] public TileBase Tile {get; private set;}
    }
}
