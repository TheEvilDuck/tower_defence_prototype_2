using System;

namespace GamePlay
{
    public class PlayerStats
    {
        private int _money;

        public Action<int> moneyChanged;

        public int Money => _money;

        public PlayerStats(int money)
        {
            _money = money;
        }

        public void Add(int money)
        {
            if (!ValidateMoney(money))
                return;

            _money += money;
            moneyChanged?.Invoke(_money);
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