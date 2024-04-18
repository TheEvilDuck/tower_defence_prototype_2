using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Interfaces;
using UnityEngine;

namespace Services.PlayerInput
{
    public class PlayerInput: IPausable
    {
        public event Action<Vector2> mouseLeftClicked;
        public event Action<Vector2> mouseRightClicked;
        public event Action<Vector2> mouseLeftHold;
        public event Action<Vector2> mouseRightHold;
        public event Action<Vector2> mouseLeftUp;
        public event Action<Vector2> mouseRightUp;
        public event Action<Vector2> movementInput;
        public event Action<Vector2> mousePositionChanged;
        public event Action<KeyCode[]>keysCombinationHold;
        public event Action<KeyCode[]>keyCombinationDown;
        public event Action<KeyCode>keyHold;
        public event Action<KeyCode>keyDown;
        public event Action<float>mouseWheelScrolled;

        //можно будет в меню клавиши назначать, сохранять в конфиг, а потом на сцене и в редакторе подгружать с конфига
        private List<KeyCode> _watchingKeys;
        private bool _paused;

        private List<KeyCode>_currentCombination;
        public bool Blocked {get; set;} = false;
        public bool MouseBlocked {get; set;} = false;
        

        public PlayerInput()
        {
            _currentCombination = new List<KeyCode>();
            _watchingKeys = new List<KeyCode>();
        }

        public void Pause() => _paused = true;

        public void UnPause() => _paused = false;


        public void Update()
        {
            if (_paused)
                return;

            if (Blocked)
                return;

            HandleMovementKeys();
            HandleMouse();
            HandleAnyKeys();
        }

        public void RegisterKeyCode(KeyCode keyCode)
        {
            if (!_watchingKeys.Contains(keyCode))
                _watchingKeys.Add(keyCode);
        }

        private void HandleMouse()
        {
            if (MouseBlocked)
                return;

            Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePositionChanged?.Invoke(mouseScreenPosition);

            if (Input.GetMouseButtonDown(0))
                mouseLeftClicked?.Invoke(mouseScreenPosition);

            if (Input.GetMouseButtonDown(1))
                mouseRightClicked?.Invoke(mouseScreenPosition);

            if (Input.GetMouseButton(0))
                mouseLeftHold?.Invoke(mouseScreenPosition);

            if (Input.GetMouseButton(1))
                mouseRightHold?.Invoke(mouseScreenPosition);

            if (Input.GetMouseButtonUp(0))
                mouseLeftUp?.Invoke(mouseScreenPosition);

            if (Input.GetMouseButtonUp(1))
                mouseRightUp?.Invoke(mouseScreenPosition);

            mouseWheelScrolled?.Invoke(Input.mouseScrollDelta.y);

        }

        private void HandleMovementKeys()
        {
            Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
            movementInput?.Invoke(moveVector);
        }

        private void HandleAnyKeys()
        {
            foreach (KeyCode key in _watchingKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    keyDown?.Invoke(key);

                    if (!_currentCombination.Contains(key))
                        _currentCombination.Add(key);

                    if (_currentCombination.Count>0)
                        keyCombinationDown?.Invoke(_currentCombination.ToArray());
                }

                if (Input.GetKeyUp(key))
                {
                    if (_currentCombination.Contains(key))
                        _currentCombination.Remove(key);

                }

                if (Input.GetKey(key))
                    keyHold?.Invoke(key);
            }

            if (_currentCombination.Count>0)
                keysCombinationHold?.Invoke(_currentCombination.ToArray());
        }
    }
}
