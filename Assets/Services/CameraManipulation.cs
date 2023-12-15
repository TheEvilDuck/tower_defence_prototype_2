using UnityEngine;

namespace Services.CameraManipulation
{
    public class CameraManipulation
    {
        private Camera _camera;
        private float _moveSpeed = 1f;

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
    }
}
