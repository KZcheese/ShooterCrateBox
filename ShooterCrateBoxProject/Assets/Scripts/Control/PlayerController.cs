using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Relays information from the Player Input component to a Player class,
/// allowing the user to control a player character using the new Unity Input
/// System.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player;

    public void OnFireInput(InputAction.CallbackContext context)
    {

    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        // True on first frame jump input is pressed.
        if (context.started)
        {
            player.Jumper2D.Jump();
        }
        // True on the first frame the jump input is released.
        else if (context.canceled)
        {
            player.Jumper2D.StopVariableJump();
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        player.Mover2D.MoveInput = context.ReadValue<float>();
    }
}
