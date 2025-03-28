using UnityEditor;
using UnityEngine;

public class UIMenuItems : MonoBehaviour
{
    [MenuItem("GameObject/Royo Games/UI/UIPopup", true, 0)]
    [MenuItem("GameObject/Royo Games/UI/UIPanel", true, 0)]
    [MenuItem("GameObject/Royo Games/UI/UIPointer", true, 0)]
    [MenuItem("GameObject/Royo Games/UI/AnimatedButton", true, 0)]
    [MenuItem("GameObject/Royo Games/UI/AnimatedToggle", true, 0)]
    [MenuItem("GameObject/Royo Games/UI/Joystick", true, 0)]
    [MenuItem("GameObject/Royo Games/UI/DragPad", true, 0)]
    private static bool ValidateSelectedGameObject()
    {
        return Selection.activeGameObject != null;
    }
    [MenuItem("GameObject/Royo Games/UI/UIPanel", false, 0)]
    private static void AddUIPanel()
    {
        if (Selection.activeGameObject != null)
        {
            GameObject uiPanel = Instantiate(Resources.Load<GameObject>("RoyoGames UI Elements/UI Panel"), Selection.activeGameObject.transform);
            uiPanel.name = "UI Panel";
            Selection.activeObject = uiPanel;
        }
    }
    [MenuItem("GameObject/Royo Games/UI/UIPopup", false, 0)]
    private static void AddUIPopup()
    {
        if (Selection.activeGameObject != null)
        {
            GameObject uiPopup = Instantiate(Resources.Load<GameObject>("RoyoGames UI Elements/UI Popup"), Selection.activeGameObject.transform);
            uiPopup.name = "UI Popup";
            Selection.activeObject = uiPopup;
        }
    }
    [MenuItem("GameObject/Royo Games/UI/UIPointer", false, 0)]
    private static void AddUIPointer()
    {
        if (Selection.activeGameObject != null)
        {
            GameObject uiPointer = Instantiate(Resources.Load<GameObject>("RoyoGames UI Elements/UI Pointer"), Selection.activeGameObject.transform);
            uiPointer.name = "UI Pointer";
            Selection.activeObject = uiPointer;
        }
    }
    [MenuItem("GameObject/Royo Games/UI/AnimatedButton", false, 0)]
    private static void AddAnmatedButton()
    {
        if (Selection.activeGameObject != null)
        {
            GameObject animatedButton = Instantiate(Resources.Load<GameObject>("RoyoGames UI Elements/Animated Button"), Selection.activeGameObject.transform);
            animatedButton.name = "Animated Button";
            Selection.activeObject = animatedButton;
        }
    }
    [MenuItem("GameObject/Royo Games/UI/AnimatedToggle", false, 0)]
    private static void AddAnmatedToggle()
    {
        if (Selection.activeGameObject != null)
        {
            GameObject animatedToggle = Instantiate(Resources.Load<GameObject>("RoyoGames UI Elements/Animated Toggle"), Selection.activeGameObject.transform);
            animatedToggle.name = "Animated Toggle";
            Selection.activeObject = animatedToggle;
        }
    }
    [MenuItem("GameObject/Royo Games/UI/Joystick", false, 0)]
    private static void AddJoystick()
    {
        if (Selection.activeGameObject != null)
        {
            GameObject joystick = Instantiate(Resources.Load<GameObject>("RoyoGames UI Elements/Joystick"), Selection.activeGameObject.transform);
            joystick.name = "Joystick";
            Selection.activeObject = joystick;
        }
    }
    [MenuItem("GameObject/Royo Games/UI/DragPad", false, 0)]
    private static void AddDragPad()
    {
        if (Selection.activeGameObject != null)
        {
            GameObject dragPad = Instantiate(Resources.Load<GameObject>("RoyoGames UI Elements/Drag Pad"), Selection.activeGameObject.transform);
            dragPad.name = "Drag Pad";
            Selection.activeObject = dragPad;
        }
    }
    private static void AddComponenetsSelectedObjects<T>() where T : Component
    {
        foreach (var item in Selection.objects)
        {
            if (item is GameObject)
            {
                (item as GameObject).AddComponent<T>();
            }
        }
    }
}
