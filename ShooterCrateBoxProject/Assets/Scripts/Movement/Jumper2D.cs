using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acts upon an assigned RigidBody2D. Allows the Rigidbody2D's attached
/// GameObject to jump by adding an initial upward force and manipulating the
/// gravity scale, allowing for highly customizeable jump behaviour.
/// </summary>
public class Jumper2D : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
