using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// ObjectPool design pattern for projectiles. Uses Unity's built in object
/// pooling system introduced in 2021.
/// </summary>
public class ProjectilePool : MonoBehaviour
{
    /// <summary>
    /// Unique projectile type to be pooled.
    /// </summary>
    public ProjectileType ProjectileType;

    /// <summary>
    /// Projectile prefab to be pooled.
    /// </summary>
    public GameObject ProjectilePrefab;

    /// <summary>
    /// Collection checks will throw errors if we try to release an item that is 
    /// already in the pool.
    /// </summary>
    public bool CollectionCheck = true;

    /// <summary>
    /// Maximum number of projectiles that will be stored in the pool at one
    /// time. Any extra projectiles will be instantiated and destroyed.
    /// </summary>
    public int MaxPoolSize = 10;

    /// <summary>
    /// IObjectPool used to contain the pooled projectiles.
    /// </summary>
    private IObjectPool<GameObject> pool;

    /// <summary>
    /// IObjectPool used to contain the pooled projectiles.
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
        GameObject projectileObject = Instantiate(ProjectilePrefab, transform);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Pool = Pool;
        return projectileObject;
    }

    /// <summary>
    /// Instructions for how to deal with projectiles that eclipse the maximum
    /// pool size.
    /// </summary>
    /// <param name="projectile">Offending GameObject.</param>
    private void OnDestroyPoolObject(GameObject projectile)
    {
        Destroy(projectile);
    }

    /// <summary>
    /// Instructions for returning the projectile to its pool.
    /// </summary>
    /// <param name="projectile">Projectile to be returned to the pool.</param>
    private void OnReturnedToPool(GameObject projectile)
    {
        projectile.transform.SetParent(transform);
        projectile.SetActive(false);
    }

    /// <summary>
    /// Instructions for taking the projectile from its pool.
    /// </summary>
    /// <param name="projectile">Projectile to be taken from the pool.</param>
    private void OnTakeFromPool(GameObject projectile)
    {
        projectile.transform.SetParent(null);
        projectile.SetActive(true);
    }
}
