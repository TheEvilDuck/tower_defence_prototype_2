using UnityEngine;

namespace LevelEditor
{
    [CreateAssetMenu(fileName = "Level editor config", menuName = "Configs/New level editor config")]
    public class LevelEditorConfig : ScriptableObject
    {
        [field: SerializeField]public KeyCode[] UndoKeyCodes {get; private set;}
        [field: SerializeField]public KeyCode FillKeyCode {get; private set;}
        [field: SerializeField]public KeyCode DrawKeyCode {get; private set;}
        [field: SerializeField]public KeyCode[] SaveKeyCodes {get; private set;}
    }
}
