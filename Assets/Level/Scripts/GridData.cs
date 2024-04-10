using System;
using Levels.Tiles;
using UnityEngine;

namespace Levels.Logic
{
    [Serializable]
    public struct GridData
    {
        [SerializeField] public int gridSize;
        [SerializeField] public CellSavedData[] cells;
        [SerializeField] public int[] roadIndexes;
    }

    [Serializable]
    public struct CellSavedData
    {
        [SerializeField] public int tileType;
        [SerializeField] public int index;
    }
}
