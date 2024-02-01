using UnityEngine;
using System;

namespace Towers
{
    [Serializable]
    public class AvailablePlacables
    {
        [SerializeField] public PlacableEnum[] placableIds;
    }
}
