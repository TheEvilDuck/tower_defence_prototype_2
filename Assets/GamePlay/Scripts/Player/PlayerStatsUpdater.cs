using System;
using Builder;
using Towers;
using UnityEngine;

namespace GamePlay.Player
{
    public class PlayerStatsUpdater : IDisposable
    {
        private const float CANCEL_MONEY_RETURN = 1f;
        private const float DESTROY_MONEY_RETURN = 0.5f;
        private readonly PlayerStats _playerStats;
        private readonly PlacableBuilder _placableBuilder;
        private readonly TowersDatabase _towersDatabase;

        public PlayerStatsUpdater(PlayerStats playerStats, PlacableBuilder placableBuilder, TowersDatabase towersDatabase)
        {
            _playerStats = playerStats;
            _placableBuilder = placableBuilder;
            _towersDatabase = towersDatabase;

            _placableBuilder.placableBuildCanceled += OnBuildCanceled;
            _placableBuilder.placableBuildStarted += OnBuildStarted;
            _placableBuilder.placableDestroyed += OnBuildDestroyed;


        }
        public void Dispose()
        {
            _placableBuilder.placableBuildCanceled -= OnBuildCanceled;
            _placableBuilder.placableBuildStarted -= OnBuildStarted;
            _placableBuilder.placableDestroyed -= OnBuildDestroyed;
        }

        private void OnBuildCanceled(PlacableEnum placableId)
        {
            if (!_towersDatabase.TryGetValue(placableId, out var config))
                throw new ArgumentException($"Can't find placable config with id {placableId}");

            _playerStats.Add((int)(config.Cost * CANCEL_MONEY_RETURN));
        }

        private void OnBuildDestroyed(PlacableEnum placableId, Vector2Int cellPosition)
        {
            if (!_towersDatabase.TryGetValue(placableId, out var config))
                throw new ArgumentException($"Can't find placable config with id {placableId}");

            _playerStats.Add((int)(config.Cost * DESTROY_MONEY_RETURN));
        }

        private void OnBuildStarted(PlacableEnum placableId)
        {
            if (!_towersDatabase.TryGetValue(placableId, out var config))
                throw new ArgumentException($"Can't find placable config with id {placableId}");

            _playerStats.Spend(config.Cost);
        }
    }
}