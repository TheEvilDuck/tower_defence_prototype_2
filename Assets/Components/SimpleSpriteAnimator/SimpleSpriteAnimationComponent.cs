using System;
using UnityEngine;

namespace Components.SimpleSpriteAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SimpleSpriteAnimationComponent : MonoBehaviour
    {
        [SerializeField] private bool _hideOnStop = true;
        public event Action animationEnd;

        private SpriteRenderer _spriteRenderer;
        private SpriteAnimation _currentAnimation;
        private bool _playing = false;

        private void Awake() 
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (!_playing)
                return;

            _currentAnimation?.Tick(_spriteRenderer);
        }

        public void StartAnimation(SpriteAnimationData spriteAnimationData)
        {
            if (_currentAnimation != null)
            {
                _currentAnimation.StopAnimation();
            }

            _playing = true;
            _currentAnimation = spriteAnimationData.StartNewAnimation();
            _currentAnimation.animationEnd += OnAnimationEnd;

            _spriteRenderer.enabled = true;
        }

        public void StopAnimation()
        {
            _currentAnimation?.StopAnimation();

            _currentAnimation = null;
            _playing = false;
        }

        public void PauseAnimation() => _playing = false;

        public void ResumeAnimation() => _playing = true;

        private void OnAnimationEnd(SpriteAnimation spriteAnimation)
        {
            spriteAnimation.animationEnd -= OnAnimationEnd;

            if (_hideOnStop)
                _spriteRenderer.enabled = false;
        }
    }

}

