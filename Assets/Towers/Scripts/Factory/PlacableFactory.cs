using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using UnityEngine;

namespace Towers
{
    public class PlacableFactory
    {
        TowersDatabase _towersDatabase;
        EnemySpawner _enemySpawner;

        public PlacableFactory(TowersDatabase towersDatabase, EnemySpawner enemySpawner)
        {
            _towersDatabase = towersDatabase;
            _enemySpawner = enemySpawner;
        }

        public Placable Get(PlacableEnum placableId)
        {
            if (!_towersDatabase.TryGetValue(placableId, out PlacableConfig config))
                throw new ArgumentException($"There is no config in towers database for {placableId}", "placableId");

            Placable tower = UnityEngine.Object.Instantiate(config.Prefab);
            InitPlacable(tower, config);

            return tower;
        }

        public PlacableConfig GetConfig(PlacableEnum placableId)
        {
            if (!_towersDatabase.TryGetValue(placableId, out PlacableConfig config))
                throw new ArgumentException($"There is no config in towers database for {placableId}", "placableId");

            return config;
        }

        private void InitPlacable(Placable placable, PlacableConfig config)
        {
            PlacableInitilizer placableInitilizer = new PlacableInitilizer(config,_enemySpawner);
            placableInitilizer.Visit(placable);
        }

        private class PlacableInitilizer : IPlacableVisitor
        {
            private PlacableConfig _config;
            private EnemySpawner _enemySpawner;

            public PlacableInitilizer(PlacableConfig config, EnemySpawner enemySpawner)
            {
                _config = config;
                _enemySpawner = enemySpawner;
            }
            public void Visit(Placable placable)
            {
                placable.Init(_config.CanBeDestroyed);
                Visit((dynamic)placable);
            }

            public void Visit(Tower tower)
            {
                if (_config is not TowerConfig towerConfig)
                    throw new ArgumentException("Passed wrong config to init placable", "_config");

                tower.Init(_enemySpawner,towerConfig);
            }

            public void Visit(MainBuilding mainBuilding)
            {
                if (_config is not MainBuildingConfig mainBuildingConfig)
                    throw new ArgumentException("Passed wrong config to init placable", "_config");

                mainBuilding.Init(mainBuildingConfig.MaxHealth);
            }
        }
    }
}
