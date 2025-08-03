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
    private SerializedProperty sprayCurveTypeProperty;
    private SerializedProperty sprayCurveProperty;

    private SerializedProperty moveDurationProperty;
    private SerializedProperty springDirectionProperty;
    private SerializedProperty springCurveTypeProperty;
    private SerializedProperty springCurveProperty;
    private SerializedProperty minSpringAmplitudeProperty;
    private SerializedProperty maxSpringAmplitudeProperty;
    private SerializedProperty moveCurveTypeProperty;
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
        sprayCurveTypeProperty = serializedObject.FindProperty("sprayCurveType");
        sprayCurveProperty = serializedObject.FindProperty("sprayCurve");

        moveDurationProperty = serializedObject.FindProperty("moveDuration");
        springDirectionProperty = serializedObject.FindProperty("springDirection");
        springCurveTypeProperty = serializedObject.FindProperty("springCurveType");
        springCurveProperty = serializedObject.FindProperty("springCurve");
        minSpringAmplitudeProperty = serializedObject.FindProperty("minSpringAmplitude");
        maxSpringAmplitudeProperty = serializedObject.FindProperty("maxSpringAmplitude");
        moveCurveTypeProperty = serializedObject.FindProperty("moveCurveType");
        moveCurveProperty = serializedObject.FindProperty("moveCurve");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawSpawnSettings();
        DrawSpraySettings();
        DrawMoveSettings();

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void DrawSpawnSettings()
    {
        EditorGUILayout.PropertyField(itemPrefabProperty);
        EditorGUILayout.PropertyField(maxSpawnCountProperty);
        EditorGUILayout.PropertyField(spawnDurationProperty);
        EditorGUILayout.PropertyField(spawnRadiusProperty);
    }

    private void DrawSpraySettings()
    {
        EditorGUILayout.PropertyField(sprayDirectionProperty);

        if (spawner.SprayDirection != ItemParticleSpawner.SprayDirections.None)
        {
            EditorGUILayout.PropertyField(sprayDurationProperty);
            DrawMinMaxProperty("Spray Radius", minSprayRadiusProperty, maxSprayRadiusProperty);

            if(spawner.SprayDirection != ItemParticleSpawner.SprayDirections.Random)
            EditorGUILayout.PropertyField(sprayConeAngleProperty);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sprayCurveTypeProperty);
            if (EditorGUI.EndChangeCheck() || sprayCurveProperty.animationCurveValue.keys.Length == 0)
                sprayCurveProperty.animationCurveValue = AnimationCurveCreator.Create
                     ((CurveType)sprayCurveTypeProperty.enumValueIndex);

            EditorGUILayout.PropertyField(sprayCurveProperty);
        }
    }

    private void DrawMoveSettings()
    {
        EditorGUILayout.PropertyField(moveDurationProperty);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(moveCurveTypeProperty);
        if (EditorGUI.EndChangeCheck() || moveCurveProperty.animationCurveValue.keys.Length == 0)
            moveCurveProperty.animationCurveValue = AnimationCurveCreator.Create
                 ((CurveType)moveCurveTypeProperty.enumValueIndex);
        EditorGUILayout.PropertyField(moveCurveProperty);
        EditorGUILayout.PropertyField(springDirectionProperty);

        if (spawner.SpringDirection != ItemParticleSpawner.SpringDirections.None)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(springCurveTypeProperty);
            if (EditorGUI.EndChangeCheck() || springCurveProperty.animationCurveValue.keys.Length == 0)
                springCurveProperty.animationCurveValue = AnimationCurveCreator.Create
                     ((CurveType)springCurveTypeProperty.enumValueIndex);

            EditorGUILayout.PropertyField(springCurveProperty);
            DrawMinMaxProperty("Spring Amplitude", minSpringAmplitudeProperty, maxSpringAmplitudeProperty);
        }
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
