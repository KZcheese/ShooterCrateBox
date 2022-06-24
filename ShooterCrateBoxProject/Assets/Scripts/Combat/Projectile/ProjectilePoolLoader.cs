using UnityEngine;

/// <summary>
/// Creates an Object Pool for each unique projectile at runtime.
/// </summary>
public class ProjectilePoolLoader : MonoBehaviour
{
    /// <summary>
    /// Max pool size of each pool created.
    /// </summary>
    [SerializeField] private int maxPoolSize = 20;

    /// <summary>
    /// List of all potential projectile prefabs in the game.
    /// </summary>
    [SerializeField] private GameObject[] projectilePrefabs;

    #region MonoBehaviour Methods
    private void Awake()
    {
        foreach (GameObject projectilePrefab in projectilePrefabs)
        {
            CreateNewProjectilePool(projectilePrefab);
        }
    }
    #endregion

    /// <summary>
    /// Creates a new projectile pool from the passed prefab and parents its
    /// gameObject to the ProjectilePoolLoader's transform, which then acts as
    /// a root for all projectile pools.
    /// </summary>
    /// <param name="projectilePrefab">Prefab to create a pool of.</param>
    private void CreateNewProjectilePool(GameObject projectilePrefab)
    {
        string poolName = projectilePrefab.name + "Pool";
        GameObject poolParent = new GameObject(poolName);
        poolParent.transform.SetParent(transform);

        Projectile projectile = projectilePrefab.GetComponent<Projectile>();

        ProjectilePool pool = poolParent.AddComponent<ProjectilePool>();
        pool.ProjectileType = projectile.ProjectileType;
        pool.CollectionCheck = true;
        pool.MaxPoolSize = maxPoolSize;
        pool.ProjectilePrefab = projectilePrefab;
    }
}
