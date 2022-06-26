using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor class for the Mover2D object. Allows designers to design and
/// test movement behaviour with a variety of tools.
/// </summary>
[CustomEditor(typeof(Mover2D))]
[CanEditMultipleObjects]
public class Mover2DEditor : Editor
{
    /// <summary>
    /// Represents the Mover2D's acceleration property.
    /// </summary>
    private SerializedProperty accelerationProperty;

    /// <summary>
    /// Represents the Mover2D's current X velocity.
    /// </summary>
    private SerializedProperty currentVelocityXProperty;

    /// <summary>
    /// Represents the Mover2D's deceleration property.
    /// </summary>
    private SerializedProperty decelerationProperty;

    /// <summary>
    /// Represents the Mover2D's maxVelocity property.
    /// </summary>
    private SerializedProperty maxVelocityXProperty;

    /// <summary>
    /// Represents the Mover2D's naturalDecelerationX property;
    /// </summary>
    private SerializedProperty naturalDecelerationXProperty;

    /// <summary>
    /// Represents the Mover2D's rb2D property.
    /// </summary>
    private SerializedProperty rb2DProperty;

    /// <summary>
    /// Represents the Mover2D's useAcceleration property.
    /// </summary>
    private SerializedProperty useAccelerationProperty;

    /// <summary>
    /// Represents the Mover2D's useDeceleration property.
    /// </summary>
    private SerializedProperty useDecelerationProperty;

    /// <summary>
    /// Represents the Mover2D's wallSensorLeft property.
    /// </summary>
    private SerializedProperty wallSensorLeftProperty;

    /// <summary>
    /// Represents the Mover2D's wallSensorRight property.
    /// </summary>
    private SerializedProperty wallSensorRightProperty;

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
        accelerationProperty = serializedObject.FindProperty("acceleration");

        currentVelocityXProperty = 
            serializedObject.FindProperty("currentVelocityX");

        decelerationProperty = serializedObject.FindProperty("deceleration");
        maxVelocityXProperty = serializedObject.FindProperty("maxVelocityX");

        naturalDecelerationXProperty = 
            serializedObject.FindProperty("naturalDecelerationX");

        rb2DProperty = serializedObject.FindProperty("rb2D");
        useAccelerationProperty = 
            serializedObject.FindProperty("useAcceleration");

        useDecelerationProperty = 
            serializedObject.FindProperty("useDeceleration");

        wallSensorLeftProperty = 
            serializedObject.FindProperty("wallSensorLeft");

        wallSensorRightProperty = 
            serializedObject.FindProperty("wallSensorRight");
    }

    public override void OnInspectorGUI()
    {
        Mover2D mover2D = target as Mover2D;

        // Displays the script name at the top of the inspector GUI, which
        // mimics Unity-standard format.
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script",
            MonoScript.FromMonoBehaviour((MonoBehaviour)target),
            GetType(), false);

        DrawComponentReferences();

        EditorGUILayout.Space();

        DrawVelocityParams();

        EditorGUILayout.Space();

        DrawAccelDecelParams();

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
            DrawTesting(mover2D);
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
    /// Draws the Acceleration/Deceleration Parameters section of the cutom 
    /// editor window.
    /// </summary>
    private void DrawAccelDecelParams()
    {
        EditorGUILayout.LabelField("Acceleration/Deceleration Parameters",
                    EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(naturalDecelerationXProperty,
            new GUIContent("Natural Deceleration X",
            "The rate of deceleration when the mover is disabled (i.e. after weapon recoil)."));

        EditorGUILayout.PropertyField(useAccelerationProperty,
            new GUIContent("Use Acceleration", 
            "Should this Mover2D approach max velocity over time?"));

        if (useAccelerationProperty.boolValue)
        {
            EditorGUILayout.PropertyField(accelerationProperty,
                new GUIContent("Acceleration", 
                "At which rate should this Mover2D approach max velocity?"));
        }

        EditorGUILayout.PropertyField(useDecelerationProperty,
            new GUIContent("Use Deceleration", 
            "Should this Mover2D approach a velocity of 0 over time?"));

        if (useDecelerationProperty.boolValue)
        {
            EditorGUILayout.PropertyField(decelerationProperty,
                new GUIContent("Deceleration", 
                "At which rate should this Mover2D approach a velocity of 0?"));
        }
    }

    /// <summary>
    /// Draws Component References section of the custom editor window.
    /// </summary>
    private void DrawComponentReferences()
    {
        EditorGUILayout.LabelField("Component References", 
            EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(rb2DProperty,
            new GUIContent("Rb 2D", 
            "Which Rigidbody2D should this Mover2D affect?"));

        EditorGUILayout.PropertyField(wallSensorLeftProperty,
            new GUIContent("Wall Sensor Left", 
            "Which Sensor2D should be responsible for sensing walls " +
            "to the left?"));

        EditorGUILayout.PropertyField(wallSensorRightProperty,
            new GUIContent("Wall Sensor Right", 
            "Which Sensor2D should be responsible for sensing walls " + 
            "to the right?"));
    }

    /// <summary>
    /// Draws the Info section of the custom editor window.
    /// </summary>
    private void DrawInfo()
    {
        // Current Velocity
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.PropertyField(currentVelocityXProperty);

        // Acceleration Data
        if (useAccelerationProperty.boolValue)
        {
            float accelTime = 
                maxVelocityXProperty.floatValue / 
                accelerationProperty.floatValue;
            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.FloatField("Acceleration Time", accelTime);

            float accelFrames = accelTime / Time.fixedDeltaTime;
            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.FloatField("Acceleration Frames (60fps)",
                UtilityMethods.StandardizeFrameCount(accelFrames, 60));
        }

        // Deceleration Data
        if (useDecelerationProperty.boolValue)
        {
            float decelTime = 
                maxVelocityXProperty.floatValue / 
                decelerationProperty.floatValue;
            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.FloatField("Deceleration Time", decelTime);

            float decelFrames = decelTime / Time.fixedDeltaTime;
            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.FloatField("Deceleration Frames (60fps)",
                UtilityMethods.StandardizeFrameCount(decelFrames, 60));
        }
    }

    /// <summary>
    /// Draws the Presets section of the custom editor window.
    /// </summary>
    private void DrawPresets()
    {
        EditorGUILayout.LabelField("Use these buttons to set movement " +
            "behaviour to the selected description.");

        if (GUILayout.Button(new GUIContent("Lightweight and Responsive",
            "i.e. Celeste")))
        {
            maxVelocityXProperty.floatValue = 5.625f;
            useAccelerationProperty.boolValue = true;
            accelerationProperty.floatValue = 56.25f;
            useDecelerationProperty.boolValue = true;
            decelerationProperty.floatValue = 112.5f;
        }

        if (GUILayout.Button(new GUIContent("Heavyweight and Fast",
            "i.e. Super Meat Boy")))
        {
            maxVelocityXProperty.floatValue = 6.25f;
            useAccelerationProperty.boolValue = true;
            accelerationProperty.floatValue = 15.625f;
            useDecelerationProperty.boolValue = true;
            decelerationProperty.floatValue = 125.0f;
        }

        if (GUILayout.Button(new GUIContent("Stiff and Robotic",
            "i.e. Mega Man 11")))
        {
            maxVelocityXProperty.floatValue = 6.25f;
            useAccelerationProperty.boolValue = true;
            accelerationProperty.floatValue = 250.0f;
            useDecelerationProperty.boolValue = true;
            decelerationProperty.floatValue = 250.0f;
        }

        if (GUILayout.Button(new GUIContent("Midweight and Slippery",
            "i.e Super Mario Bros. 3")))
        {
            maxVelocityXProperty.floatValue = 6.25f;
            useAccelerationProperty.boolValue = true;
            accelerationProperty.floatValue = 27.77777f;
            useDecelerationProperty.boolValue = true;
            decelerationProperty.floatValue = 18.5185f;
        }
    }

    /// <summary>
    /// Draws the Testing section of the custom editor window.
    /// </summary>
    private static void DrawTesting(Mover2D mover2D)
    {
        EditorGUILayout.LabelField("While in Play Mode, use these " +
            "buttons to test movement behaviour.");
        if (GUILayout.Button("Set Move Input to 1"))
        {
            if (Application.isPlaying)
            {
                mover2D.MoveInput = 1;
            }
        }
        if (GUILayout.Button("Set Move Input to -1"))
        {
            if (Application.isPlaying)
            {
                mover2D.MoveInput = -1;
            }
        }
        if (GUILayout.Button("Set Move Input to 0"))
        {
            if (Application.isPlaying)
            {
                mover2D.MoveInput = 0;
            }
        }
    }

    /// <summary>
    /// Draws Velocity Parameters section of the custom editor window.
    /// </summary>
    private void DrawVelocityParams()
    {
        EditorGUILayout.LabelField("Velocity Parameters", 
            EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(maxVelocityXProperty,
            new GUIContent("Max Velocity X", 
            "What is the fastest velocity at which this Mover2D can travel?"));
    }
}

