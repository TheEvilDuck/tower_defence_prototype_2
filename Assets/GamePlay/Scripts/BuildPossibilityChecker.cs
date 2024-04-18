using System;
using Builder;
using Towers;

namespace GamePlay
{
    public class BuildPossibilityChecker: IDisposable
    {
        private readonly PlayerStats _playerStats;
        private readonly PlacableBuilder _placableBuilder;
        private readonly TowersDatabase _placableDatabase;

        public BuildPossibilityChecker(PlayerStats playerStats, PlacableBuilder placableBuilder, TowersDatabase towersDatabase)
        {
            _playerStats = playerStats;
            _placableBuilder = placableBuilder;
            _placableDatabase = towersDatabase;

            _placableBuilder.checkCanBuild += CheckBuildPossibility;
        }

        public void Dispose()
        {
            _placableBuilder.checkCanBuild -= CheckBuildPossibility;
        }

        private bool CheckBuildPossibility(PlacableEnum placableId)
        {
            if (!_placableDatabase.TryGetValue(placableId, out var config))
                return false;

            return _playerStats.CanSpend(config.Cost);
        }
    }
}