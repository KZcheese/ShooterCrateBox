using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom Editor for the Firearm class.
/// </summary>
[CustomEditor(typeof(Firearm))]
[CanEditMultipleObjects]
public class FirearmEditor : Editor
{
    // Appearance Properties
    private SerializedProperty colorProperty;
    private SerializedProperty offsetProperty;
    private SerializedProperty projectileOffsetProperty;
    private SerializedProperty scaleProperty;
    private SerializedProperty spriteProperty;

    // Combat Properties
    private SerializedProperty damageProperty;
    private SerializedProperty knockbackProperty;
    private SerializedProperty recoilProperty;

    // Fire Strategy Properties
    private SerializedProperty accuracyConeAngleProperty;
    private SerializedProperty fireModeProperty;
    private SerializedProperty minTimeBetweenShotsProperty;
    private SerializedProperty projectileLifetimeProperty;
    private SerializedProperty projectilesPerShotProperty;
    private SerializedProperty projectileTypeProperty;
    private SerializedProperty projectileVelocityProperty;
    private SerializedProperty projectileVelocityMinProperty;
    private SerializedProperty projectileVelocityMaxProperty;
    private SerializedProperty useProjectileLifetimeProperty;
    private SerializedProperty randomProjectileVelocityProperty;

    // Fire Effects Properties
    private SerializedProperty useCameraShakeProperty;
    private SerializedProperty cameraShakeIntensityProperty;
    private SerializedProperty cameraShakeTimeProperty;
    private SerializedProperty fireAudioProperty;


    #region Editor Methods
    private void OnEnable()
    {
        // Find Appearance Properties
        colorProperty = serializedObject.FindProperty("Color");
        offsetProperty = serializedObject.FindProperty("Offset");
        projectileOffsetProperty = serializedObject.FindProperty("ProjectileOffset");
        scaleProperty = serializedObject.FindProperty("Scale");
        spriteProperty = serializedObject.FindProperty("Sprite");

        // Find Combat Properties
        damageProperty = serializedObject.FindProperty("Damage");
        recoilProperty = serializedObject.FindProperty("Recoil");
        knockbackProperty = serializedObject.FindProperty("Knockback");

        // Find Fire Strategy Properties
        accuracyConeAngleProperty = serializedObject.FindProperty("AccuracyConeAngle");
        fireModeProperty = serializedObject.FindProperty("FireMode");
        minTimeBetweenShotsProperty = serializedObject.FindProperty("MinTimeBetweenShots");
        projectileLifetimeProperty = serializedObject.FindProperty("ProjectileLifetime");
        projectilesPerShotProperty = serializedObject.FindProperty("ProjectilesPerShot");
        projectileTypeProperty = serializedObject.FindProperty("ProjectileType");
        projectileVelocityProperty = serializedObject.FindProperty("ProjectileVelocity");
        projectileVelocityMinProperty = serializedObject.FindProperty("ProjectileVelocityMin");
        projectileVelocityMaxProperty = serializedObject.FindProperty("ProjectileVelocityMax");
        useProjectileLifetimeProperty = serializedObject.FindProperty("UseProjectileLifetime");
        randomProjectileVelocityProperty = serializedObject.FindProperty("RandomProjectileVelocity");

        // Find Fire Effects Properties
        useCameraShakeProperty = serializedObject.FindProperty("UseCameraShake");
        cameraShakeIntensityProperty = serializedObject.FindProperty("CameraShakeIntensity");
        cameraShakeTimeProperty = serializedObject.FindProperty("CameraShakeTime");
        fireAudioProperty = serializedObject.FindProperty("FireAudio");

    }

    public override void OnInspectorGUI()
    {
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script",
            MonoScript.FromScriptableObject((ScriptableObject)target),
            GetType(), false);
            
        DrawAppearance();
        DrawCombatData();
        DrawFireStrategy();
        DrawFireEffects();

        serializedObject.ApplyModifiedProperties();
    }
    #endregion

    /// <summary>
    /// Draws Appearance parameters in the custom editor window.
    /// </summary>
    private void DrawAppearance()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Appearance", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(spriteProperty);
        EditorGUILayout.PropertyField(colorProperty);
        EditorGUILayout.PropertyField(scaleProperty);
        EditorGUILayout.PropertyField(offsetProperty);
        EditorGUILayout.PropertyField(projectileOffsetProperty);
    }

    /// <summary>
    /// Draws Combat Data parameters in the custom editor window.
    /// </summary>
    private void DrawCombatData()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Combat Data", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(damageProperty,
            new GUIContent("Damage (per projectile)"));
        EditorGUILayout.PropertyField(knockbackProperty,
            new GUIContent("Knockback", "Force applied to target struck by projectile."));
        EditorGUILayout.PropertyField(recoilProperty,
            new GUIContent("Recoil", "Opposite force applied to actor shooting the firearm."));
    }

    /// <summary>
    /// Draws Fire Effects parameters in the custom editor window.
    /// </summary>
    private void DrawFireEffects()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Fire Effects", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(fireAudioProperty);
        EditorGUILayout.PropertyField(useCameraShakeProperty);
        if (useCameraShakeProperty.boolValue)
        {
            EditorGUILayout.PropertyField(cameraShakeIntensityProperty);
            EditorGUILayout.PropertyField(cameraShakeTimeProperty);
        }
    }

    /// <summary>
    /// Draws FireStrategy parameters in the custom editor window.
    /// </summary>
    private void DrawFireStrategy()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Fire Strategy", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(fireModeProperty);
        EditorGUILayout.PropertyField(projectileTypeProperty);
        EditorGUILayout.PropertyField(projectilesPerShotProperty);
        EditorGUILayout.PropertyField(minTimeBetweenShotsProperty);
        EditorGUILayout.PropertyField(accuracyConeAngleProperty);

        EditorGUILayout.PropertyField(randomProjectileVelocityProperty);
        if (randomProjectileVelocityProperty.boolValue)
        {
            EditorGUILayout.PropertyField(projectileVelocityMinProperty);
            EditorGUILayout.PropertyField(projectileVelocityMaxProperty);
        }
        else
        {
            EditorGUILayout.PropertyField(projectileVelocityProperty);
        }

        EditorGUILayout.PropertyField(useProjectileLifetimeProperty);
        if (useProjectileLifetimeProperty.boolValue)
        {
            EditorGUILayout.PropertyField(projectileLifetimeProperty);
        }
    }
}
