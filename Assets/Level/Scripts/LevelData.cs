using System;
using UnityEngine;

namespace Levels.Logic
{
    [Serializable]
    public struct LevelData
    {
        
        [SerializeField] public int startMoney;
        [SerializeField] public GridData gridData;
    }
}
