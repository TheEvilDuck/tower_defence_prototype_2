using System;
using Builder;
using LevelEditor.Commands.Factory;
using LevelEditor.Selectors;
using LevelEditor.Tools;
using LevelEditor.UI;
using Services.PlayerInput;
using Towers;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor.Mediators
{
    public class EditorBuilderMediator: IDisposable
    {
        private readonly TowersPlaceMenu _towersPlaceMenu;
        private readonly PlacableBuilder _placableBuilder;
        private readonly Grid _grid;
        private readonly PlayerInput _playerInput;
        private readonly PlacablesContainer _placablesContainer;
        private readonly AddPlacableCommandFactory _addPlacableCommandFactory;
        private readonly Tool _placeTowersTool;
        private readonly Tool _deletingTowersTool;
        private readonly BrushSelector _brushSelector;
        private readonly DeleteSelector _deleteSelector;
        private readonly LevelEditor _levelEditor;

        public EditorBuilderMediator(
            TowersPlaceMenu towersPlaceMenu, 
            PlacableBuilder placableBuilder, 
            Grid grid, 
            PlayerInput playerInput, 
            PlacablesContainer placablesContainer,
            AddPlacableCommandFactory addPlacableCommandFactory,
            Tool placeTowersTool,
            Tool deletingTowersTool,
            BrushSelector brushSelector,
            DeleteSelector deleteSelector,
            LevelEditor levelEditor
            )
        {
            _towersPlaceMenu = towersPlaceMenu;
            _placableBuilder = placableBuilder;
            _grid = grid;
            _playerInput = playerInput;
            _placablesContainer = placablesContainer;
            _addPlacableCommandFactory = addPlacableCommandFactory;
            _placeTowersTool = placeTowersTool;
            _deleteSelector = deleteSelector;
            _deletingTowersTool = deletingTowersTool;
            _brushSelector = brushSelector;
            _levelEditor = levelEditor;

            _placableBuilder.placableBuilt += OnPlacableBuild;

            _playerInput.mouseRightClicked += OnMouseRightClicked;
            _playerInput.mousePositionChanged += OnMouseMove;
            _towersPlaceMenu.selected += OnTowerSelected;
            _placableBuilder.placableDestroyed += OnPlacableDestroyed;
        }

        public void Dispose()
        {
            _playerInput.mouseRightClicked -= OnMouseRightClicked;
            _playerInput.mousePositionChanged -= OnMouseMove;
            _towersPlaceMenu.selected -= OnTowerSelected;
            _placableBuilder.placableDestroyed -= OnPlacableDestroyed;
        }

        private void OnPlacableBuild(PlacableEnum id, Vector2Int cellPosition)
        {
            _placablesContainer.Pause();
            _towersPlaceMenu.SavePlacedTower(id, cellPosition);
        }

        private void OnPlacableDestroyed(PlacableEnum id, Vector2Int cellPosition)
        {
            _towersPlaceMenu.RemovePlacedTower(id, cellPosition);
        }

        private void OnMouseRightClicked(Vector2 position)
        {
            if (_placableBuilder.PreviewAble)
            {
                _placableBuilder.DisablePreview();
                _levelEditor.ChangeTool(_deletingTowersTool);
                _levelEditor.ChangeSelector(_deleteSelector); 
            }
        }

        private void OnMouseMove(Vector2 position)
        {
            Vector2Int gridPosition = _grid.WorldPositionToGridPosition(position);

            _placableBuilder.MovePreview(_grid.GridPositionToWorldPosition(gridPosition));
        }

        private void OnTowerSelected(PlacableEnum id)
        {
            _addPlacableCommandFactory.ChangePlacableId(id);
            _placableBuilder.SwitchCurrentId(id);
            _placableBuilder.EnablePreview();

            _levelEditor.ChangeTool(_placeTowersTool);
            _levelEditor.ChangeSelector(_brushSelector);
        }
    }
}