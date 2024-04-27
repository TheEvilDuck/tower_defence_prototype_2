using UnityEngine;

namespace Towers.Configs
{
    [CreateAssetMenu(fileName = "Main building config", menuName = "Configs/Placable configs/New main building config")]
    public class MainBuildingConfig : PlacableConfig
    {
        [field:SerializeField, Min(0)] public int MaxHealth {get; private set;}
    }
}