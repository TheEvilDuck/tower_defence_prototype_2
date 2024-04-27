using UnityEngine;

namespace Components.ComponentsAnimations
{
    public class ColorAnimation : Animation
    {
        private readonly Color _color;
        private readonly Color _startColor;
        private SpriteRenderer _target;

        public ColorAnimation(IAnimationValueUpdater updater,Color color, SpriteRenderer spriteRenderer): base(updater)
        {
            _color = color;
            _target = spriteRenderer;
            _startColor = _target.color;
        }

        protected override void OnStop()
        {
            _target.color = _startColor;
        }

        protected override void OnUpdate(float value)
        {
            Color color = new Color(
                Mathf.Lerp(_startColor.r,_color.r,value),
                Mathf.Lerp(_startColor.g,_color.g,value),
                Mathf.Lerp(_startColor.b,_color.b,value)
                );

            _target.color = color;
        }
    }
}