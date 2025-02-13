using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIUtility
{
    public static void ClampRect(RectTransform parentRect, RectTransform rect)
    {
        ClampRect(parentRect, rect, Vector2.zero);
    }
    public static void ClampRect(RectTransform canvasRect, RectTransform rect, Vector2 offset)
    {
        var position = rect.anchoredPosition;

        Vector2 anchorOffset = canvasRect.sizeDelta * (rect.anchorMin - Vector2.one / 2);

        Vector2 maxPivotOffset = rect.sizeDelta * (rect.pivot - (Vector2.one / 2) * 2);
        Vector2 minPivotOffset = rect.sizeDelta * ((Vector2.one / 2) * 2 - rect.pivot);

        float minX = (canvasRect.sizeDelta.x) * -0.5f - anchorOffset.x - minPivotOffset.x + rect.sizeDelta.x;
        float maxX = (canvasRect.sizeDelta.x) * 0.5f - anchorOffset.x + maxPivotOffset.x;
        float minY = (canvasRect.sizeDelta.y) * -0.5f - anchorOffset.y - minPivotOffset.y + rect.sizeDelta.y;
        float maxY = (canvasRect.sizeDelta.y) * 0.5f - anchorOffset.y + maxPivotOffset.y;

        position.x = Mathf.Clamp(position.x, minX - offset.x, maxX + offset.x);
        position.y = Mathf.Clamp(position.y, minY - offset.y, maxY + offset.y);

        rect.anchoredPosition = position;
    }
}
