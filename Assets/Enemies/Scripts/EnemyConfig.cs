using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "Enemy config", menuName = "Enemies/Configs/New enemy config")]
    public class EnemyConfig : ScriptableObject
    {
        [field:SerializeField, Range(1,1000)]public int MaxHealth {get; private set;} = 100;
        [field:SerializeField, Range (0.1f, 50f)]public float WalkSpeed {get; private set;} = 1f;
    }
}
