using Components;
using Components.SimpleSpriteAnimator;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(ComponentsAnimator), typeof(SimpleSpriteAnimationComponent))]
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private SpriteAnimationData _idleAnimation;

        private SpriteRenderer _spriteRenderer;
        private ComponentsAnimator _animator;
        private SimpleSpriteAnimationComponent _spriteAnimator;

        private ColorAnimation _colorAnimation;
        private RandomizedPositionAnimation _positionAnimation;

        private void Awake() 
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<ComponentsAnimator>();
            _spriteAnimator = GetComponent<SimpleSpriteAnimationComponent>();

            SmoothAnimationValueUpdater colorUpdater = new SmoothAnimationValueUpdater(0,1f,100);
            _colorAnimation = new ColorAnimation(colorUpdater,new Color(1,0.2f,0.2f),_spriteRenderer);

            WiggleAnimationValueUpdater _positionUpdater = new WiggleAnimationValueUpdater(0,1f,100);
            _positionAnimation = new RandomizedPositionAnimation(_positionUpdater,transform,new Vector2(0.1f,0.1f), Vector2.zero);

            _enemy.tookDamage+=OnEnemyTookDamage;
            _enemy.died += OnEnemyDied;

            _spriteAnimator.StartAnimation(_idleAnimation);
        }

        private void OnDestroy() 
        {
            _enemy.tookDamage-=OnEnemyTookDamage;
            _enemy.died -= OnEnemyDied;
        }

        private void OnEnemyTookDamage()
        {
            _animator.AddAnimation(_colorAnimation);
            _animator.AddAnimation(_positionAnimation);
        }

        private void OnEnemyDied(Enemy enemy)
        {
            enemy.died -= OnEnemyDied;
            //to do die animation
        }
    }
}
