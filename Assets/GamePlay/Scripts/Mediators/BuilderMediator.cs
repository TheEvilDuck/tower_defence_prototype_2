using System;
using Builder;
using GamePlay.UI;
using Services.PlayerInput;
using Towers;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace GamePlay.Mediators
{
    public class BuilderMediator: IDisposable
    {
        private PlayerInput _playerInput;
        private PlacableBuilder _builder;
        private TowersPanel _towersPanel;
        private Grid _grid;
        private PlacablesContainer _placables;

        public BuilderMediator(PlayerInput playerInput, PlacableBuilder builder, Grid grid,TowersPanel towersPanel, PlacablesContainer placablesContainer)
        {
            _playerInput = playerInput;
            _builder = builder;
            _grid = grid;
            _towersPanel = towersPanel;
            _placables = placablesContainer;

            _playerInput.mouseLeftClicked+=OnMouseLeftClicked;
            _playerInput.mouseRightClicked+=OnMouseRightClicked;
            _towersPanel.placableButtonPressed+=OnTowerChosenInTowersPanel;
            _playerInput.mousePositionChanged += OnMouseMoved;
        }
        public void Dispose()
        {
            _playerInput.mouseLeftClicked-=OnMouseLeftClicked;
            _playerInput.mouseRightClicked-=OnMouseRightClicked;
            _towersPanel.placableButtonPressed-=OnTowerChosenInTowersPanel;
            _playerInput.mousePositionChanged -= OnMouseMoved;
        }
        private void OnMouseLeftClicked(Vector2 position)
        {
            if (_builder.PreviewAble)
                _builder.Build(position,_grid);
        }
        private void OnMouseRightClicked(Vector2 position)
        {
            if (_builder.PreviewAble)
            {
                _builder.DisablePreview();
            }
            else
            {
                _placables.DestroyAt(_grid.WorldPositionToGridPosition(position));
                _builder.DeleteInConstructionAt(_grid.WorldPositionToGridPosition(position));
            }
        }
        private void OnTowerChosenInTowersPanel(PlacableEnum id)
        {
            _builder.SwitchCurrentId(id);
            _builder.EnablePreview();
        }

        private void OnMouseMoved(Vector2 position)
        {
            Vector2Int gridPos = _grid.WorldPositionToGridPosition(position);
            Vector2 worldPos = _grid.GridPositionToWorldPosition(gridPos);

            _builder.MovePreview(worldPos);
        }
    }
}
