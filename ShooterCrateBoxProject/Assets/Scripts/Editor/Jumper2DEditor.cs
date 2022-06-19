using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor class for the Jumper2D object. Allows designers to design and
/// test jumping behaviour with a variety of tools.
/// </summary>
[CustomEditor(typeof(Jumper2D))]
[CanEditMultipleObjects]
public class Jumper2DEditor : Editor
{
    /// <summary>
    /// Represents the Jumper2D's cancelRate property.
    /// </summary>
    private SerializedProperty cancelRateProperty;

    /// <summary>
    /// Represents the Jumper2D's gravityScale property.
    /// </summary>
    private SerializedProperty gravityScaleProperty;

    /// <summary>
    /// Sensor2D responsible for checking the grounded state of the object.
    /// </summary>
    private SerializedProperty groundSensorProperty;

    /// <summary>
    /// Sensor2D responsible for checking collisions above the player.
    /// </summary>
    private SerializedProperty headSensorProperty;

    /// <summary>
    /// Represents the Jumper2D's jumpCount property.
    /// </summary>
    private SerializedProperty jumpCountProperty;

    /// <summary>
    /// Represents the Jumper2D's jumpWindowTime property.
    /// </summary>
    private SerializedProperty jumpWindowTimeProperty;

    /// <summary>
    /// Represents the Jumper2D's maxJumpHeight property.
    /// </summary>
    private SerializedProperty maxJumpHeightProperty;

    /// <summary>
    /// Represents the Jumper2D's multiJump property.
    /// </summary>
    private SerializedProperty multiJumpProperty;

    /// <summary>
    /// Represents the Jumper2D's rb2D property.
    /// </summary>
    private SerializedProperty rb2DProperty;

    /// <summary>
    /// Represents the Jumper2D's totalJumps property.
    /// </summary>
    private SerializedProperty totalJumpsProperty;

    /// <summary>
    /// If true, final jump height is higher the longer the player holds down 
    /// jump input, clamped at maxJumpHeight. If false, player always jumps to
    /// maxJumpHeight when jump input is pressed.
    /// </summary>
    private SerializedProperty variableJumpProperty;

    /// <summary>
    /// Should info be displayed in the editor?
    /// </summary>
    private static bool showInfo = true;

    /// <summary>
    /// Should jump parameter presets be displayed in the editor?
    /// </summary>
    private static bool showPresets = false;

    /// <summary>
    /// Should testing controls be displayed in the editor?
    /// </summary>
    private static bool showTesting = false;

    #region Editor Methods
    private void OnEnable()
    {
        cancelRateProperty = serializedObject.FindProperty("cancelRate");
        gravityScaleProperty = serializedObject.FindProperty("gravityScale");
        groundSensorProperty = serializedObject.FindProperty("groundSensor");
        headSensorProperty = serializedObject.FindProperty("headSensor");
        jumpCountProperty = serializedObject.FindProperty("jumpCount");
        jumpWindowTimeProperty = serializedObject.FindProperty("jumpWindowTime");
        maxJumpHeightProperty = serializedObject.FindProperty("maxJumpHeight");
        multiJumpProperty = serializedObject.FindProperty("multiJump");
        rb2DProperty = serializedObject.FindProperty("rb2D");
        totalJumpsProperty = serializedObject.FindProperty("totalJumps");
        variableJumpProperty = serializedObject.FindProperty("variableJump");
    }

    public override void OnInspectorGUI()
    {
        Jumper2D jumper2D = target as Jumper2D;

        // Displays the script name at the top of the inspector GUI, which
        // mimics Unity-standard format.
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", 
            MonoScript.FromMonoBehaviour((MonoBehaviour)target), 
            GetType(), false);

        DrawComponentReferences();

        EditorGUILayout.Space();
        DrawJumpParams();

        if (multiJumpProperty.boolValue)
        {
            DrawMultiJumpParams();
        }

        if (variableJumpProperty.boolValue)
        {
            DrawVariableJumpParams();
        }

        EditorGUILayout.Space();
        showPresets = EditorGUILayout.Foldout(showPresets, "Presets");
        if (showPresets)
        {
            DrawPresets();
        }

        showTesting = EditorGUILayout.Foldout(showTesting,
            "Editor Testing");
        if (showTesting)
        {
            DrawTesting(jumper2D);
        }

        showInfo = EditorGUILayout.Foldout(showInfo, "Info");
        if (showInfo)
        {
            DrawInfo();
        }

        serializedObject.ApplyModifiedProperties();
    }
    #endregion

    /// <summary>
    /// Draws Component References section of the custom editor window.
    /// </summary>
    private void DrawComponentReferences()
    {
        EditorGUILayout.LabelField("Component References",
            EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(rb2DProperty,
            new GUIContent("Rb 2D",
            "Which Rigidbody2D should this Jumper2D affect?"));

        EditorGUILayout.PropertyField(groundSensorProperty,
            new GUIContent("Ground Sensor",
            "Which Sensor2D should be used to check the grounded state?"));

        EditorGUILayout.PropertyField(headSensorProperty,
            new GUIContent("Head Sensor",
            "Which Sensor2D should be used to check if the Jumper2D has hit " +
            "a ceiling?"));
    }

    /// <summary>
    /// Draws the Info section of the custom editor window.
    /// </summary>
    private void DrawInfo()
    {
        // Jump Hang Time
        float gravity = Physics2D.gravity.y * gravityScaleProperty.floatValue;
        float totalJumpTime = 2 *
            Mathf.Sqrt((-2 * maxJumpHeightProperty.floatValue) / gravity);
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.FloatField("Total Jump Time", totalJumpTime);

        // Jump Frames
        float jumpFrames = totalJumpTime / Time.fixedDeltaTime;
        float standardJumpFrames = UtilityMethods.StandardizeFrameCount(jumpFrames, 60);
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.FloatField("Total Jump Frames (60fps)", standardJumpFrames);

        // Remaining Jumps
        if (multiJumpProperty.boolValue)
        {
            int remainingJumps =
                totalJumpsProperty.intValue - jumpCountProperty.intValue;
            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.FloatField("Jumps Remaining", remainingJumps);
        }
    }

    /// <summary>
    /// Draws Jump Parameters section of the custom editor window.
    /// </summary>
    private void DrawJumpParams()
    {
        EditorGUILayout.LabelField("Jump Parameters", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(maxJumpHeightProperty,
            new GUIContent("Max Jump Height", 
            "How high should it be allowed to jump?"));

        EditorGUILayout.PropertyField(gravityScaleProperty,
            new GUIContent("Gravity Scale", 
            "How many times should gravity act per frame?"));
            
        EditorGUILayout.PropertyField(multiJumpProperty,
            new GUIContent("Multi Jump", "Can it jump while in the air?"));

        EditorGUILayout.PropertyField(variableJumpProperty,
            new GUIContent("Variable Jump", 
            "Should the jump cancel if input is released?"));
    }

    /// <summary>
    /// Draws Multi Jump Parameters section of the custom editor window.
    /// </summary>
    private void DrawMultiJumpParams()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Multi Jump Parameters", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(totalJumpsProperty,
            new GUIContent("Total Jumps", "How many times can it jump?"));
    }

    /// <summary>
    /// Draws the Presets section of the custom editor window.
    /// </summary>
    private void DrawPresets()
    {
        EditorGUILayout.LabelField("Use these buttons to set jump " +
                    "behaviour to the selected description.");

        if (GUILayout.Button(new GUIContent("Midweight and Precise", 
            "i.e Celeste")))
        {
            maxJumpHeightProperty.floatValue = 2.0f;
            gravityScaleProperty.floatValue = 4.53051f;
            multiJumpProperty.boolValue = false;
            variableJumpProperty.boolValue = false;
        }
        if (GUILayout.Button(new GUIContent("Lightweight and Floaty", 
            "i.e. Super Meat Boy")))
        {
            maxJumpHeightProperty.floatValue = 5.0f;
            gravityScaleProperty.floatValue = 4.075f;
            multiJumpProperty.boolValue = false;
            variableJumpProperty.boolValue = true;
            cancelRateProperty.floatValue = 50.0f;

        }
        if (GUILayout.Button(new GUIContent("Heavy and Leaden", 
            "i.e. Unravel")))
        {
            maxJumpHeightProperty.floatValue = 1.5f;
            gravityScaleProperty.floatValue = 3.8f;
            multiJumpProperty.boolValue = false;
            variableJumpProperty.boolValue = false;
        }
    }

    /// <summary>
    /// Draws Testing section of the custom editor window.
    /// </summary>
    /// <param name="jumper2D">Jumper2D to test.</param> 
    private static void DrawTesting(Jumper2D jumper2D)
    {
        EditorGUILayout.LabelField("While in Play Mode, use these buttons " + 
            "to test jump behaviour.");
        EditorGUILayout.LabelField("Best to keep jumpWindowTime >= 1 for " + 
            "editor testing.");

        if (GUILayout.Button("Jump"))
        {
            if (Application.isPlaying)
            {
                jumper2D.Jump();
            }
        }
        if (GUILayout.Button("Stop Variable Jump"))
        {
            if (Application.isPlaying)
            {
                jumper2D.StopVariableJump();
            }
        }
    }

    /// <summary>
    /// Draws Variable Jump Parameters section of the custom editor window.
    /// </summary>
    private void DrawVariableJumpParams()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Variable Jump Parameters", 
            EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(cancelRateProperty,
            new GUIContent("Cancel Rate", "How quickly should the jump " + 
            "cancel? If set to 0, jump will cancel instantly."));

        EditorGUILayout.PropertyField(jumpWindowTimeProperty,
            new GUIContent("Jump Window Time", "How long before it stops " + 
            "checking for a released jump input?"));
    }
}
