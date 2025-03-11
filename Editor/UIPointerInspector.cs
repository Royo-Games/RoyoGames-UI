using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(UIPointer))]
public class UIPointerInspector : Editor
{
    private UIPointer pointer;

    private StringArrayPopupProperty showTriggerProperty;
    private StringArrayPopupProperty hideTriggerProperty;
    private StringArrayPopupProperty highlightTriggerProperty;

    private StringArrayPopupProperty beginDragTriggerProperty;
    private StringArrayPopupProperty endDragTriggerProperty;

    private void OnEnable()
    {
        pointer = (UIPointer)target;
        InitTriggerProperties();
    }
    private void InitTriggerProperties()
    {
        AnimatorController animatorController = pointer.GetComponent<Animator>().runtimeAnimatorController as AnimatorController;

        List<string> trigerList = new();

        foreach (var parameter in animatorController.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
                trigerList.Add(parameter.name);
        }

        string[] triggerArray = trigerList.ToArray();

        showTriggerProperty = new StringArrayPopupProperty(triggerArray, pointer.ShowTrigger);
        hideTriggerProperty = new StringArrayPopupProperty(triggerArray, pointer.HideTrigger);
        highlightTriggerProperty = new StringArrayPopupProperty(triggerArray, pointer.HighlightTrigger);

        beginDragTriggerProperty = new StringArrayPopupProperty(triggerArray, pointer.BeginDragTrigger);
        endDragTriggerProperty = new StringArrayPopupProperty(triggerArray, pointer.EndDragTrigger);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("flip"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("angle"));

        EditorGUILayout.Space();

        pointer.ShowTrigger = showTriggerProperty.DrawLayout("Show Trigger");
        pointer.HideTrigger = hideTriggerProperty.DrawLayout("Hide Trigger");
        pointer.HighlightTrigger = highlightTriggerProperty.DrawLayout("Highligh Trigger");

        EditorGUILayout.Space();
        pointer.BeginDragTrigger = beginDragTriggerProperty.DrawLayout("Begin Drag Trigger");
        pointer.EndDragTrigger = endDragTriggerProperty.DrawLayout("End Drag Trigger");

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startToTargetDuration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startToTargetAmplitude"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetToStartDuration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetToStartAmplitude"));

        if (GUI.changed)
        {
            EditorUtility.SetDirty(pointer);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
