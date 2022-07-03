using UnityEngine;

/// <summary>
/// Deals damage to the player when the player enters its trigger.
/// </summary>
public class Hazard : MonoBehaviour
{
    /// <summary>
    /// Amount of damage to deal to the player.
    /// </summary>
    [SerializeField] private int damage;

    #region MonoBehaviour Methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Here we do not null check for health component, because if
            // the player does not have a health component there is an issue!
            Health health = other.GetComponent<Health>();
            health.LoseHealth(damage);
            Debug.Log(string.Format("Player has taken {0} damage!", damage));
        }
    }
    #endregion
}
