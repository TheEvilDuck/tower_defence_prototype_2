using System;
using UnityEngine;

namespace GamePlay.Player
{
    public class PlayerStats
    {
        private int _money;
        private int _maxMoney;

        public Action<int> moneyChanged;
        public Action<int> maxMoneyChanged;

        public int Money => _money;
        public int MaxMoney => _maxMoney;

        public PlayerStats(int money, int maxMoney)
        {
            _money = money;
            _maxMoney = maxMoney;
        }

        public void Add(int money)
        {
            if (!ValidateMoney(money))
                return;

            if (_money == _maxMoney)
                return;

            _money += money;

            _money = Mathf.Min(_money, _maxMoney);

            moneyChanged?.Invoke(_money);
        }

        public void IncreaseMaxMoney(int money)
        {
            if (!ValidateMoney(money))
                return;

            _maxMoney += money;
            maxMoneyChanged?.Invoke(_maxMoney);
        }

        public void DecreaseMaxMoney(int money)
        {
            if (!ValidateMoney(money))
                return;

            if (_maxMoney == 0)
                return;

            _maxMoney -= money;

            _maxMoney = Mathf.Max(0, _maxMoney);
            maxMoneyChanged?.Invoke(_maxMoney);

            if (_money > _maxMoney)
            {
                _money = _maxMoney;
                moneyChanged?.Invoke(_money);
            }
        }

        public bool Spend(int money)
        {
            if (!ValidateMoney(money))
                return false;

            if (!CanSpend(money))
                return false;

            _money -= money;
            moneyChanged?.Invoke(_money);

            return true;
        }

        public bool CanSpend(int money)
        {
            if (!ValidateMoney(money))
                return false;

            return _money >= money;
        }

        private bool ValidateMoney(int money)
        {
            if (money < 0)
            {
                throw new ArgumentException($"Passed delta money must be greater than 0 or equals, you passed {money}");
            }

            return true;
        }
    }
}