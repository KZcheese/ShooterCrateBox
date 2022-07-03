using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a random Enemy actor at an assigned enemy spawn point.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// Multiply player's score by this to calculate how many enemies are 
    /// spawned in a wave.
    /// </summary>
    [Tooltip("Multiply player's score by this to calculate how many " +
        "enemies are spawned in a wave.")]
    [SerializeField] private float enemyCounterCoefficient = 0.5f;
    [SerializeField] private RollerEnemyPool rollerEnemyPool;
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private Vector2Int intrawaveWaitTime;
    [SerializeField] private Vector2Int interwaveWaitTime;
    [SerializeField] private IntVariable playerScore;

    private IEnumerator currentCoroutine;

    public void OnGameStart()
    {
        SpawnRollerEnemy();
        StartSpawnCycle();
    }

    public void OnGameEnd()
    {
        StopAllCoroutines();
    }

    private void StartSpawnCycle()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = SpawnCycleRoutine();
        StartCoroutine(currentCoroutine);
    }

    private IEnumerator SpawnCycleRoutine()
    {
        int timeBetweenWaves = Random.Range(interwaveWaitTime.x,
            interwaveWaitTime.y);

        yield return new WaitForSeconds(timeBetweenWaves);

        int enemiesToSpawn = Mathf.RoundToInt(Mathf.Max(2, playerScore.Value *
            enemyCounterCoefficient));
        Debug.Log(enemiesToSpawn);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnRollerEnemy();

            int timeBetweenEnemies = Random.Range(intrawaveWaitTime.x,
                intrawaveWaitTime.y);

            yield return new WaitForSeconds(timeBetweenEnemies);
        }

        StartSpawnCycle();
    }

    private void SpawnRollerEnemy()
    {
        GameObject enemyObject = rollerEnemyPool.Pool.Get();

        // This enemy may be reused, so reset it's health before spawning.
        Health rollerEnemyHealth = enemyObject.GetComponent<Health>();
        rollerEnemyHealth.ResetHealth();

        enemyObject.transform.position = enemySpawnPoint.position;
    }
}
