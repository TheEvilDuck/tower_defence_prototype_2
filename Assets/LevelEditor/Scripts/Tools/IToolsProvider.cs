using System;

namespace LevelEditor.Tools
{
    public interface IToolsProvider
    {
        public event Action<Tool> toolChanged;
    }
}
