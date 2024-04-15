using System.Collections.Generic;
using Towers;

namespace Levels.Logic
{
    public interface IPlacableListHandler
    {
        public IEnumerable<Placable> Placables {get;}
    }
}