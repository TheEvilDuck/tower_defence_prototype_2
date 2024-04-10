using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.PlayerInput
{
    public class KeyHandler: IDisposable,IPlayerInputWatcher
    {
        public event Action Down;
        public event Action Hold;

        private readonly KeyCode _key;
        private readonly PlayerInput _playerInput;

        public KeyHandler(PlayerInput playerInput, KeyCode key)
        {
            _key = key;
            _playerInput = playerInput;

            _playerInput.keyDown+=OnKeyDown;
            _playerInput.keyHold+=OnKeyHold;

            _playerInput.RegisterKeyCode(_key);
        }

        public void Dispose()
        {
            _playerInput.keyDown-=OnKeyDown;
            _playerInput.keyHold-=OnKeyHold;
        }

        private void OnKeyDown(KeyCode key)
        {
            if (_key == key)
            {
                Down?.Invoke();
            }
        }

        private void OnKeyHold(KeyCode key)
        {
            if (_key == key)
                Hold?.Invoke();
        }
    }
}
