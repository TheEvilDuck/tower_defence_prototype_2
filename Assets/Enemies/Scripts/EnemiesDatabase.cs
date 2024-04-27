using Services;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "Enemies database", menuName = "Configs/Database/New enemies database")]
    public class EnemiesDatabase : EnumDataBase<EnemyEnum,EnemyConfig>
    {
        
    }

}