using System;

namespace GamePlay
{
    public class PlayerStats
    {
        private int _money;

        public PlayerStats(int money)
        {
            _money = money;
        }

        public void Add(int money)
        {
            if (money <= 0)
                throw new ArgumentException();
        }
    }
}