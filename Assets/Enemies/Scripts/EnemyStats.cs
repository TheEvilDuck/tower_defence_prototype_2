namespace Enemies
{
    public class EnemyStats: IEnemyStatsProvider
    {
        public float WalkSpeed {get; private set;}
        public int MaxHealth {get; private set;}
        public int Health {get; private set;}
        public float Range {get; private set;}
        public float AttackRate {get; private set;}
        public int Damage {get; private set;}

        public EnemyStats(int maxHealth, float walkSpeed, float range, float attackRate, int damage)
        {
            Health = MaxHealth = maxHealth;
            WalkSpeed = walkSpeed;
            Range = range;
            AttackRate = attackRate;
            Damage = damage;
        }

        public void ModifyHealth(int amount)
        {
            if (amount == 0)
                return;

            Health+=amount;

            if (Health<0)
                Health = 0;

            if (Health>MaxHealth)
                Health = MaxHealth;
        }

        public EnemyStats GetStats() => this;
    }
}
