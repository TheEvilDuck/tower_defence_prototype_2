using Builder;
using Towers;
using UnityEngine;

namespace LevelEditor.Commands.Factory
{
    public class AddPlacableCommandFactory : CommandFactory
    {
        private readonly PlacableBuilder _placableBuilder;
        private readonly PlacablesContainer _placablesContainer;
        private PlacableEnum _id;
        public AddPlacableCommandFactory(Levels.Logic.Grid grid, PlacableBuilder placableBuilder, PlacablesContainer placablesContainer) : base(grid)
        {
            _placableBuilder = placableBuilder;
            _placablesContainer = placablesContainer;
        }


        public void ChangePlacableId(PlacableEnum id)
        {
            _id = id;
        }

        public override ICommand CreateCommandAtCell(Vector2Int position)
        {
            return new AddPlacableAtCommand(_id, position, _placableBuilder,_placablesContainer, _grid);
        }
    }
}