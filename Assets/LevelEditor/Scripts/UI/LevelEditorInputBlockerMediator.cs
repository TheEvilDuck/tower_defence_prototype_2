using System;
using System.Collections;
using System.Collections.Generic;
using Common.UI;
using Services.PlayerInput;
using UnityEngine;

namespace LevelEditor.UI
{
    public class LevelEditorInputBlockerMediator : IDisposable
    {
        private UIInputBlocker _inputBlocker;
        private PlayerInput _playerInput;

        public LevelEditorInputBlockerMediator(UIInputBlocker uIInputBlocker, PlayerInput playerInput)
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

        private void OnBlockerStart() => _playerInput.MouseBlocked = true;
        private void OnBlockerEnd() => _playerInput.MouseBlocked = false;
    }
}
