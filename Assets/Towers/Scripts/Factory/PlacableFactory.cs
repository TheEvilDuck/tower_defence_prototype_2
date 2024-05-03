using System;
using GamePlay.EnemiesSpawning;
using GamePlay.Player;
using Towers.Configs;
using UnityEngine;

namespace Towers
{
    public class PlacableFactory
    {
        TowersDatabase _towersDatabase;
        EnemySpawner _enemySpawner;
        PlayerStats _playerStats;

        public PlacableFactory(TowersDatabase towersDatabase, EnemySpawner enemySpawner, PlayerStats playerStats)
        {
            _towersDatabase = towersDatabase;
            _enemySpawner = enemySpawner;
            _playerStats = playerStats;
        }

        public Placable Get(PlacableEnum placableId)
        {
            if (!_towersDatabase.TryGetValue(placableId, out PlacableConfig config))
                throw new ArgumentException($"There is no config in towers database for {placableId}", "placableId");

            Placable tower = UnityEngine.Object.Instantiate(config.Prefab);
            InitPlacable(tower, config);

            return tower;
        }

        public GameObject GetInConstruction()
        {
            return UnityEngine.Object.Instantiate(_towersDatabase.InConstructionPrefab);
        }

        public PlacableConfig GetConfig(PlacableEnum placableId)
        {
            if (!_towersDatabase.TryGetValue(placableId, out PlacableConfig config))
                throw new ArgumentException($"There is no config in towers database for {placableId}", "placableId");

            return config;
        }

        private void InitPlacable(Placable placable, PlacableConfig config)
        {
            PlacableInitilizer placableInitilizer = new PlacableInitilizer(config,_enemySpawner,_playerStats);
            placableInitilizer.Visit(placable);
        }

        private class PlacableInitilizer : IPlacableVisitor
        {
            private PlacableConfig _config;
            private EnemySpawner _enemySpawner;
            private PlayerStats _playerStats;

            public PlacableInitilizer(PlacableConfig config, EnemySpawner enemySpawner, PlayerStats playerStats)
            {
                _config = config;
                _enemySpawner = enemySpawner;
                _playerStats = playerStats;
            }
            public void Visit(Placable placable)
            {
                placable.Init(_config.CanBeDestroyed);
                Visit((dynamic)placable);
            }

            public void Visit(Tower tower)
            {
                if (_config is not TowerConfig towerConfig)
                    throw new ArgumentException($"Passed wrong config to init placable, {_config.GetType()}");

                tower.Init(_enemySpawner,towerConfig);
            }

            public void Visit(MainBuilding mainBuilding)
            {
                if (_config is not MainBuildingConfig mainBuildingConfig)
                    throw new ArgumentException($"Passed wrong config to init placable, {_config.GetType()}");

                mainBuilding.Init(mainBuildingConfig.MaxHealth);
            }

            public void Visit(MoneyGiver moneyGiver)
            {
                if (_config is not MoneyGiverConfig moneyGiverConfig)
                    throw new ArgumentException($"Passed wrong config to init placable, {_config.GetType()}");

                moneyGiver.Init(moneyGiverConfig, _playerStats);
            }

            public void Visit(SlowBomb slowBomb)
            {
                if (_config is not SlowBombConfig slowBombConfig)
                    throw new ArgumentException($"Passed wrong config to init placable, {_config.GetType()}");

                slowBomb.Init(slowBombConfig, _enemySpawner);
            }

            public void Visit(Storage storage)
            {
                if (_config is not StorageConfig storageConfig)
                    throw new ArgumentException($"Passed wrong config to init placable, {_config.GetType()}");

                storage.Init(storageConfig, _playerStats);
            }

            public void Visit(Bomb bomb)
            {
                if (_config is not BombConfig bombConfig)
                    throw new ArgumentException($"Passed wrong config to init placable, {_config.GetType()}");

                bomb.Init(bombConfig, _enemySpawner);
            }
        }
    }
}
