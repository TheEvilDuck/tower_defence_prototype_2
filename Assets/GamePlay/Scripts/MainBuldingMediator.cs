using System;
using Builder;
using GamePlay.UI;
using Towers;
using UnityEngine;

namespace GamePlay
{
    public class MainBuldingMediator : IDisposable
    {
        private MainBuilding _mainBuilding;
        private PlacableBuilder _builder;

        private MainBuildingHealth _mainBuildingHealthUI;

        public MainBuldingMediator(PlacableBuilder placableBuilder, MainBuildingHealth mainBuildingHealth)
        {
            _builder = placableBuilder;
            _mainBuildingHealthUI = mainBuildingHealth;

            _builder.mainBuildingBuilt += OnMainBuildingBuilt;

            _mainBuildingHealthUI.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _builder.mainBuildingBuilt -= OnMainBuildingBuilt;
            
            if (_mainBuilding != null)
                OnMainBuildingDestroyed(_mainBuilding);
        }

        private void OnMainBuildingBuilt(Vector2Int position)
        {
            if (_builder.MainBuilding is not MainBuilding mainBuilding)
                throw new Exception("Somehow you created main building of not main building class");

            _builder.mainBuildingBuilt -= OnMainBuildingBuilt;
            _mainBuildingHealthUI.gameObject.SetActive(true);


            _mainBuilding = mainBuilding;
            _mainBuilding.destroyed += OnMainBuildingDestroyed;
            _mainBuilding.healthChanged += OnhealthChanged;

            OnhealthChanged(_mainBuilding.Health);
        }

        private void OnMainBuildingDestroyed(Placable placable)
        {
            _mainBuilding.healthChanged -= OnhealthChanged;
            _mainBuilding.destroyed -= OnMainBuildingDestroyed;
        }

        private void OnhealthChanged(int health)
        {
            _mainBuildingHealthUI.UpdateHealthText(health);
        }
    }
}