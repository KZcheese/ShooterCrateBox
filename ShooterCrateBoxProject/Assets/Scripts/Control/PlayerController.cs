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
        
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {

    }
}
