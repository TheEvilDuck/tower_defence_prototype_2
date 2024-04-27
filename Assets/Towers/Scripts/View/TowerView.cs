using Components.ComponentsAnimations;
using Components.SimpleSpriteAnimator;
using Enemies;
using UnityEngine;

namespace Towers.View
{
    public class TowerView : MonoBehaviour
    {
        [SerializeField]protected Tower _tower;
        [SerializeField]protected SimpleSpriteAnimationComponent _particles;
        [SerializeField]protected SpriteAnimationData[] _particlesAnimations;
        [SerializeField, Range(-180f, 180f)]protected float _particlesRotationAngle;
        [SerializeField]private ComponentsAnimator _animator;
        [SerializeField]protected Transform _particlesPlace;
        [SerializeField]protected Transform _particlesTransform;
        [SerializeField]private float _attackRecoil = 6f;

        protected Transform _targetTransform;
        protected Vector3 _directionToLastTarget;
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
            _particles.transform.Rotate(0,0,  _particlesRotationAngle,  Space.Self);

            OnInit();
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
            _recoilAnimation.ChangeOffset(-_directionToLastTarget.normalized*_attackRecoil);
            _animator.AddAnimation(_recoilAnimation);
            PariclesOnAttack();
        }

        protected virtual void PariclesOnAttack()
        {
            SpriteAnimationData randomAnimation = _particlesAnimations[UnityEngine.Random.Range(0, _particlesAnimations.Length)];
            _particles.StartAnimation(randomAnimation);
            _particles.gameObject.SetActive(true);
            _particlesTransform.rotation = transform.rotation;
            _particlesTransform.position = _particlesPlace.position;
        }
        private void OnParticlesAnimationEnd() => _particles.gameObject.SetActive(false);
        protected virtual void OnInit() {}
    }
}
