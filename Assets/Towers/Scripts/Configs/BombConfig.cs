using UnityEngine;

namespace Towers.Configs
{
    [CreateAssetMenu(fileName = "Bomb config", menuName = "Configs/Placable configs/New bomb config")]
    public class BombConfig : PlacableConfig
    {
        [field: SerializeField, Min(0)] public float Range {get; private set;}
        [field: SerializeField, Min(0)] public float Delay {get; private set;}
        [field: SerializeField, Min(0)] public int Damage {get; private set;}
    }
}
