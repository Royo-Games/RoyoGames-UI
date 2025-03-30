using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIScrollRectSnap))]
public class ScrollRectSnapInspector : Editor
{
    private UIScrollRectSnap scrollRectSnap;

    private void OnEnable()
    {
        scrollRectSnap = target as UIScrollRectSnap;
        scrollRectSnap.UpdateItemList();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUI.changed)
        {
            scrollRectSnap.CenterTo();
        }
    }
}