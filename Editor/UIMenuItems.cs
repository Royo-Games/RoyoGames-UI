using UnityEditor;
using UnityEngine;

public class UIMenuItems : MonoBehaviour
{
    [MenuItem("Component/Royo Games/UI/UIPanel", true, 0)]
    [MenuItem("Component/Royo Games/UI/ToggleAnimation", true, 0)]
    [MenuItem("Component/Royo Games/UI/ScrollRectSnap", true, 0)]
    [MenuItem("Component/Royo Games/UI/ScrollRectSnapItem", true, 0)]
    [MenuItem("Component/Royo Games/UI/ScrollRectSnapScaleEffect", true, 0)]
    private static bool ValidateSelectedGameObject()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("Component/Royo Games/UI/UIPanel",false, 0)]
    private static void AddUIPanel()
    {
        if(Selection.activeGameObject != null)
        {
            AddComponenetsSelectedObjects<UIPanel>(); 
        }
    }
    [MenuItem("Component/Royo Games/UI/ToggleAnimation", false, 0)]
    private static void AddToggleAnimation()
    {
        if (Selection.activeGameObject != null)
        {
            AddComponenetsSelectedObjects<ToggleAnimation>();
        }
    }
    [MenuItem("Component/Royo Games/UI/ScrollRectSnap", false, 0)]
    private static void AddScrollRectSnap()
    {
        if (Selection.activeGameObject != null)
        {
            AddComponenetsSelectedObjects<ScrollRectSnap>();
        }
    }
    [MenuItem("Component/Royo Games/UI/ScrollRectSnapScaleEffect", false, 0)]
    private static void AddScrollRectSnapScaleEffect()
    {
        if (Selection.activeGameObject != null)
        {
            AddComponenetsSelectedObjects<ScrollRectSnapScaleEffect>();
        }
    }
    [MenuItem("Component/Royo Games/UI/SubScrollRect", false, 0)]
    private static void AddSubScrollRect()
    {
        if (Selection.activeGameObject != null)
        {
            AddComponenetsSelectedObjects<SubScrollRect>();
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
