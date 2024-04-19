using Components;
using Components.SimpleSpriteAnimator;
using Enemies;
using UnityEngine;

namespace Towers.View
{
    public class TowerView : MonoBehaviour
    {
        [SerializeField]private Tower _tower;
        [SerializeField]private SimpleSpriteAnimationComponent _particles;
        [SerializeField]private SpriteAnimationData _particlesAnimation;
        [SerializeField]private ComponentsAnimator _animator;
        [SerializeField]private Transform _particlesPlace;
        [SerializeField]private Transform _particlesTransform;
        [SerializeField]private float _attackRecoil = 6f;

        private Transform _targetTransform;
        private Vector3 _directionToLastTarget;
        private PositionAnimation _recoilAnimation;
        private Vector3 _startPosition;

        private void Awake() 
        {
            _tower.targetChanged+=OnTowerTargetChanged;
            _tower.attacked+=OnTowerAttacked;
            _particles.animationEnd+=OnParticlesAnimationEnd;

            WiggleAnimationValueUpdater updater = new WiggleAnimationValueUpdater(0.5f,1f,90);
            _startPosition = transform.localPosition;
            _recoilAnimation = new PositionAnimation(updater,transform, Vector2.zero, _startPosition);
        }

        private void OnDestroy() 
        {
            _tower.targetChanged-=OnTowerTargetChanged;
            _tower.attacked-=OnTowerAttacked;
            _particles.animationEnd-=OnParticlesAnimationEnd;
        }

        private void Update() 
        {
            if (_targetTransform==null)
                return;

            _directionToLastTarget = _targetTransform.position-transform.position;
            float angle = Mathf.Atan2(_directionToLastTarget.y,_directionToLastTarget.x)*Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle-90,Vector3.forward);
        }

        private void OnTowerTargetChanged(Enemy target)
        {
            if (target!=null)
                _targetTransform = target.transform;
            else
                _targetTransform = null;
        }

        private void OnTowerAttacked()
        {
            _particles.gameObject.SetActive(true);
            _particlesTransform.rotation = transform.rotation;
            _particlesTransform.position = _particlesPlace.position;
            _particles.StartAnimation(_particlesAnimation);

            _recoilAnimation.ChangeOffset(-_directionToLastTarget.normalized*_attackRecoil);
            _animator.AddAnimation(_recoilAnimation);

        }
        private void OnParticlesAnimationEnd() => _particles.gameObject.SetActive(false);
    }
}
