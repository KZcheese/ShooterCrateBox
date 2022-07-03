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
    [SerializeField] private CommandStream playerCommandStream;

    private FireCommand fire = new FireCommand();
    private JumpCommand jump = new JumpCommand();
    private MoveCommand move = new MoveCommand();
    private StopFireCommand stopFire = new StopFireCommand();
    private StopVariableJumpCommand stopVariableJump = 
        new StopVariableJumpCommand();
    private UpdateWeaponHandlerCommand updateWeaponHandler =
        new UpdateWeaponHandlerCommand();


    public void OnFireInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerCommandStream.Enqueue(fire);
        }
        else if (context.canceled)
        {
            playerCommandStream.Enqueue(stopFire);
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        // True on first frame jump input is pressed.
        if (context.started)
        {
            playerCommandStream.Enqueue(jump);
        }
        // True on the first frame the jump input is released.
        else if (context.canceled)
        {
            playerCommandStream.Enqueue(stopVariableJump);
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        int newDir = Mathf.RoundToInt(context.ReadValue<float>());
        if (newDir != 0)
        {
            updateWeaponHandler.NewDir = newDir;
            playerCommandStream.Enqueue(updateWeaponHandler);
        }

        move.MoveInput = context.ReadValue<float>();
        playerCommandStream.Enqueue(move);
    }
}
