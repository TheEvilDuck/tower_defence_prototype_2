using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;

namespace Enemies
{
    public enum EnemyEnum
    {
        Gray = 0,
        Big = 1
    }
    [CreateAssetMenu(fileName = "Enemies database", menuName = "Configs/Database/New enemies database")]
    public class EnemiesDatabase : EnumDataBase<EnemyEnum,EnemyConfig>
    {
        
    }

}