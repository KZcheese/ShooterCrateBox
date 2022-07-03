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
    /// <summary>
    /// Number of hits this health component can take before being defeated.
    /// </summary>
    public int MaxHealthPoints = 3;

    /// <summary>
    /// Response to losing health. Configured in the inspector.
    /// </summary>
    public UnityEvent OnHealthLost;

    /// <summary>
    /// Response to losing final health point. Configured in the inspector.
    /// </summary>
    public UnityEvent OnHealthEmpty;

    private float currentHealthPoints;

    #region MonoBehaviour Methods
    private void Start()
    {
        ResetHealth();
    }
    #endregion

    /// <summary>
    /// Reduces health by amount of damage dealt and invokes appropriate events.
    /// </summary>
    /// <param name="damage">Amount by which to reduce health.</param>
    public void LoseHealth(int damage)
    {
        currentHealthPoints -= damage;

        if (currentHealthPoints == 0)
        {
            OnHealthEmpty.Invoke();
            return;
        }

        OnHealthLost.Invoke();
    }

    /// <summary>
    /// Resets the health component.
    /// </summary>
    public void ResetHealth()
    {
        currentHealthPoints = MaxHealthPoints;
    }
}
