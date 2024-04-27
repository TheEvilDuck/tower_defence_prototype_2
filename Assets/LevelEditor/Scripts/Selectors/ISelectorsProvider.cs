using System;

namespace LevelEditor.Selectors
{
    public interface ISelectorsProvider
    {
        public event Action<ISelector> selectorChanged;
        public ISelector CurrentSelector {get;}
    }
}