using Levels.Logic;
using Services;
using UnityEngine;

namespace Enemies.AI
{
    [CreateAssetMenu(fileName = "Pathfind tile database", menuName = "Configs/Database/New pathfind tile database")]
    public class PathFindMultipliersDatabase: EnumDataBase<TileType,PathFindTileConfig>
    {
        [field: SerializeField] public PathFindTileConfig RoadTileConfig {get; private set;}
    }
}


