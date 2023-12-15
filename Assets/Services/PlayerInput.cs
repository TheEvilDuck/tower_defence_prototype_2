using System;
using UnityEngine;

namespace Services.PlayerInput
{
    public class PlayerInput
    {
        public event Action<Vector2> mouseLeftClicked;
        public event Action<Vector2> mouseRightClicked;
        public event Action<Vector2> movementInput;


        public void Update()
        {
            Vector2 moveVector = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

            movementInput?.Invoke(moveVector);

            if (Input.GetMouseButtonDown(0))
                mouseLeftClicked?.Invoke(Input.mousePosition);

            if (Input.GetMouseButtonDown(1))
                mouseRightClicked?.Invoke(Input.mousePosition);
        }
    }
}
