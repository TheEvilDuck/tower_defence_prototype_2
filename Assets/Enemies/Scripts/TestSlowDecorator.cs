namespace Enemies
{
    public class TestSlowDecorator : EnemyStatsProvider
    {
        private float _walkSpeedMultiplier;

        public TestSlowDecorator(float walkSpeedMultiplier)
        {
            _walkSpeedMultiplier = walkSpeedMultiplier;
        }

        public override EnemyStats GetStats()
        {
            float walkSpeed = _wrappedEnemyStatsProvider.GetStats().WalkSpeed*_walkSpeedMultiplier;
            return new EnemyStats(_wrappedEnemyStatsProvider.GetStats().MaxHealth, walkSpeed);
        }
    }
}
