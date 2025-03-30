using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIHud))]
public class UIHudElementInspector : Editor
{
    private UIHud _uiHudElement;

    private SerializedProperty _worldCameraProperty;
    private SerializedProperty _targetProperty;
    private SerializedProperty _targetOfsetProperty;
    private SerializedProperty _enableClampProperty;
    private SerializedProperty _clampPaddingProperty;
    private SerializedProperty _enableAutoScalePropert;
    private SerializedProperty _scaleFactorProperty;
    private SerializedProperty _minScaleProperty;

    private void OnEnable()
    {
        _uiHudElement = (UIHud)target;
        _worldCameraProperty = serializedObject.FindProperty("_worldCamera");
        _targetProperty = serializedObject.FindProperty("_target");
        _targetOfsetProperty = serializedObject.FindProperty("_targetOffset");
        _enableClampProperty = serializedObject.FindProperty("_enableClamp");
        _clampPaddingProperty = serializedObject.FindProperty("_clampPadding");
        _enableAutoScalePropert = serializedObject.FindProperty("_enableAutoScale");
        _scaleFactorProperty = serializedObject.FindProperty("_scaleFactor");
        _minScaleProperty = serializedObject.FindProperty("_minScale");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_worldCameraProperty);
        EditorGUILayout.PropertyField(_targetProperty);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_targetOfsetProperty);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_enableClampProperty);

        if (_uiHudElement.EnableClamp)
        {
            EditorGUILayout.PropertyField(_clampPaddingProperty);
            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(_enableAutoScalePropert);

        if (_uiHudElement.EnableAutoScale)
        {
            EditorGUILayout.PropertyField(_scaleFactorProperty);
            EditorGUILayout.PropertyField(_minScaleProperty);
        }

        if (GUI.changed)
            EditorUtility.SetDirty(_uiHudElement);

        serializedObject.ApplyModifiedProperties();
    }
}
