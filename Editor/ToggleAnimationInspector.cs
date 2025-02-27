using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(ToggleAnimation))]
public class ToggleAnimationInspector : Editor
{
    private ToggleAnimation toggleAnimation;
    private StringArrayPopupProperty onTriggerProperty;
    private StringArrayPopupProperty offTriggerProperty;

    private void OnEnable()
    {
        toggleAnimation = (ToggleAnimation)target;
        InitTriggerProperties();
    }
    private void InitTriggerProperties()
    {
        AnimatorController animatorController = toggleAnimation.GetComponent<Animator>().runtimeAnimatorController as AnimatorController;

        List<string> trigerList = new();

        foreach (var parameter in animatorController.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
                trigerList.Add(parameter.name);
        }

        string[] triggerArray = trigerList.ToArray();

        onTriggerProperty = new StringArrayPopupProperty(triggerArray, toggleAnimation.OnTrigger);
        offTriggerProperty = new StringArrayPopupProperty(triggerArray, toggleAnimation.OffTrigger);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        toggleAnimation.OnTrigger = onTriggerProperty.DrawLayout("On trigger");
        toggleAnimation.OffTrigger = offTriggerProperty.DrawLayout("Off trigger");

        if (GUI.changed)
            EditorUtility.SetDirty(target);

        serializedObject.ApplyModifiedProperties();
    }
}
