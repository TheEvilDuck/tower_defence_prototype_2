using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = "Slow tower config", menuName = "Configs/Placable configs/New slow tower config")]
    public class SlowTowerConfig : TowerConfig
    {
        [field: SerializeField, Range(0, 1f)] public float SlowMulriplier {get; private set;}
    }
}
