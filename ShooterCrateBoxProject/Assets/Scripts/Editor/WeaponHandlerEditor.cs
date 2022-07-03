using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom Editor for the WeaponHandler class.
/// </summary>
[CustomEditor(typeof(WeaponHandler))]
[CanEditMultipleObjects]
public class WeaponHandlerEditor : Editor
{
    // Current Firearm Property
    private SerializedProperty currentFirearmProperty;

    // Component References Properties
    private SerializedProperty projectilePoolRootProperty;
    private SerializedProperty projectileStartProperty;
    private SerializedProperty rb2DProperty;
    private SerializedProperty weaponRendererProperty;

    // ScriptableObject Reference Properties
    private SerializedProperty cameraShakeIntensityProperty;
    private SerializedProperty cameraShakeTimeProperty;
    private SerializedProperty recoilEventProperty;
    private SerializedProperty weaponFireEventProperty;
    private SerializedProperty weaponFireClipProperty;
    private SerializedProperty weaponStringProperty;

    // Editor control parameters
    private static bool showTesting = false;

    #region Editor Methods
    private void OnEnable()
    {
        // Find Current Firearm Property
        currentFirearmProperty = serializedObject.FindProperty("currentFirearm");

        // Find Component Reference Properties
        projectilePoolRootProperty = serializedObject.FindProperty("projectilePoolRoot");
        projectileStartProperty = serializedObject.FindProperty("projectileStart");
        rb2DProperty = serializedObject.FindProperty("rb2D");
        weaponRendererProperty = serializedObject.FindProperty("weaponRenderer");

        // Find ScriptableObject Reference Properties
        weaponFireEventProperty = serializedObject.FindProperty("weaponFireEvent");
        recoilEventProperty = serializedObject.FindProperty("recoilEvent");
        cameraShakeIntensityProperty = serializedObject.FindProperty("cameraShakeIntensity");
        cameraShakeTimeProperty = serializedObject.FindProperty("cameraShakeTime");
        weaponFireClipProperty = serializedObject.FindProperty("weaponFireClip");
        weaponStringProperty = serializedObject.FindProperty("weaponString");
    }

    public override void OnInspectorGUI()
    {
        WeaponHandler weaponHandler = target as WeaponHandler;

        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script",
            MonoScript.FromMonoBehaviour((MonoBehaviour)target),
            GetType(), false);

        EditorGUILayout.PropertyField(currentFirearmProperty);
        DrawComponentReferences();
        DrawScriptableObjectReferences();

        EditorGUILayout.Space();
        showTesting = EditorGUILayout.Foldout(showTesting,
            "Editor Testing");
        if (showTesting)
        {
            DrawTesting(weaponHandler);
        }

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Draws the Testing section of the custom editor window.
    /// </summary>
    /// <param name="weaponHandler">WeaponHandler upon which the testing 
    /// controls will act.</param>
    private void DrawTesting(WeaponHandler weaponHandler)
    {
        EditorGUILayout.LabelField("Use this button to cycle through available firearms.");
        if (GUILayout.Button("Next Firearm"))
        {
            List<Firearm> firearms = new List<Firearm>();

            string[] guids = AssetDatabase.FindAssets("t:Firearm",
                new string[] { "Assets/Game/ScriptableObjects/Combat/Firearm/"});

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Firearm firearm = AssetDatabase.LoadAssetAtPath<Firearm>(path);
                firearms.Add(firearm);
            }

            int currentIndex =
                firearms.IndexOf((Firearm)currentFirearmProperty.objectReferenceValue);
            int nextIndex = currentIndex + 1;
            if (nextIndex > firearms.Count - 1)
            {
                nextIndex = 0;
            }

            if (Application.isPlaying)
            {
                weaponHandler.CurrentFirearm = firearms[nextIndex];
            }
            else
            {
                currentFirearmProperty.objectReferenceValue = firearms[nextIndex];
            }
        }
    }
    #endregion

    /// <summary>
    /// Draws the Component References section of the custom editor window.
    /// </summary>
    private void DrawComponentReferences()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Component References", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(projectilePoolRootProperty);
        EditorGUILayout.PropertyField(projectileStartProperty);
        EditorGUILayout.PropertyField(rb2DProperty);
        EditorGUILayout.PropertyField(weaponRendererProperty);
    }

    /// <summary>
    /// Draws the ScriptableObject References section of the custom editor 
    /// window.
    /// </summary>
    private void DrawScriptableObjectReferences()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("ScriptableObject References", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(weaponFireEventProperty);
        EditorGUILayout.PropertyField(recoilEventProperty);
        EditorGUILayout.PropertyField(cameraShakeIntensityProperty);
        EditorGUILayout.PropertyField(cameraShakeTimeProperty);
        EditorGUILayout.PropertyField(weaponFireClipProperty);
        EditorGUILayout.PropertyField(weaponStringProperty);
    }
}
