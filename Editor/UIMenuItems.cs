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
        if(Selection.activeGameObject != null)
        {
            Selection.activeObject = Instantiate(Resources.Load("RoyoGames UI Elements/UI Panel"), Selection.activeGameObject.transform);
        }
    }
    [MenuItem("GameObject/Royo Games/UI/UIPopup", false, 0)]
    private static void AddUIPopup()
    {
        if (Selection.activeGameObject != null)
        {
            Selection.activeObject = Instantiate(Resources.Load("RoyoGames UI Elements/UI Popup"), Selection.activeGameObject.transform);
        }
    }
    [MenuItem("GameObject/Royo Games/UI/UIPointer", false, 0)]
    private static void AddUIPointer()
    {
        if (Selection.activeGameObject != null)
        {
            Selection.activeObject = Instantiate(Resources.Load("RoyoGames UI Elements/UI Pointer"), Selection.activeGameObject.transform);
        }
    }
    [MenuItem("GameObject/Royo Games/UI/AnimatedButton", false, 0)]
    private static void AddAnmatedButton()
    {
        if (Selection.activeGameObject != null)
        {
            Selection.activeObject = Instantiate(Resources.Load("RoyoGames UI Elements/Animated Button"), Selection.activeGameObject.transform);
        }
    }
    [MenuItem("GameObject/Royo Games/UI/AnimatedToggle", false, 0)]
    private static void AddAnmatedToggle()
    {
        if (Selection.activeGameObject != null)
        {
            Selection.activeObject = Instantiate(Resources.Load("RoyoGames UI Elements/Animated Toggle"), Selection.activeGameObject.transform);
        }
    }
    [MenuItem("GameObject/Royo Games/UI/Joystick", false, 0)]
    private static void AddJoystick()
    {
        if (Selection.activeGameObject != null)
        {
            Selection.activeObject = Instantiate(Resources.Load("RoyoGames UI Elements/Joystick"), Selection.activeGameObject.transform);
        }
    }
    [MenuItem("GameObject/Royo Games/UI/DragPad", false, 0)]
    private static void AddDragPad()
    {
        if (Selection.activeGameObject != null)
        {
            Selection.activeObject = Instantiate(Resources.Load("RoyoGames UI Elements/Drag Pad"), Selection.activeGameObject.transform);
        }
    }
    private static void AddComponenetsSelectedObjects<T>() where T : Component
    {
        foreach (var item in Selection.objects)
        {
            if(item is GameObject)
            {
                (item as GameObject).AddComponent<T>();
            }
        }
    }
}
