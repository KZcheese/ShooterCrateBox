using UnityEngine;

/// <summary>
/// Reads information from Firearm ScriptableObject, allowing the actor to 
/// use the firearm in the game world.
/// </summary>
public class WeaponHandler : MonoBehaviour
{
    /// <summary>
    /// GameEvent that signals the camera to shake.
    /// </summary>
    [SerializeField] private GameEvent cameraShakeEvent;

    /// <summary>
    /// Float variable representing the intensity of any camera shake caused by 
    /// weapon fire.
    /// </summary>
    [SerializeField] private FloatVariable cameraShakeIntensity;

    /// <summary>
    /// Float variable representing the time of any camera shake caused by 
    /// weapon fire.
    /// </summary>
    [SerializeField] private FloatVariable cameraShakeTime;

    /// <summary>
    /// Firearm the weaponHandler has equipped.
    /// </summary>
    [SerializeField] private Firearm currentFirearm;

    /// <summary>
    /// GameEvent that signals fire audio to play.
    /// </summary>
    [SerializeField] private GameEvent playWeaponAudioEvent;

    /// <summary>
    /// Transform whose children contain projectile object pools.
    /// </summary>
    [SerializeField] private Transform projectilePoolRoot;

    /// <summary>
    /// Transform whose location dictates the start position of projectiles
    /// fired from this weaponHandler's current weapon.
    /// </summary>
    [SerializeField] private Transform projectileStart;

    /// <summary>
    /// Rigidbody2D component that will react to gunfire.
    /// </summary>
    [SerializeField] private Rigidbody2D rb2D;

    /// <summary>
    /// AudioClipVariable containing active weapon's fire audio clip.
    /// </summary>
    [SerializeField] private AudioClipVariable weaponFireClip;

    /// <summary>
    /// Graphics renderer for the weapon.
    /// </summary>
    [SerializeField] private SpriteRenderer weaponRenderer;

    /// <summary>
    /// Denotes whether the weaponHandler is currently firing its weapon.
    /// </summary>
    private bool isFiring = false;

    /// <summary>
    /// Tracks for how long the weapon has been on cooldown (in between shots.)
    /// </summary>
    private float cooldownTimer = 0.0f;

    /// <summary>
    /// The current projectile pool from which this weaponHandler is using
    /// projectiles.
    /// </summary>
    private ProjectilePool currentPool;

    /// <summary>
    /// Array of projectile pools from which the weaponHandler can use 
    /// projectiles.
    /// </summary>
    private ProjectilePool[] projectilePools;

    #region Properties
    /// <summary>
    /// Firearm the weaponHandler has equipped.
    /// </summary>
    public Firearm CurrentFirearm
    {
        get
        {
            return currentFirearm;
        }
        set
        {
            currentFirearm = value;

            if (currentFirearm != null)
            {
                LoadFirearmGraphics();
                SelectProjectilePool();
            }
        }
    }

    /// <summary>
    /// Direction in which this weaponHandler should fire its weapon.
    /// </summary>
    public int Direction { get; set; } = 1;
    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        LoadProjectilePools();

        Direction = 1;

        if (CurrentFirearm != null)
        {
            LoadFirearmGraphics();
            SelectProjectilePool();
        }
    }
    private void Update()
    {
        if (cooldownTimer > 0.0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        OnFireHold();
    }
    #endregion

    /// <summary>
    /// Describes the behaviour of this WeaponHandler when fire input is 
    /// released. 
    /// </summary>
    public void OnFireEnd()
    {
        isFiring = false;
    }

    /// <summary>
    /// Describes the behaviour of this WeaponHandler when fire input is
    /// pressed.
    /// </summary>
    public void OnFireStart()
    {
        switch (CurrentFirearm.FireMode)
        {
            case FireMode.Auto:
                isFiring = true;
                if (cooldownTimer <= 0.0f)
                {
                    FireProjectile();
                }
                break;
            case FireMode.SemiAuto:
                if (cooldownTimer <= 0.0f)
                {
                    isFiring = true;
                    FireProjectile();
                }
                break;
            default:
                isFiring = true;
                if (cooldownTimer <= 0.0f)
                {
                    FireProjectile();
                }
                break;
        }
    }

    /// <summary>
    /// Describes the behaviour of this WeaponHandler when fire input is 
    /// held. 
    /// </summary>
    private void OnFireHold()
    {
        if (CurrentFirearm.FireMode == FireMode.Auto)
        {
            if (isFiring)
            {
                if (cooldownTimer <= 0.0f)
                {
                    FireProjectile();
                }
            }
        }
    }

    /// <summary>
    /// Fires projectile(s) following parameters.
    /// </summary>
    private void FireProjectile()
    {
        for (int i = 0; i < CurrentFirearm.ProjectilesPerShot; i++)
        {
            // Retrieve projectile from pool
            GameObject projectileObject = currentPool.Pool.Get();
            projectileObject.transform.position = projectileStart.position;

            // Assign Projectile Parameters
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.UseLifeTime = CurrentFirearm.UseProjectileLifetime;
            projectile.LifeTimer = projectile.UseLifeTime ?
                CurrentFirearm.ProjectileLifetime : 0.0f;
            projectile.Damage = CurrentFirearm.Damage;
            projectile.KnockbackForce = CurrentFirearm.Knockback;

            // Fire projectile
            Rigidbody2D projectileRb2D = projectileObject.GetComponent<Rigidbody2D>();

            float projectileVelocity = Direction *
                (CurrentFirearm.RandomProjectileVelocity ?
                Random.Range(CurrentFirearm.ProjectileVelocityMin,
                CurrentFirearm.ProjectileVelocityMax) :
                CurrentFirearm.ProjectileVelocity);

            if (CurrentFirearm.AccuracyConeAngle > 0.0f)
            {
                float randomDegree =
                    Random.Range(-(CurrentFirearm.AccuracyConeAngle * 0.5f),
                    CurrentFirearm.AccuracyConeAngle * 0.5f);

                float randX = Mathf.Cos(Mathf.Deg2Rad * randomDegree);
                float randY = Mathf.Sin(Mathf.Deg2Rad * randomDegree);

                Vector2 direction = new Vector2(randX, randY).normalized;
                projectileRb2D.velocity = direction * projectileVelocity;
            }
            else
            {
                projectileRb2D.velocity = new Vector2(projectileVelocity, 0.0f);
            }
        }

        // Play Audio
        weaponFireClip.Value = CurrentFirearm.FireAudio;
        playWeaponAudioEvent.Raise();

        // Apply Camera Shake
        if (CurrentFirearm.UseCameraShake)
        {
            cameraShakeIntensity.Value = CurrentFirearm.CameraShakeIntensity;
            cameraShakeTime.Value = CurrentFirearm.CameraShakeTime;
            cameraShakeEvent.Raise();
        }

        // Apply Kickback
        rb2D.AddForce(new Vector2(-Direction *
            CurrentFirearm.Recoil, 0.0f), ForceMode2D.Impulse);

        // Set cooldown timer.
        cooldownTimer = CurrentFirearm.MinTimeBetweenShots;
    }

    /// <summary>
    /// Displays the firearm in the gameworld by adjusting the renderer's 
    /// sprite, color, size and position. Adjusts the projectileStart position.
    /// </summary>
    private void LoadFirearmGraphics()
    {
        weaponRenderer.sprite = CurrentFirearm.Sprite;
        weaponRenderer.color = CurrentFirearm.Color;

        weaponRenderer.transform.localScale = CurrentFirearm.Scale;
        weaponRenderer.transform.localPosition = CurrentFirearm.Offset;

        projectileStart.localPosition = CurrentFirearm.ProjectileOffset;
    }

    /// <summary>
    /// Gathers a list of active projectile pools.
    /// </summary>
    private void LoadProjectilePools()
    {
        projectilePools =
            projectilePoolRoot.GetComponentsInChildren<ProjectilePool>();
    }

    /// <summary>
    /// Determines which projectile pool the weaponHandler should use
    /// projectiles from based on its CurrentFirearm.
    /// </summary>
    private void SelectProjectilePool()
    {
        foreach (ProjectilePool pool in projectilePools)
        {
            if (pool.ProjectileType == CurrentFirearm.ProjectileType)
            {
                currentPool = pool;
                return;
            }
        }
    }
}
