using UnityEngine;

namespace Towers.Configs
{
    [CreateAssetMenu(fileName = "Slow bomb config", menuName = "Configs/Placable configs/New slow bomb config")]
    public class SlowBombConfig : BombConfig
    {
        [field: SerializeField, Range(0, 1f)] public float SlowMultiplier {get; private set;}
        [field: SerializeField, Min(0)] public float SlowTime {get; private set;}
    }
}
