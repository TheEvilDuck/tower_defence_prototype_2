using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SimpleSpriteAnimationComponent : MonoBehaviour
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _loop = false;
        [SerializeField] private bool _playOnAwake = false;
        [SerializeField, Range(0.01f, 10f)] private float _secondsPerFrame = 1;

        private SpriteRenderer _spriteRenderer;
        private float _timer = 0;
        private int _currentFrameId = 0;
        private bool _playing = false;

        private void Awake() 
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_playOnAwake)
                StartAnimation();
        }

        void Update()
        {
            if (_sprites.Length == 0)
                return;

            if (!_playing)
                return;

            if (_timer>=_secondsPerFrame)
            {
                _timer = 0;

                _spriteRenderer.sprite = _sprites[_currentFrameId];

                _currentFrameId++;

                if (_currentFrameId>=_sprites.Length)
                    if (_loop)
                        _currentFrameId = 0;
                    else
                        StopAnimation();
            }

            _timer+=Time.deltaTime;
        }

        public void StartAnimation()
        {
            _currentFrameId = 0;
            _timer = 0;
            _playing = true;
        }

        public void StopAnimation()
        {
            _currentFrameId = 0;
            _timer = 0;
            _playing = false;
        }

        public void PauseAnimation() => _playing = false;

        public void ResumeAnimation() => _playing = true;
    }

}

