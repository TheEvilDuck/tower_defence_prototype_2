using Services;
using UnityEngine;
using Towers.Configs;

namespace Towers
{
    [CreateAssetMenu(fileName = "Towers database", menuName = "Configs/New towers database")]
    public class TowersDatabase : EnumDataBase<PlacableEnum, PlacableConfig>
    {
        [field: SerializeField] public GameObject InConstructionPrefab {get; private set;}
    }
}
