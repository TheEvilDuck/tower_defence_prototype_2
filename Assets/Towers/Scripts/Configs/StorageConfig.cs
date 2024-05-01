using UnityEngine;

namespace Towers.Configs
{
    [CreateAssetMenu(fileName = "Storage config", menuName = "Configs/Placable configs/New storage config")]
    public class StorageConfig : PlacableConfig
    {
        [field: SerializeField, Min(0)] public int Money {get; private set;}
    }
}
