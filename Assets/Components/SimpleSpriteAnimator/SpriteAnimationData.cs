using System;
using UnityEngine;

namespace Components.SimpleSpriteAnimator
{
    [CreateAssetMenu(fileName = "Sprite animation", menuName = "Sprite animator/New sprite animation")]
    public class SpriteAnimationData: ScriptableObject
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _loop = false;
        [SerializeField, Range(0.01f, 10f)] private float _secondsPerFrame = 1;

        

        public SpriteAnimation StartNewAnimation()
        {
            return new SpriteAnimation(_sprites, _loop, _secondsPerFrame);
        }

    }
}