using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(UIPanel))]
public class UIPanelInspector : Editor
{
    private UIPanel panel;

    private StringArrayPopupProperty showTriggerProperty;
    private StringArrayPopupProperty hideTriggerProperty;

    private void OnEnable()
    {
        panel = target as UIPanel;
        InitTriggerProperties();
    }
    private void InitTriggerProperties()
    {
        Animator animator = panel.GetComponent<Animator>();
        RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
        AnimatorController animatorController = runtimeAnimatorController as AnimatorController;

        if (animatorController == null)
        {
            AnimatorOverrideController overrideController = runtimeAnimatorController as AnimatorOverrideController;
            if (overrideController != null)
                animatorController = overrideController.runtimeAnimatorController as AnimatorController;
        }

        List<string> trigerList = new();

        if (animatorController != null)
        {
            foreach (var parameter in animatorController.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                    trigerList.Add(parameter.name);
            }
        }

        string[] triggerArray = trigerList.ToArray();

        showTriggerProperty = new StringArrayPopupProperty(triggerArray, panel.ShowTriggerName);
        hideTriggerProperty = new StringArrayPopupProperty(triggerArray, panel.HideTriggerName);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        panel.ShowTriggerName = showTriggerProperty.DrawLayout("Show Trigger");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("showSpeed"));

        EditorGUILayout.Space();

        panel.HideTriggerName = hideTriggerProperty.DrawLayout("Hide Trigger");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hideSpeed"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("editMode"));
        EditorGUILayout.Space();

        var eventsShowProperty = serializedObject.FindProperty("eventsShow");
        eventsShowProperty.boolValue = UIDraw.DrawOpenerHeader("Events", eventsShowProperty.boolValue);

        if (eventsShowProperty.boolValue)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onBeginShow"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onEndShow"));
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
