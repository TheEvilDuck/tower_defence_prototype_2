using System.Collections.Generic;
using Common.Interfaces;
using Towers;

namespace Builder
{
    public interface IPlacableListHandler: IPausable
    {
        public IEnumerable<Placable> Placables {get;}
    }
}