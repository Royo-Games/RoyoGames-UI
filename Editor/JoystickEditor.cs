using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Joystick), true)]
public class JoystickEditor : Editor
{
    private SerializedProperty moveThreshold;
    private SerializedProperty joystickType;
    private SerializedProperty handleRange;
    private SerializedProperty axisOptions;
    protected SerializedProperty background;
    private SerializedProperty handle;
    private SerializedProperty autoHide;
    private SerializedProperty showDistance;

    protected Vector2 center = new Vector2(0.5f, 0.5f);

    protected virtual void OnEnable()
    {
        moveThreshold = serializedObject.FindProperty("_moveThreshold");
        joystickType = serializedObject.FindProperty("_joystickType");
        handleRange = serializedObject.FindProperty("_handleRange");
        axisOptions = serializedObject.FindProperty("_axisOptions");
        background = serializedObject.FindProperty("_background");
        handle = serializedObject.FindProperty("_handle");
        autoHide = serializedObject.FindProperty("_autoHide");
        showDistance = serializedObject.FindProperty("_visibleMagnitude");
    }
    public override void OnInspectorGUI()   
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        if(target.GetType() != typeof(Joystick))
        EditorGUILayout.Separator();

        DrawValues();
        EditorGUILayout.Space();
        DrawComponents();

        serializedObject.ApplyModifiedProperties();

        if (handle.objectReferenceValue != null)
        {
            RectTransform handleRect = (RectTransform)handle.objectReferenceValue;
            handleRect.anchorMax = center;
            handleRect.anchorMin = center;
            handleRect.pivot = center;
            handleRect.anchoredPosition = Vector2.zero;
        }

        if (handle.objectReferenceValue != null)
        {
            RectTransform backgroundRect = (RectTransform)handle.objectReferenceValue;
            backgroundRect.pivot = center;
        }
    }
    protected virtual void DrawValues()
    {
        EditorGUILayout.PropertyField(handleRange, new GUIContent("Handle Range", "The distance the visual handle can move from the center of the joystick."));
        EditorGUILayout.PropertyField(axisOptions, new GUIContent("Axis Options", "Which axes the joystick uses."));
        EditorGUILayout.PropertyField(joystickType, new GUIContent("Joystick Type", "The type of joystick the variable joystick is current using."));

        JoystickType type = (JoystickType)joystickType.enumValueIndex;

        switch (type)
        {
            case JoystickType.Floating:
            case JoystickType.Dynamic:

                if (type == JoystickType.Dynamic)
                    EditorGUILayout.PropertyField(moveThreshold, new GUIContent("Move Threshold", "The distance away from the center input has to be before the joystick begins to move."));

                EditorGUILayout.PropertyField(autoHide, new GUIContent("Auto Hide"));

                if(autoHide.boolValue)
                    EditorGUILayout.PropertyField(showDistance, new GUIContent("Visible Magnitude"));

                break;
        }
    }
    protected virtual void DrawComponents()
    {
        EditorGUILayout.ObjectField(background, new GUIContent("Background", "The background's RectTransform component."));
        EditorGUILayout.ObjectField(handle, new GUIContent("Handle", "The handle's RectTransform component."));
    }
}