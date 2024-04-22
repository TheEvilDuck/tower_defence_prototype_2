using System;

namespace LevelEditor
{
    public interface IToolsProvider
    {
        public event Action<Tool> toolChanged;
    }
}
