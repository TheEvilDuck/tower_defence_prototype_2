using System;
using Builder;
using GamePlay.UI;
using Services.PlayerInput;
using Towers;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace GamePlay
{
    public class BuilderMediator: IDisposable
    {
        private PlayerInput _playerInput;
        private PlacableBuilder _builder;
        private TowersPanel _towersPanel;
        private Grid _grid;

        public BuilderMediator(PlayerInput playerInput, PlacableBuilder builder, Grid grid,TowersPanel towersPanel)
        {
            _playerInput = playerInput;
            _builder = builder;
            _grid = grid;
            _towersPanel = towersPanel;

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
                _grid.DestroyAt(_grid.WorldPositionToGridPosition(position));
                _builder.DeleteInConstructionAt(_grid.WorldPositionToGridPosition(position));
            }
        }
        private void OnTowerChosenInTowersPanel(PlacableEnum id)
        {
            _builder.SwitchCurrentId(id);
        }

        private void OnMouseMoved(Vector2 position)
        {
            Vector2Int gridPos = _grid.WorldPositionToGridPosition(position);
            Vector2 worldPos = _grid.GridPositionToWorldPosition(gridPos);

            _builder.MovePreview(worldPos);
        }
    }
}
