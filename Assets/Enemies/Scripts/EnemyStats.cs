using BuffSystem;

namespace Enemies
{
    public struct EnemyStats: IStats
    {
        public float walkSpeed;
        public int maxHealth;
        public int health;
        public float range;
        public float attackRate;
        public int damage;

        public EnemyStats(EnemyConfig enemyConfig)
        {
            walkSpeed = enemyConfig.WalkSpeed;
            health = maxHealth = enemyConfig.MaxHealth;
            range = enemyConfig.Range;
            attackRate = enemyConfig.AttackRate;
            damage = enemyConfig.Damage;
        }

        public void ModifyHealth(int amount)
        {
            if (amount == 0)
                return;

            health+=amount;

            if (health<0)
                health = 0;

            if (health>maxHealth)
                health = maxHealth;
        }
    }
}
