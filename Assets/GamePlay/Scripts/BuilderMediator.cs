using System;
using System.Collections;
using System.Collections.Generic;
using Builder;
using Services.PlayerInput;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace GamePlay
{
    public class BuilderMediator: IDisposable
    {
        private PlayerInput _playerInput;
        private PlacableBuilder _builder;
        private Grid _grid;

        public BuilderMediator(PlayerInput playerInput, PlacableBuilder builder, Grid grid)
        {
            _playerInput = playerInput;
            _builder = builder;
            _grid = grid;

            _playerInput.mouseLeftClicked+=OnMouseLeftClicked;
        }

        private void OnMouseLeftClicked(Vector2 position)
        {
            Debug.Log("A");
            _builder.Build(position,_grid);
        }

        public void Dispose()
        {
            _playerInput.mouseLeftClicked-=OnMouseLeftClicked;
        }
    }
}
