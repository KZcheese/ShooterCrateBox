using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a random Enemy actor at an assigned enemy spawn point.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private RollerEnemyPool rollerEnemyPool;
    [SerializeField] private Transform enemySpawnPoint;

    #region MonoBehaviour Methods
    private void Start()
    {
        SpawnRollerEnemy();
    }
    #endregion

    private void SpawnRollerEnemy()
    {
        GameObject enemyObject = rollerEnemyPool.Pool.Get();

        // This enemy may be reused, so reset it's health before spawning.
        Health rollerEnemyHealth = enemyObject.GetComponent<Health>();
        rollerEnemyHealth.ResetHealth();
        
        enemyObject.transform.position = enemySpawnPoint.position;
    }
}
