using BuffSystem;
using Components.ComponentsAnimations;
using Components.SimpleSpriteAnimator;
using Enemies.Buffs;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(ComponentsAnimator), typeof(SimpleSpriteAnimationComponent))]
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private SpriteAnimationData _idleAnimation;
        [SerializeField] private ParticleSystem _prefabParticles;

        private SpriteRenderer _spriteRenderer;
        private ComponentsAnimator _animator;
        private SimpleSpriteAnimationComponent _spriteAnimator;

        private ColorAnimation _colorAnimation;
        private ColorAnimation _freezeAnimation;
        private RandomizedPositionAnimation _positionAnimation;

        private ParticleSystem _particles;
        private void Awake() 
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<ComponentsAnimator>();
            _spriteAnimator = GetComponent<SimpleSpriteAnimationComponent>();

            SmoothAnimationValueUpdater colorUpdater = new SmoothAnimationValueUpdater(0,1f,100);
            _colorAnimation = new ColorAnimation(colorUpdater,new Color(1,0.2f,0.2f),_spriteRenderer);

            WiggleAnimationValueUpdater _positionUpdater = new WiggleAnimationValueUpdater(0,1f,100);
            _positionAnimation = new RandomizedPositionAnimation(_positionUpdater,transform,new Vector2(0.1f,0.1f), Vector2.zero);

            _freezeAnimation = new ColorAnimation(colorUpdater, Color.cyan, _spriteRenderer);

            _enemy.tookDamage+=OnEnemyTookDamage;
            _enemy.died += OnEnemyDied;
            _enemy.buffApplied += OnEnemyBuffApplied;
            _enemy.buffDispeled += OnEnemyBuffDispeled;

            _spriteAnimator.StartAnimation(_idleAnimation);
        }

        private void OnDestroy() 
        {
            _enemy.tookDamage-=OnEnemyTookDamage;
            _enemy.died -= OnEnemyDied;
            _enemy.buffApplied -= OnEnemyBuffApplied;
            _enemy.buffDispeled -= OnEnemyBuffDispeled;
        }

        private void OnEnemyTookDamage()
        {
            _animator.AddAnimation(_colorAnimation);
            _animator.AddAnimation(_positionAnimation);
        }

        private void OnEnemyDied(Enemy enemy)
        {
            enemy.died -= OnEnemyDied;
            _enemy.buffApplied -= OnEnemyBuffApplied;
            _enemy.buffDispeled -= OnEnemyBuffDispeled;
            //to do die animation
        }

        private void OnEnemyBuffApplied(IBuff<EnemyStats, BuffId> buff)
        {
            switch (buff.Id)
            {
                case BuffId.Frost:
                {
                    _animator.AddAnimation(_freezeAnimation);
                    
                    if (_particles == null)
                        _particles = Instantiate(_prefabParticles, transform);

                    break;
                }
                default: break;
            }
        }

        private void OnEnemyBuffDispeled(IBuff<EnemyStats, BuffId> buff)
        {
            switch (buff.Id)
            {
                case BuffId.Frost:
                {
                    _animator.StopAnimation(_freezeAnimation);

                    if (_particles != null)
                        Destroy(_particles.gameObject);

                    break;
                }
                default: break;
            }
        }
    }
}
