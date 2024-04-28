using System;
using System.Collections.Generic;
using Builder;
using Towers;
using UnityEngine;

namespace LevelEditor.Commands.Factory
{
    public class RemovePlacableCommandFactory : CommandFactory
    {
        private readonly PlacableBuilder _placableBuilder;
        private readonly PlacablesContainer _placablesContainer;
        public RemovePlacableCommandFactory(Levels.Logic.Grid grid, PlacableBuilder placableBuilder, PlacablesContainer placablesContainer) : base(grid)
        {
            _placableBuilder = placableBuilder;
            _placablesContainer = placablesContainer;
        }

        public override ICommand CreateCommandAtCell(Vector2Int position)
        {
            return new RemovePlacableAtCommand(position, _placableBuilder, _placablesContainer, _grid);
        }
    }
}