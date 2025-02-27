using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(ButtonAnimation))]
public class ButtonAnimationInspector : Editor
{
    private ButtonAnimation buttonAnimation;
    private StringArrayPopupProperty downTriggerProperty;
    private StringArrayPopupProperty upTriggerProperty;

    private void OnEnable()
    {
        buttonAnimation = (ButtonAnimation)target;
        InitTriggerProperties();
    }
    private void InitTriggerProperties()
    {
        AnimatorController animatorController = buttonAnimation.GetComponent<Animator>().runtimeAnimatorController as AnimatorController;

        List<string> trigerList = new();

        foreach (var parameter in animatorController.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
                trigerList.Add(parameter.name);
        }

        string[] triggerArray = trigerList.ToArray();

        downTriggerProperty = new StringArrayPopupProperty(triggerArray, buttonAnimation.DownTrigger);
        upTriggerProperty = new StringArrayPopupProperty(triggerArray, buttonAnimation.UpTrigger);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        buttonAnimation.DownTrigger = downTriggerProperty.DrawLayout("Down trigger");
        buttonAnimation.UpTrigger = upTriggerProperty.DrawLayout("Up trigger");

        if(GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
