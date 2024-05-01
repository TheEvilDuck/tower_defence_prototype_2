using System;
using Towers;
using Towers.Configs;

namespace LevelEditor.UI
{
    public class PlacableStatsTextFactory
    {
        private readonly TowersDatabase _towersDatabase;
        private PlacableVisitor _placableVisitor;

        public PlacableStatsTextFactory(TowersDatabase towersDatabase)
        {
            _towersDatabase = towersDatabase;
            _placableVisitor = new PlacableVisitor(_towersDatabase);
        }

        public string GetText(PlacableEnum id)
        {
            _placableVisitor.UpdateText(id);
            return _placableVisitor.ResultText;
        }

        private class PlacableVisitor: IPlacableVisitor
        {
            private readonly TowersDatabase _towersDatabase;
            private PlacableConfig _currentConfig;
            public string ResultText {get; private set;}

            public PlacableVisitor(TowersDatabase towersDatabase)
            {
                _towersDatabase = towersDatabase;
            }

            public void UpdateText(PlacableEnum id)
            {
                ResultText = string.Empty;

                if (!_towersDatabase.TryGetValue(id, out _currentConfig))
                    throw new ArgumentException($"There is no config in towers database for id: {id}");

                ResultText = $"Cost: {_currentConfig.Cost}\nBuild time: {_currentConfig.BuildTime}, Can be destroyed: {_currentConfig.CanBeDestroyed}\n";

                Visit(_currentConfig.Prefab);
            }
            
            public void Visit(Placable placable)
            {
                Visit((dynamic)placable);
            }

            public void Visit(Tower tower)
            {
                if (_currentConfig is not TowerConfig config)
                    throw new ArgumentException($"Passed wrong config to get placable stats text, {_currentConfig.GetType()}");

                ResultText = ResultText + $"Damage: {config.Damage}\nAttack rate: {config.AttackRate}\nRange: {config.Range}";
            }

            public void Visit(MainBuilding mainBuilding)
            {
                if (_currentConfig is not MainBuildingConfig config)
                    throw new ArgumentException($"Passed wrong config to get placable stats text, {_currentConfig.GetType()}");

                ResultText = ResultText + $"Health: {config.MaxHealth}";
            }

            public void Visit(MoneyGiver moneyGiver)
            {
                if (_currentConfig is not MoneyGiverConfig config)
                    throw new ArgumentException($"Passed wrong config to get placable stats text, {_currentConfig.GetType()}");

                ResultText = ResultText + $"Money amount: {config.Money}\nMoney rate: {config.MoneyRate}";
            }

            public void Visit(SlowBomb slowBomb)
            {
                if (_currentConfig is not SlowBombConfig config)
                    throw new ArgumentException($"Passed wrong config to get placable stats text, {_currentConfig.GetType()}");

                ResultText = ResultText + $"Walk speed multiplier: {config.SlowMultiplier}\nSlow time: {config.SlowTime}\nDelay: {config.Delay}\nRange: {config.Range}";
            }

            public void Visit(Storage storage)
            {
                if (_currentConfig is not StorageConfig config)
                    throw new ArgumentException($"Passed wrong config to get placable stats text, {_currentConfig.GetType()}");

                ResultText = ResultText + $"Max money increase: {config.Money}";
            }
        }
    }

}