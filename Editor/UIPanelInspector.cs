using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIPanel))]
public class UIPanelInspector : Editor
{
    private UIPanel panel;

    private SerializedProperty showAnimationProperty;
    private SerializedProperty hideAnimationProperty;
    private SerializedProperty editModeProperty;
    private SerializedProperty eventsShowProperty;
    private SerializedProperty onBeginShowProperty;
    private SerializedProperty onEndShowProperty;
    private SerializedProperty onBeginHideProperty;
    private SerializedProperty onEndHideProperty;

    private void OnEnable()
    {
        panel = target as UIPanel;
        showAnimationProperty = serializedObject.FindProperty("showAnimation");
        hideAnimationProperty = serializedObject.FindProperty("hideAnimation");
        editModeProperty = serializedObject.FindProperty("editMode");
        eventsShowProperty = serializedObject.FindProperty("eventsShow");
        onBeginShowProperty = serializedObject.FindProperty("onBeginShow");
        onEndShowProperty = serializedObject.FindProperty("onEndShow");
        onBeginHideProperty = serializedObject.FindProperty("onBeginHide");
        onEndHideProperty = serializedObject.FindProperty("onEndHide");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(showAnimationProperty);
        EditorGUILayout.PropertyField(hideAnimationProperty);


        EditorGUI.BeginChangeCheck();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(editModeProperty);
        EditorGUILayout.Space();

        eventsShowProperty.boolValue = UIDraw.DrawOpenerHeader("Events", eventsShowProperty.boolValue);

        if (eventsShowProperty.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(onBeginShowProperty);
            EditorGUILayout.PropertyField(onEndShowProperty);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(onBeginHideProperty);
            EditorGUILayout.PropertyField(onEndHideProperty);
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(panel);
        }
    }
}
