using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    [CreateAssetMenu(fileName = "Level editor config", menuName = "Configs/New level editor config")]
    public class LevelEditorConfig : ScriptableObject
    {
        [field: SerializeField]public KeyCode[] UndoKeyCodes {get; private set;}
    }
}
