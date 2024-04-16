using System;
using System.Collections;
using System.Collections.Generic;
using Builder;
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
        }
        public void Dispose()
        {
            _playerInput.mouseLeftClicked-=OnMouseLeftClicked;
            _playerInput.mouseRightClicked-=OnMouseRightClicked;
            _towersPanel.placableButtonPressed-=OnTowerChosenInTowersPanel;
        }
        private void OnMouseLeftClicked(Vector2 position)
        {
            _builder.Build(position,_grid);
        }
        private void OnMouseRightClicked(Vector2 position)
        {
            _grid.DestroyAt(_grid.WorldPositionToGridPosition(position));
            _builder.DeleteInConstructionAt(_grid.WorldPositionToGridPosition(position));
        }
        private void OnTowerChosenInTowersPanel(PlacableEnum id)
        {
            _builder.SwitchCurrentId(id);
        }
    }
}
