using System;
using LevelEditor.Selectors;

namespace LevelEditor
{
    public interface ISelectorsProvider
    {
        public event Action<ISelector> selectorChanged;
        public ISelector CurrentSelector {get;}
    }
}