using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Area that instructs colliding enemy actors to evolve (if applicable) and 
/// return to the enemy spawn point for another pass through the level.
/// </summary>
public class EnemyGoal : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    #region MonoBehaviour Methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            RollerEnemy rollerEnemy = other.GetComponent<RollerEnemy>();
            rollerEnemy.OnReachedGoal(spawnPoint);
        }
    }
    #endregion
}
