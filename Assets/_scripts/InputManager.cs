using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Vector2 moveInput;
    public Vector2 lookInput;

    public bool isJumping;
    public bool isSprinting;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        isJumping = value.isPressed;
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }
}
