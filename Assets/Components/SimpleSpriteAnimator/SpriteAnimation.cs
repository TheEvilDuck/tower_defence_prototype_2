using System;
using UnityEngine;

namespace Components.SimpleSpriteAnimator
{
    public class SpriteAnimation
    {
        private readonly Sprite[] _sprites;
        private readonly bool _loop = false;
        private readonly float _secondsPerFrame = 1;
        private float _timer = 0;
        private int _currentFrameId = 0;

        public event Action<SpriteAnimation> animationEnd;

        public SpriteAnimation(Sprite[] sprites, bool loop, float secondsPerFrame)
        {
            _sprites = sprites;
            _loop = loop;
            _secondsPerFrame = secondsPerFrame;
        }
        public void Tick(SpriteRenderer spriteRenderer)
        {
            if (_sprites.Length == 0)
                return;

            if (_timer>=_secondsPerFrame)
            {
                _timer = 0;

                spriteRenderer.sprite = _sprites[_currentFrameId];

                _currentFrameId++;

                if (_currentFrameId>=_sprites.Length)
                    if (_loop)
                        _currentFrameId = 0;
                    else
                        StopAnimation();
            }

            _timer+=Time.deltaTime;
        }

        public void StopAnimation()
        {
            _currentFrameId = 0;
            _timer = 0;
            animationEnd?.Invoke(this);
        }

    }
}