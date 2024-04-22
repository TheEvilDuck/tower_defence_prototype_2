using Components.SimpleSpriteAnimator;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "Enemy config", menuName = "Enemies/Configs/New enemy config")]
    public class EnemyConfig : ScriptableObject
    {
        [field:SerializeField]public string Name {get; private set;} = "Enemy";
        [field:SerializeField, Range(1,1000)]public int MaxHealth {get; private set;} = 100;
        [field:SerializeField, Range(1,1000)]public int Damage {get; private set;} = 5;
        [field:SerializeField, Range (0.1f, 50f)]public float WalkSpeed {get; private set;} = 1f;
        [field:SerializeField, Range (0.1f, 50f)]public float Range {get; private set;} = 1f;
        [field:SerializeField, Range (0.1f, 50f)]public float AttackRate {get; private set;} = 1f;

        [field:SerializeField]public Enemy Prefab {get; private set;}
    }
}
