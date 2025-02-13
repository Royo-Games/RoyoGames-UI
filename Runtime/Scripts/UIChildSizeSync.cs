using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIChildSizeSync : MonoBehaviour
{
    public RectTransform Reference;
    private Vector2 tempSize;

    [SerializeField] private bool width;
    [SerializeField] private bool height;

    private void Awake()
    {
        UpdateSize();
    }
    private void OnValidate()
    {
        if (Reference != null && enabled)
            Calculate();
    }
    private void LateUpdate()
    {
        UpdateSize();
    }
    private void UpdateSize()
    {
        if (Reference == null || !enabled)
            return;

        var size = new Vector2(Reference.rect.width, Reference.rect.height);

        if(tempSize != size || !Application.isPlaying)
        {
            Calculate();
            tempSize = size;
        }
    }
    public void Calculate()
    {
        var size = new Vector2(Reference.rect.width, Reference.rect.height);

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i) as RectTransform;
            var sizeDelta = child.sizeDelta;

            if (width)
                sizeDelta.x = size.x;

            if (height)
                sizeDelta.y = size.y;

            child.sizeDelta = sizeDelta;
        }
    }
    private void OnTransformChildrenChanged()
    {
        if(enabled)
        Calculate();
    }
}
