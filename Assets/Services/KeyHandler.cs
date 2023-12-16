using System;
using UnityEngine;

namespace Services.PlayerInput
{
    public class KeyHandler: IDisposable
    {
        public event Action Down;
        public event Action Hold;

        private KeyCode _key;
        private PlayerInput _playerInput;

        public KeyHandler(PlayerInput playerInput, KeyCode key)
        {
            _key = key;
            _playerInput = playerInput;

            _playerInput.keyDown+=OnKeyDown;
            _playerInput.keyHold+=OnKeyHold;
        }

        public void Dispose()
        {
            _playerInput.keyDown-=OnKeyDown;
            _playerInput.keyHold-=OnKeyHold;
        }

        private void OnKeyDown(KeyCode key)
        {
            if (_key == key)
                Down?.Invoke();
        }

        private void OnKeyHold(KeyCode key)
        {
            if (_key == key)
                Hold?.Invoke();
        }
    }
}
