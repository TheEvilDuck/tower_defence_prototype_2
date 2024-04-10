using Services;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.Tiles
{
    [CreateAssetMenu(fileName = "Tile database", menuName = "Configs/Database/New tiles database")]
    public class TileDatabase: EnumDataBase<TileType,TileConfig>
    {
        [field: SerializeField] public TileBase RoadTile {get; private set;}
    }
}
