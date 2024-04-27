using GamePlay.Player;
using Towers.Configs;
using UnityEngine;

namespace Towers
{
    public class MoneyGiver : Placable
    {
        private float _moneyRate;
        private int _money;
        private PlayerStats _playerStats;
        private bool _inited;
        private bool _paused;
        private float _currentTimer;

        public void Init(MoneyGiverConfig config, PlayerStats playerStats)
        {
            _currentTimer = _moneyRate;
            _playerStats = playerStats;
            _moneyRate = config.MoneyRate;
            _money = config.Money;
            _inited = true;
        }

        private void Update() 
        {
            if (!_inited)
                return;

            if (_paused)
                return;

            if (_currentTimer <= 0)
            {
                _currentTimer = _moneyRate;
                _playerStats.Add(_money);
            }
            else
            {
                _currentTimer -= Time.deltaTime;
            }
        }
        public override void Pause() => _paused = true;

        public override void UnPause() => _paused = false;
    }

}