using UnityEngine;

namespace Services.CameraManipulation
{
    public class CameraManipulation
    {
        private Camera _camera;
        private float _moveSpeed = 1f;
        private float _changeScaleSpeed = 0.2f;

        public CameraManipulation(float cameraSpeed, Camera camera)
        {
            _moveSpeed = cameraSpeed;
            _camera = camera;
        }

        public void MoveInDirection(Vector2 direction)
        {
            direction = Vector2.ClampMagnitude(direction,1f);
            _camera.transform.Translate(direction*_moveSpeed);
        }

        public void Zoom(bool isIn)
        {
            int direction = 1;

            if (!isIn)
                direction = -1;

            _camera.orthographicSize+=_changeScaleSpeed*direction;
        }
    }
}
