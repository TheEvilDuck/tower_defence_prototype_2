using UnityEngine;

namespace Towers.View
{
    public class TiledShotTowerView: TowerView
    {
        [SerializeField] private SpriteRenderer _particlesSpriteRenderer;
        private Vector3 _startLocalPosition;
        protected override void PariclesOnAttack()
        {
            base.PariclesOnAttack();

            _particlesSpriteRenderer.drawMode = SpriteDrawMode.Tiled;

            int i = 1;

            var scale = _particlesSpriteRenderer.transform.lossyScale;

            i = (int)(_directionToLastTarget.magnitude / scale.y);

            _particlesSpriteRenderer.size = new Vector2(1,i);
            _particlesSpriteRenderer.transform.localPosition = _startLocalPosition + new Vector3(0,i/2,0);
        }

        protected override void OnInit()
        {
            _startLocalPosition = _particlesSpriteRenderer.transform.localPosition;
        }
    }
}