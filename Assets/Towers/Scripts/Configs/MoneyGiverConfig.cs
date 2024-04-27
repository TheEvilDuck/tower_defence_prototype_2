using UnityEngine;

namespace Towers.Configs
{
    [CreateAssetMenu(fileName = "Money giver config", menuName = "Configs/Placable configs/New money giver config")]
    public class MoneyGiverConfig : PlacableConfig
    {
        [field: SerializeField, Min(0)] public float MoneyRate {get; private set;}
        [field: SerializeField, Min(0)] public int Money {get; private set;}
    }
}