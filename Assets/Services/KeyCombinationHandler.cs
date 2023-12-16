using System;
using System.Linq;
using UnityEngine;

namespace Services.PlayerInput
{
    public class KeyCombinationHandler: IDisposable
    {
        public event Action Down;
        public event Action Hold;

        private readonly PlayerInput _playerInput;
        private readonly KeyCode[] _watchingCombination;

        public KeyCombinationHandler(PlayerInput playerInput, KeyCode[] watchingCombination)
        {
            _playerInput = playerInput;
            _watchingCombination = watchingCombination;

            _playerInput.keyCombinationDown+=OnPlayerInputCombinationDown;
            _playerInput.keysCombinationHold+=OnPlayerInputCombinationHold;
        }

        public void Dispose()
        {
            _playerInput.keyCombinationDown-=OnPlayerInputCombinationDown;
            _playerInput.keysCombinationHold-=OnPlayerInputCombinationHold;
        }

        private void OnPlayerInputCombinationHold(KeyCode[] combination)
        {
            if (CombinationCompare(combination))
                Hold?.Invoke();
        }

        private void OnPlayerInputCombinationDown(KeyCode[] combination)
        {
            if (CombinationCompare(combination))
                Down?.Invoke();
        }

        private bool CombinationCompare(KeyCode[] combination)
        {
            foreach (KeyCode key in _watchingCombination)
            {
                if (!combination.Contains(key))
                    return false;
            }

            return true;
        }
    }
}
