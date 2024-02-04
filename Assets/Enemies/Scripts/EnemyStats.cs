namespace Enemies
{
    public class EnemyStats: IEnemyStatsProvider
    {
        private int _health;
        private int _maxHealth;
        private float _walkSpeed;

        public float WalkSpeed => _walkSpeed;
        public int MaxHealth => _maxHealth;
        public int Health => _health;

        public EnemyStats(int maxHealth, float walkSpeed)
        {
            _health = _maxHealth = maxHealth;
            _walkSpeed = walkSpeed;
        }

        public void ModifyHealth(int amount)
        {
            if (amount == 0)
                return;

            _health+=amount;

            if (_health<0)
                _health = 0;

            if (_health>_maxHealth)
                _health = _maxHealth;
        }

        public EnemyStats GetStats() => this;
    }
}
