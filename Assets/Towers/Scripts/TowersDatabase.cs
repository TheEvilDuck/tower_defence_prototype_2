using System;
using Services;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Towers
{
    public enum PlacableEnum
    {
        MainBuilding = 0
    };

    [CreateAssetMenu(fileName = "Towers database", menuName = "Configs/New towers database")]
    public class TowersDatabase : EnumDataBase<PlacableEnum, PlacableConfig>
    {
        
    }
}
