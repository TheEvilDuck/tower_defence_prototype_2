using System;
using LevelEditor.Selectors;
using Levels.Tiles;

namespace LevelEditor.UI
{
    public class ToolBarMediator : IDisposable
    {
        private readonly ISelectorsProvider _selectorsProvider;
        private readonly ITilesProvider _tilesProvider;
        private readonly IToolsProvider _toolsProvider;
        private readonly LevelEditor _levelEditor;
        private readonly DrawCommandsFactory _drawCommandsFactory;
        public ToolBarMediator(ISelectorsProvider selectorsProvider, LevelEditor levelEditor, DrawCommandsFactory drawCommandsFactory, ITilesProvider tilesProvider, IToolsProvider toolsProvider)
        {
            _selectorsProvider = selectorsProvider;
            _levelEditor = levelEditor;
            _drawCommandsFactory = drawCommandsFactory;
            _tilesProvider = tilesProvider;
            _toolsProvider = toolsProvider;

            _selectorsProvider.selectorChanged += OnSelectorChanged;
            _tilesProvider.tileChanged += OnTilesChanged;
            _toolsProvider.toolChanged += OnToolChanged;
        }

        public void Dispose()
        {
            _selectorsProvider.selectorChanged -= OnSelectorChanged;
            _tilesProvider.tileChanged -= OnTilesChanged;
            _toolsProvider.toolChanged -= OnToolChanged;
        }

        private void OnSelectorChanged(ISelector currentSelector)
        {
            _levelEditor.ChangeSelector(currentSelector);
        }

        private void OnTilesChanged(TileType tileType)
        {
            _drawCommandsFactory.ChangeTileType(tileType);
        }

        private void OnToolChanged(Tool tool)
        {
            _levelEditor.ChangeTool(tool);
        }
    }
}
