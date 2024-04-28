using Builder;
using Levels.Logic;
using Towers;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor.Commands
{
    public class RemovePlacableAtCommand: ICommand
    {
        private readonly PlacablesContainer _placables;
        private readonly PlacableBuilder _placableBuilder;
        private readonly Vector2Int _position;
        private readonly Grid _grid;
        private PlacableEnum _destoyedId;
        public RemovePlacableAtCommand(Vector2Int position, PlacableBuilder placableBuilder, PlacablesContainer placablesContainer, Grid grid)
        {
            _position = position;
            _placableBuilder = placableBuilder;
            _placables = placablesContainer;
            _grid = grid;
        }
        public bool Execute()
        {
            if (_placables.CanBuildAt(_position))
                return false;

            _placableBuilder.placableDestroyed += OnPlacableDestroyed;
            
            _placables.ForceDestroyAt(_position);

            return true;
        }

        public void Undo()
        {
            _placableBuilder.SwitchCurrentId(_destoyedId);
            _placableBuilder.Build(_position, _grid);
        }

        private void OnPlacableDestroyed(PlacableEnum id, Vector2Int position)
        {
            if (_position != position)
                return;

            _placableBuilder.placableDestroyed -= OnPlacableDestroyed;

            _destoyedId = id;
        }
    }
}