using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RollerEnemyPool : MonoBehaviour
{
    /// <summary>
    /// Enemy prefab to be pooled.
    /// </summary>
    public GameObject RollerEnemyPrefab;

    /// <summary>
    /// Collection checks will throw errors if we try to release an item that is 
    /// already in the pool.
    /// </summary>
    public bool CollectionCheck = true;

    /// <summary>
    /// Maximum number of enemies that will be stored in the pool at one
    /// time. Any extra enemies will be instantiated and destroyed.
    /// </summary>
    public int MaxPoolSize = 10;

    /// <summary>
    /// IObjectPool used to contain the pooled enemies.
    /// </summary>
    private IObjectPool<GameObject> pool;

    /// <summary>
    /// IObjectPool used to contain the pooled enemies.
    /// </summary>
    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (pool == null)
            {
                pool = new ObjectPool<GameObject>(CreatePooledItem,
                    OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
                    CollectionCheck, 10, MaxPoolSize);
            }
            return pool;
        }
    }

    /// <summary>
    /// Instructions for creating a pooled item.
    /// </summary>
    /// <returns>GameObject representing the projectile to be pooled.</returns>
    private GameObject CreatePooledItem()
    {
        GameObject enemyObject = Instantiate(RollerEnemyPrefab, transform);
        RollerEnemy rollerEnemy = enemyObject.GetComponent<RollerEnemy>();
        rollerEnemy.OriginPool = Pool;
        return enemyObject;
    }

    /// <summary>
    /// Instructions for how to deal with projectiles that eclipse the maximum
    /// pool size.
    /// </summary>
    /// <param name="enemyObject">Offending GameObject.</param>
    private void OnDestroyPoolObject(GameObject enemyObject)
    {
        Destroy(enemyObject);
    }

    /// <summary>
    /// Instructions for returning the projectile to its pool.
    /// </summary>
    /// <param name="enemyObject">Projectile to be returned to the pool.</param>
    private void OnReturnedToPool(GameObject enemyObject)
    {
        enemyObject.transform.SetParent(transform);
        enemyObject.SetActive(false);
    }

    /// <summary>
    /// Instructions for taking the projectile from its pool.
    /// </summary>
    /// <param name="enemyObject">Projectile to be taken from the pool.</param>
    private void OnTakeFromPool(GameObject enemyObject)
    {
        enemyObject.transform.SetParent(null);
        enemyObject.SetActive(true);
    }
}
