using RoyoGames.UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemParticleSpawner), true)]
public class ItemParticleSpawnerInspector : Editor
{
    private ItemParticleSpawner spawner;
    private SerializedProperty itemPrefabProperty;
    private SerializedProperty maxSpawnCountProperty;
    private SerializedProperty spawnDurationProperty;
    private SerializedProperty spawnRadiusProperty;

    private SerializedProperty sprayDirectionProperty;
    private SerializedProperty sprayDurationProperty;
    private SerializedProperty minSprayRadiusProperty;
    private SerializedProperty maxSprayRadiusProperty;
    private SerializedProperty sprayConeAngleProperty;
    private SerializedProperty sprayCurveProperty;

    private SerializedProperty moveDurationProperty;
    private SerializedProperty springDirectionProperty;
    private SerializedProperty springCurveProperty;
    private SerializedProperty minSpringAmplitudeProperty;
    private SerializedProperty maxSpringAmplitudeProperty;
    private SerializedProperty moveCurveProperty;

    private void OnEnable()
    {
        spawner = target as ItemParticleSpawner;

        itemPrefabProperty = serializedObject.FindProperty("itemPrefab");
        maxSpawnCountProperty = serializedObject.FindProperty("maxSpawnCount");
        spawnDurationProperty = serializedObject.FindProperty("spawnDuration");
        spawnRadiusProperty = serializedObject.FindProperty("spawnRadius");

        sprayDirectionProperty = serializedObject.FindProperty("sprayDirection");
        sprayDurationProperty = serializedObject.FindProperty("sprayDuration");
        minSprayRadiusProperty = serializedObject.FindProperty("minSprayRadius");
        maxSprayRadiusProperty = serializedObject.FindProperty("maxSprayRadius");
        sprayConeAngleProperty = serializedObject.FindProperty("sprayConeAngle");
        sprayCurveProperty = serializedObject.FindProperty("sprayCurve");

        moveDurationProperty = serializedObject.FindProperty("moveDuration");
        springDirectionProperty = serializedObject.FindProperty("springDirection");
        springCurveProperty = serializedObject.FindProperty("springCurve");
        minSpringAmplitudeProperty = serializedObject.FindProperty("minSpringAmplitude");
        maxSpringAmplitudeProperty = serializedObject.FindProperty("maxSpringAmplitude");
        moveCurveProperty = serializedObject.FindProperty("moveCurve");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(itemPrefabProperty);
        EditorGUILayout.PropertyField(maxSpawnCountProperty);
        EditorGUILayout.PropertyField(spawnDurationProperty);
        EditorGUILayout.PropertyField(spawnRadiusProperty);

        EditorGUILayout.PropertyField(sprayDirectionProperty);

        if (spawner.SprayDirection != ItemParticleSpawner.SprayDirections.None)
        {
            EditorGUILayout.PropertyField(sprayDurationProperty);
            DrawMinMaxProperty("Spray Radius", minSprayRadiusProperty, maxSprayRadiusProperty);
            EditorGUILayout.PropertyField(sprayConeAngleProperty);
            EditorGUILayout.PropertyField(sprayCurveProperty);
        }

        EditorGUILayout.PropertyField(moveDurationProperty);
        EditorGUILayout.PropertyField(springDirectionProperty);
        EditorGUILayout.PropertyField(springCurveProperty);
        DrawMinMaxProperty("Spring Amplitude", minSpringAmplitudeProperty, maxSpringAmplitudeProperty);
        EditorGUILayout.PropertyField(moveCurveProperty);

        if (GUI.changed)
            serializedObject.ApplyModifiedProperties();
    }

    private void DrawMinMaxProperty(string label, SerializedProperty minProperty, SerializedProperty maxProperty)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtility.labelWidth));
        float labelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 25;
        EditorGUILayout.PropertyField(minProperty, new GUIContent("Min"));
        EditorGUILayout.PropertyField(maxProperty, new GUIContent("Max"));
        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUILayout.EndHorizontal();
    }
}
