using System.Collections.Generic;
using Common.Interfaces;
using Towers;

namespace Levels.Logic
{
    public interface IPlacableListHandler: IPausable
    {
        public IEnumerable<Placable> Placables {get;}
    }
}