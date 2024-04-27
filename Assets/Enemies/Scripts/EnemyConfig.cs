using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "Enemy config", menuName = "Enemies/Configs/New enemy config")]
    public class EnemyConfig : ScriptableObject
    {
        [field:SerializeField]public string Name {get; private set;} = "Enemy";
        [field:SerializeField, Min(1)]public int MaxHealth {get; private set;} = 100;
        [field:SerializeField, Min(0)]public int Damage {get; private set;} = 5;
        [field:SerializeField, Min(0)]public float WalkSpeed {get; private set;} = 1f;
        [field:SerializeField, Min(0)]public float Range {get; private set;} = 1f;
        [field:SerializeField, Min(0)]public float AttackRate {get; private set;} = 1f;

        [field:SerializeField]public Enemy Prefab {get; private set;}
    }
}
