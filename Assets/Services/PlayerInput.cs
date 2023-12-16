using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.PlayerInput
{
    public class PlayerInput
    {
        public event Action<Vector2> mouseLeftClicked;
        public event Action<Vector2> mouseRightClicked;
        public event Action<Vector2> movementInput;
        public event Action<KeyCode[]>keysCombinationHold;
        public event Action<KeyCode[]>keyCombinationDown;
        public event Action<KeyCode>keyHold;
        public event Action<KeyCode>keyDown;

        private readonly KeyCode[] WATCHED_KEYS = {KeyCode.LeftControl,KeyCode.Z, KeyCode.X};

        List<KeyCode>_currentCombination;

        public PlayerInput()
        {
            _currentCombination = new List<KeyCode>();
        }


        public void Update()
        {
            Vector2 moveVector = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

            movementInput?.Invoke(moveVector);

            if (Input.GetMouseButtonDown(0))
                mouseLeftClicked?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (Input.GetMouseButtonDown(1))
                mouseRightClicked?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            foreach (KeyCode key in WATCHED_KEYS)
            {
                if (Input.GetKeyDown(key))
                {
                    keyDown?.Invoke(key);

                    if (!_currentCombination.Contains(key))
                    {
                        _currentCombination.Add(key);
                    }

                    if (_currentCombination.Count>0)
                        keyCombinationDown?.Invoke(_currentCombination.ToArray());
                }

                if (Input.GetKeyUp(key))
                {
                    if (_currentCombination.Contains(key))
                        _currentCombination.Remove(key);

                    keyHold?.Invoke(key);
                }
            }

            if (_currentCombination.Count>0)
                keysCombinationHold?.Invoke(_currentCombination.ToArray());
        }
    }
}
