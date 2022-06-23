using UnityEngine;

/// <summary>
/// A set of parameters that describe the behaviour of a firearm used by the 
/// player to defeat enemies.
/// </summary>
[CreateAssetMenu(menuName = "Firearm.../Firearm")]
public class Firearm : ScriptableObject
{
    /// <summary>
    /// The angle of the cone originating from the projectile's start position
    /// within which a projectile will choose a path. If set to 0.0f, the
    /// projectile will fire straight every time.
    /// </summary>
    public float AccuracyConeAngle = 0.0f;

    /// <summary>
    /// Intensity of the camera shake effect.
    /// </summary>
    public float CameraShakeIntensity = 5.0f;

    /// <summary>
    /// Duration of the camera shake effect.
    /// </summary>
    public float CameraShakeTime = 0.1f;

    /// <summary>
    /// Color the firearm graphics are tinted.
    /// </summary>
    public Color Color = Color.white;

    /// <summary>
    /// Amount of damage dealt per projectile.
    /// </summary>
    public int Damage;

    /// <summary>
    /// AudioClip played when the firearm is fired.
    /// </summary>
    public AudioClip FireAudio;

    /// <summary>
    /// Fire behaviour of the weapon (i.e. auto, semi-auto). 
    /// </summary>
    public FireMode FireMode = FireMode.SemiAuto;

    /// <summary>
    /// Amount of force applied to the target in the same direction of the shot.
    /// </summary>
    public Vector2 Knockback;

    /// <summary>
    /// How long in between shots the gun must wait before firing again.
    /// </summary>
    public float MinTimeBetweenShots = 0.1f;

    /// <summary>
    /// Offset from the center of the actor where this weapon will appear.
    /// </summary>
    public Vector2 Offset;

    /// <summary>
    /// How long after spawning does the projectile delete itself.
    /// </summary>
    public float ProjectileLifetime = 1.0f;

    /// <summary>
    /// Offset from the center of the actor where this weapon's projectiles
    /// will begin.
    /// </summary>
    public Vector2 ProjectileOffset;

    /// <summary>
    /// Number of projectiles fired each shot.
    /// </summary>
    public int ProjectilesPerShot = 1;

    /// <summary>
    /// Type of projectile this firearm will fire.
    /// </summary>
    public ProjectileType ProjectileType;

    /// <summary>
    /// The velocity at which projectiles fired from this firearm travel.
    /// </summary>
    public float ProjectileVelocity = 15.0f;

    /// <summary>
    /// Should fired projectiles exit at a random speed?
    /// </summary>
    public float ProjectileVelocityMax = 15.0f;

    /// <summary>
    /// Should fired projectiles exit at a random speed?
    /// </summary>
    public float ProjectileVelocityMin = 5.0f;

    /// <summary>
    /// Should fired projectiles exit at a random velocity?
    /// </summary>
    public bool RandomProjectileVelocity = false;

    /// <summary>
    /// Amount of force applied to the actor opposite the direction of the shot.
    /// </summary>
    public float Recoil = 0.0f;

    /// <summary>
    /// The size of the firearm graphics.
    /// </summary>
    public Vector2 Scale;

    /// <summary>
    /// Sprite representing the firearm in the game world.
    /// </summary>
    public Sprite Sprite;

    /// <summary>
    /// Should the camera shake when the weapon is fired?
    /// </summary>
    public bool UseCameraShake = false;

    /// <summary>
    /// Should projectiles delete themselves after a set time?
    /// </summary>
    public bool UseProjectileLifetime = false;
}
