using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Tracks the health of an actor. Dictates what happens when the actor
/// loses a health point or runs out of health.
/// </summary>
public class Health : MonoBehaviour
{
    public UnityEvent OnHealthLost;
    public UnityEvent OnHealthEmpty;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
