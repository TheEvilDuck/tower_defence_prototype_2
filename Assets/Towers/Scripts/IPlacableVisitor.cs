using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Towers
{
    public interface IPlacableVisitor
    {
        public void Visit(Placable placable);
        public void Visit(Tower tower);
        public void Visit(MainBuilding mainBuilding);
    }
}
