using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIPanel))]
public class UIPanelInspector : Editor
{
    private UIPanel panel;

    private void OnEnable()
    {
        panel = target as UIPanel;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("showTrigger"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("showSpeed"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hideTrigger"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hideSpeed"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enableEscapeHide"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("editMode"));
        EditorGUILayout.Space();

        var eventsShowProperty = serializedObject.FindProperty("eventsShow");
        eventsShowProperty.boolValue = UIDraw.DrawOpenerHeader("Events", eventsShowProperty.boolValue);

        if (eventsShowProperty.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onBeginOpen"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onEndOpen"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onBeginHide"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onEndHide"));
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(panel);
        }
    }
}
