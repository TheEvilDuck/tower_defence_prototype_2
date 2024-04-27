using Components.ComponentsAnimations;
using UnityEngine;

namespace Builder
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlacablePreview : MonoBehaviour
    {
        [SerializeField] private ComponentsAnimator _componentsAnimator;
        private SpriteRenderer _spriteRenderer;
        private ColorAnimation _colorAnimation;

        private void Awake() 
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            SmoothAnimationValueUpdater colorUpdater = new SmoothAnimationValueUpdater(1f, 0f, 70);
            AnimationValueRepeater animationValueRepeater = new AnimationValueRepeater(colorUpdater, 2);
            _colorAnimation = new ColorAnimation(animationValueRepeater,new Color(1,0.2f,0.2f),_spriteRenderer);
        }

        public void UpdatePreview(Texture2D texture)
        {
            var rect = new Rect(0,0,texture.width, texture.height);
            _spriteRenderer.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

            
        }

        public void CantBuildAnimation()
        {
            _componentsAnimator.AddAnimation(_colorAnimation);
        }
    }

}