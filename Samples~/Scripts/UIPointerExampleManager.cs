using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPointerExampleManager : MonoBehaviour
{
    public UIPointer clickMePointer;
    public UIPointer dragPointer;
    public RectTransform ClickMeButtonTransform;
    public RectTransform DragStartTransform;
    public RectTransform DragEndTransform;

    private void Start()
    {
        clickMePointer.transform.position = ClickMeButtonTransform.TransformPoint(ClickMeButtonTransform.rect.center);
        clickMePointer.ShowHighlight();

        dragPointer.StartPosition = DragStartTransform.TransformPoint(DragStartTransform.rect.center);
        dragPointer.TargetPosition = DragEndTransform.TransformPoint(DragEndTransform.rect.center);

        dragPointer.ShowDrag();
    }
}
