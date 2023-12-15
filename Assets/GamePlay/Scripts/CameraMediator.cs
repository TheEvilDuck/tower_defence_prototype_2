using System;
using Services.CameraManipulation;
using Services.PlayerInput;

namespace GamePlay
{
    public class CameraMediator: IDisposable
    {
        private PlayerInput _playerInput;
        private CameraManipulation _cameraManipulation;

        public CameraMediator(PlayerInput playerInput,CameraManipulation cameraManipulation)
        {
            _playerInput = playerInput;
            _cameraManipulation = cameraManipulation;

            _playerInput.movementInput+=_cameraManipulation.MoveInDirection;
        }

        public void Dispose()
        {
            _playerInput.movementInput-=_cameraManipulation.MoveInDirection;
        }
    }
}
