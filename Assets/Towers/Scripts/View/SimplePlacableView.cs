using Components.SimpleSpriteAnimator;
using UnityEngine;

namespace Towers.View
{
    [RequireComponent(typeof(SimpleSpriteAnimationComponent), typeof(SpriteRenderer))]
    public class SimplePlacableView : MonoBehaviour
    {
        [SerializeField] private SpriteAnimationData _animation;
        private SpriteRenderer _spriteRenderer;
        private SimpleSpriteAnimationComponent _animator;

        private void Awake() 
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<SimpleSpriteAnimationComponent>();
            _animator.StartAnimation(_animation);
        }

    }
}
