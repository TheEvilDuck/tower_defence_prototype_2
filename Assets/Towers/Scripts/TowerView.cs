using System.Collections;
using System.Collections.Generic;
using Components;
using Enemies;
using UnityEngine;

namespace Towers.View
{
    public class TowerView : MonoBehaviour
    {
        [SerializeField]private Tower _tower;
        [SerializeField]private SimpleSpriteAnimationComponent _particles;
        [SerializeField]private Transform _particlesPlace;
        [SerializeField]private Transform _particlesTransform;
        [SerializeField]private float _attackRecoil = 10f;

        private Transform _targetTransform;
        private float _currentOffsetRecoil = 0;
        private Vector3 _positionBeforeRecoil;
        private Vector3 _directionToLastTarget;

        private void Awake() 
        {
            _tower.targetChanged+=OnTowerTargetChanged;
            _tower.attacked+=OnTowerAttacked;
            _particles.animationEnd+=OnParticlesAnimationEnd;
        }

        private void Start() 
        {
            _positionBeforeRecoil = transform.position;   
        }

        private void OnDestroy() 
        {
            _tower.targetChanged-=OnTowerTargetChanged;
            _tower.attacked-=OnTowerAttacked;
            _particles.animationEnd-=OnParticlesAnimationEnd;
        }

        private void Update() 
        {

            if (_currentOffsetRecoil>0)
            {
                transform.position=_positionBeforeRecoil-_directionToLastTarget.normalized*_currentOffsetRecoil;
                _currentOffsetRecoil-=Time.deltaTime;
            }

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
            _particles.StartAnimation();

            _currentOffsetRecoil = _attackRecoil;
        }
        private void OnParticlesAnimationEnd() => _particles.gameObject.SetActive(false);
    }
}
