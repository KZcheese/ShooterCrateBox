using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Sensor2D))]
[CanEditMultipleObjects]
public class Sensor2DEditor : Editor
{
    private SerializedProperty activeProperty;
    private SerializedProperty layerToSenseProperty;
    private SerializedProperty radiusProperty;
    private SerializedProperty rectangleSizeProperty;
    private SerializedProperty sensorShapeProperty;
    private SerializedProperty senseTagProperty;
    private SerializedProperty tagToSenseProperty;
    private static bool showInfo = true;

    void OnEnable()
    {
        activeProperty = serializedObject.FindProperty("active");
        layerToSenseProperty = serializedObject.FindProperty("layerToSense");
        radiusProperty = serializedObject.FindProperty("radius");
        rectangleSizeProperty = serializedObject.FindProperty("rectangleSize");
        sensorShapeProperty = serializedObject.FindProperty("SensorShape");
        senseTagProperty = serializedObject.FindProperty("SenseTag");
        tagToSenseProperty = serializedObject.FindProperty("tagToSense");
    }

    public override void OnInspectorGUI()
    {
        Sensor2D sensor2D = target as Sensor2D;

        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", 
            MonoScript.FromMonoBehaviour((MonoBehaviour)target), 
            GetType(), false);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Geometry", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(sensorShapeProperty);

        switch (sensor2D.SensorShape)
        {
            case SensorShape.Circle:
                EditorGUILayout.PropertyField(radiusProperty);
                break;
            case SensorShape.Rectangle:
                EditorGUILayout.PropertyField(rectangleSizeProperty);
                break;
            default:
                break;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("What To Sense", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(layerToSenseProperty);
        EditorGUILayout.PropertyField(senseTagProperty);

        if (sensor2D.SenseTag)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("To avoid exceptions, only edit " +
                "the following parameter outside of Play Mode.");
            EditorGUILayout.PropertyField(tagToSenseProperty);
        }

        showInfo = EditorGUILayout.Foldout(showInfo, "Info");

        if (showInfo)
        {
            using (new EditorGUI.DisabledScope(true)) EditorGUILayout.PropertyField(activeProperty);
        }   

        serializedObject.ApplyModifiedProperties();
    }

}
