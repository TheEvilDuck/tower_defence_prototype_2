using System;
using Common.UI;
using Services.PlayerInput;

namespace Common.UI.InputBlocking
{
    public class InputBlockerMediator : IDisposable
    {
        private UIInputBlocker _inputBlocker;
        private PlayerInput _playerInput;

        public InputBlockerMediator(UIInputBlocker uIInputBlocker, PlayerInput playerInput)
        {
            _inputBlocker = uIInputBlocker;
            _playerInput = playerInput;

            _inputBlocker.blockStarted+=OnBlockerStart;
            _inputBlocker.blockEnded+=OnBlockerEnd;
        }

        public void Dispose()
        {
            _inputBlocker.blockStarted-=OnBlockerStart;
            _inputBlocker.blockEnded-=OnBlockerEnd;
        }

        private void OnBlockerStart() => _playerInput.BlockMouse();
        private void OnBlockerEnd() => _playerInput.UnBlockMouse();
    }
}
