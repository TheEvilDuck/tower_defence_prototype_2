using UnityEngine;

namespace Towers.Configs
{
    [CreateAssetMenu(fileName = "Slow tower config", menuName = "Configs/Placable configs/New slow tower config")]
    public class SlowTowerConfig : TowerConfig
    {
        [field: SerializeField, Range(0, 1f)] public float SlowMulriplier {get; private set;}
        [field: SerializeField, Min(0)] public float SlowTime {get; private set;}
    }
}
