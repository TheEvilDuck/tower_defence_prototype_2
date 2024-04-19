using BuffSystem;

namespace Enemies.Buffs
{
    public class WalkSpeedBuff : IBuff<EnemyStats>
    {
        private readonly float _multiplier;

        public WalkSpeedBuff(float multiplier)
        {
            _multiplier = multiplier;
        }
        public EnemyStats Apply(EnemyStats baseStats)
        {
            baseStats.walkSpeed *= _multiplier;
            return baseStats;
        }
    }
}