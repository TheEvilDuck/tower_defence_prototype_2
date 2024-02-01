using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = "Placable config", menuName = "Configs/New placable config")]
    public class PlacableConfig : ScriptableObject
    {
        [field:SerializeField] public string Name {get; private set;}
        [field:SerializeField, Range (0, 1000)] public int Cost {get; private set;}
        [field:SerializeField, Range (0, 100f)] public float BuildTime {get; private set;}
        [field:SerializeField] public bool CanBeDestroyed = true;
        [field:SerializeField] public Placable Prefab {get; private set;}
    }
}
