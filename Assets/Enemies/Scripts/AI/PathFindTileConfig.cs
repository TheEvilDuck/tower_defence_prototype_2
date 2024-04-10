using UnityEngine;

namespace Enemies.AI
{
    [CreateAssetMenu(fileName = "Pathfind tile config", menuName = "Enemies/Configs/New pathfind tile config")]
    public class PathFindTileConfig: ScriptableObject
    {
        [field: SerializeField, Range(0.001f,100f)] public float WeightMultiplier {get; private set;}
        [field: SerializeField] public bool Walkable {get; private set;}
    }
}
