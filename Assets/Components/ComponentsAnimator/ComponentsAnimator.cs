using System.Collections.Generic;
using UnityEngine;

namespace Components.ComponentsAnimations
{
    public class ComponentsAnimator: MonoBehaviour
    {
        private List<Animation>_animations;

        private List<Animation>_animationsMarkedToDelete;

        private void Awake() 
        {
            _animations = new List<Animation>();
            _animationsMarkedToDelete = new List<Animation>();
        }

        public void AddAnimation(Animation animation)
        {
            if (!_animations.Contains(animation))
            {
                _animations.Add(animation);
                
            }

            animation.Start();
        }

        private void Update() 
        {
            foreach (Animation animation in _animations)
                animation.Update();

            foreach (Animation animationMarkedToDelete in _animationsMarkedToDelete)
            {
                _animations.Remove(animationMarkedToDelete);
            }

            _animationsMarkedToDelete.Clear();
        }

        private void OnDestroy() 
        {
            foreach (Animation animation in _animations)
            {
                animation.end-=OnAnimationEnd;
                animation.Stop();
            }

            
        }

        private void OnAnimationEnd(Animation animation)
        {
            animation.end-=OnAnimationEnd;
            _animationsMarkedToDelete.Add(animation);
        }
    }
}
