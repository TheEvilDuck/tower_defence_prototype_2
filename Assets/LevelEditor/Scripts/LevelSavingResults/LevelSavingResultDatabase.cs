using Services;
using UnityEngine;

namespace LevelEditor.LevelSaving
{
    [CreateAssetMenu(fileName = "Level saving results database", menuName = "Configs/Database/New level saving results database")]
    public class LevelSavingResultDatabase : EnumDataBase<ResultType, LevelSavingResultConfig>
    {
        
    }
}
