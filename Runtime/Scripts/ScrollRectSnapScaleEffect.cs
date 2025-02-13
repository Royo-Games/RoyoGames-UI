using System;
using UnityEngine;

[ExecuteInEditMode()]
[RequireComponent(typeof(ScrollRectSnap))]
public class ScrollRectSnapScaleEffect : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float scaleFactor = 1.0f;

    private ScrollRectSnap scrollRectSnap;
    public ScrollRectSnap ScrollRectSnap
    {
        get
        {
            if (scrollRectSnap == null)
                scrollRectSnap = GetComponentInParent<ScrollRectSnap>();

            return scrollRectSnap;
        }
    }
    public float MinScale
    {
        get
        {
            return minScale;
        }
        set
        {
            minScale = Math.Max(value, 0);
        }
    }
    public float ScaleFactor
    {
        get 
        {
            return scaleFactor;
        }
        set
        {
            scaleFactor = MathF.Max(value, 0);
        }
    }
    private void OnValidate()
    {
        scaleFactor = Math.Max(scaleFactor, 0);
    }
    private void OnDisable()
    {
        ResetItemsScale();
    }
    private void LateUpdate()
    {
        if (ScrollRectSnap.Items == null || !ScrollRectSnap.IsReady)
            return;

        var viewportPos = ScrollRectSnap.Viewport.TransformPoint(ScrollRectSnap.Viewport.rect.center);

        for (int i = 0; i < ScrollRectSnap.Items.Count; i++)
        {
            var item = ScrollRectSnap.Items[i];

            if(item == null)
                continue;

            var scale = item.localScale;
            var itemPos = item.TransformPoint(item.rect.center);
            var distance = Vector2.Distance(itemPos, viewportPos);
            var size = Mathf.Clamp(1.0f - (distance / (1000 / scaleFactor)), minScale, 1.0f);
            scale.x = size;
            scale.y = size;
            item.localScale = scale;
        }
    }
    private void ResetItemsScale()
    {
        for (int i = 0; i < ScrollRectSnap.Items.Count; i++)
        {
            var item = ScrollRectSnap.Items[i];
            item.transform.localScale = Vector3.one;
        }
    }
}
