using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightUIObjectExample : MonoBehaviour
{
    public GameObject highlightObject;
    public UIPointer pointer;

    public void StartHighligt()
    {
        pointer.transform.position = highlightObject.transform.TransformPoint(((RectTransform)highlightObject.transform).rect.center);
        pointer.ShowHighlight();

        UIManager.AddHighlightUIObject(highlightObject, 1);
        UIManager.AddHighlightUIObject(pointer.gameObject, 2);
    }

    public void ClearHighligt()
    {
        pointer.Hide();
        UIManager.ClearHighlightUIObjects();
    }
}
