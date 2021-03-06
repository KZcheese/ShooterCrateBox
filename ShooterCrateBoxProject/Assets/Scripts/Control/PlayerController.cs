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
        if (context.started)
        {
            player.WeaponHandler.OnFireStart();
        }
        else if (context.canceled)
        {
            player.WeaponHandler.OnFireEnd();
        }
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
        int newDir = Mathf.RoundToInt(context.ReadValue<float>());
        if (newDir != 0)
        {
            player.WeaponHandler.transform.localScale = 
                new Vector3(newDir, player.WeaponHandler.transform.localScale.y,
                player.WeaponHandler.transform.localScale.z);
            player.WeaponHandler.Direction = newDir;
        }
        player.Mover2D.MoveInput = context.ReadValue<float>();
    }
}
