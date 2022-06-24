using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Describes the behaviour of a projectile fired from a weapon.
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// Unique projectile type. 
    /// </summary>
    public ProjectileType ProjectileType;

    /// <summary>
    /// Damage this projectile inflicts on target.
    /// </summary>
    public int Damage { get; set; } = 0;

    /// <summary>
    /// Knockback force this projectile inflicts on target.
    /// </summary>
    public Vector2 KnockbackForce { get; set; } = Vector2.zero;

    /// <summary>
    /// Tracks how long the projectile has been alive for.
    /// </summary>
    public float LifeTimer { get; set; }

    /// <summary>
    /// The object pool from which this projectile was drawn.
    /// </summary>
    public IObjectPool<GameObject> Pool { get; set; }

    /// <summary>
    /// Should this projectile be removed after a set time?
    /// </summary>
    public bool UseLifeTime { get; set; } = false;

    /// <summary>
    /// Has this object been released back to its object pool?
    /// </summary>
    private bool released = false;

    #region MonoBehaviour Methods
    private void OnEnable()
    {
        released = false;
    }
    private void Update()
    {
        if (UseLifeTime)
        {
            if (LifeTimer > 0.0f)
            {
                LifeTimer -= Time.deltaTime;
            }
            if (LifeTimer <= 0.0f)
            {
                ReleaseToPool();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.LoseHealth(Damage);
        }
        ApplyKnockback(other);
        ReleaseToPool();
    }
    #endregion

    /// <summary>
    /// Releases this object back to its object pool.
    /// </summary>
    private void ReleaseToPool()
    {
        if (Pool != null && !released)
        {
            released = true;
            Pool.Release(gameObject);
        }
    }

    /// <summary>
    /// Applies knockback force to the target struck by this projectile.
    /// </summary>
    /// <param name="other">Collider2D component of the target.</param>
    private void ApplyKnockback(Collider2D other)
    {
        Rigidbody2D targetRb2D = other.GetComponent<Rigidbody2D>();
        if (targetRb2D == null)
        {
            // This clause is more likely and does fewer calculations, so it 
            // comes first.
            return;
        }
        else
        {
            int knockbackDirection =
                (int)Mathf.Sign(other.transform.position.x -
                transform.position.x);

            targetRb2D.AddForce(new Vector2(knockbackDirection * KnockbackForce.x,
                KnockbackForce.y), ForceMode2D.Impulse);
        }
    }
}
