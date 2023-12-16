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
        public event Action<KeyCode>keyHold;
        public event Action<KeyCode>keyDown;

        private readonly KeyCode[] WATCHED_KEYS = {KeyCode.LeftControl,KeyCode.Z, KeyCode.X};


        public void Update()
        {
            Vector2 moveVector = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

            movementInput?.Invoke(moveVector);

            if (Input.GetMouseButtonDown(0))
                mouseLeftClicked?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (Input.GetMouseButtonDown(1))
                mouseRightClicked?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            List<KeyCode>currentCombination = new List<KeyCode>();

            foreach (KeyCode key in WATCHED_KEYS)
            {
                if (Input.GetKey(key))
                {
                    keyHold?.Invoke(key);
                    currentCombination.Add(key);
                }
            }

            if (currentCombination.Count>0)
                keysCombinationHold?.Invoke(currentCombination.ToArray());
        }
    }
}
