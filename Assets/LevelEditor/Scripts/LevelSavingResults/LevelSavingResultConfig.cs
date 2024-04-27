using UnityEngine;

namespace LevelEditor.LevelSaving
{
    [CreateAssetMenu(fileName = "Level saving result config", menuName = "Configs/Level editor/New level saving result config")]
    public class LevelSavingResultConfig: ScriptableObject
    {
        [field: SerializeField] public string message {get; private set;}
    }
}
