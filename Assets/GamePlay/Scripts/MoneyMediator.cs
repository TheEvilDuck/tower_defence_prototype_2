using System;
using GamePlay.UI;

namespace GamePlay
{
    public class MoneyMediator : IDisposable
    {
        private readonly PlayerStats _playerStats;
        private readonly MoneyView _moneyView;

        public MoneyMediator(PlayerStats playerStats, MoneyView moneyView)
        {
            _playerStats = playerStats;
            _moneyView = moneyView;

            _playerStats.moneyChanged += OnMoneyChanged;

            OnMoneyChanged(_playerStats.Money);
        }
        public void Dispose()
        {
            _playerStats.moneyChanged -= OnMoneyChanged;
        }

        private void OnMoneyChanged(int money) => _moneyView.UpdateText(money);
    }
}