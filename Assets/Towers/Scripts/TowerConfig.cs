using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = "Tower config", menuName = "Configs/Placable configs/New tower config")]
    public class TowerConfig : PlacableConfig
    {
        [field:SerializeField, Range(0.01f,30f)]public float AttackRate {get; private set; } = 1f;
        [field:SerializeField, Range(0,1000)]public int Damage {get; private set; } = 10;
        [field:SerializeField,Range(0,1000)]public float Range {get; private set; } = 10f;
    }
}
