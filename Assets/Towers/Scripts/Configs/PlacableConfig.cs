using UnityEngine;

namespace Towers.Configs
{
    [CreateAssetMenu(fileName = "Placable config", menuName = "Configs/Placable configs/New placable config")]
    public class PlacableConfig : ScriptableObject
    {
        [field:SerializeField] public string Name {get; private set;}
        [field:SerializeField, Range (0, 1000)] public int Cost {get; private set;}
        [field:SerializeField, Range (0, 100f)] public float BuildTime {get; private set;}
        [field:SerializeField] public bool CanBeDestroyed = true;
        [field:SerializeField] public Placable Prefab {get; private set;}
    }
}
