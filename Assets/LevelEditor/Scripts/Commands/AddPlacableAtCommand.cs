using Builder;
using Towers;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor.Commands
{
    public class AddPlacableAtCommand : ICommand
    {
        private readonly PlacablesContainer _placables;
        private readonly PlacableBuilder _placableBuilder;
        private readonly PlacableEnum _id;
        private readonly Vector2Int _position;
        private readonly Grid _grid;
        public AddPlacableAtCommand(PlacableEnum id, Vector2Int position, PlacableBuilder placableBuilder, PlacablesContainer placablesContainer, Grid grid)
        {
            _id = id;
            _position = position;
            _placableBuilder = placableBuilder;
            _placables = placablesContainer;
            _grid = grid;
        }
        public bool Execute()
        {
            if (!_placables.CanBuildAt(_position))
                return false;

            _placableBuilder.SwitchCurrentId(_id);
            
            if (!_placableBuilder.Build(_position, _grid))
                return false;

            return true;
        }

        public void Undo()
        {
            _placables.ForceDestroyAt(_position);
        }
    }
}

