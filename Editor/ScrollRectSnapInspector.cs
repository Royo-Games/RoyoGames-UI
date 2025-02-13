using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScrollRectSnap))]
public class ScrollRectSnapInspector : Editor
{
    private ScrollRectSnap scrollRectSnap;

    private void OnEnable()
    {
        scrollRectSnap = target as ScrollRectSnap;
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