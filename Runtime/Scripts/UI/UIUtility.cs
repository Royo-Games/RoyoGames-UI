using UnityEngine;

public static class UIUtility
{
    public static void ClampRect(RectTransform parentRect, RectTransform rect)
    {
        ClamVieport(parentRect, rect, Vector2.zero);
    }
    public static Vector2 ClamVieport(this RectTransform rect, RectTransform vieportRect, Vector2 padding)
    {
        Vector2 position = rect.anchoredPosition;

        Vector2 scale = new Vector2(rect.localScale.x, rect.localScale.y);
        Vector2 scaledSize = new Vector2(rect.sizeDelta.x * scale.x, rect.sizeDelta.y * scale.y);

        Vector2 anchorOffset = vieportRect.sizeDelta * (rect.anchorMin - Vector2.one * 0.5f);

        Vector2 maxPivotOffset = scaledSize * (rect.pivot - Vector2.one);
        Vector2 minPivotOffset = scaledSize * (Vector2.one - rect.pivot);

        float minX = vieportRect.sizeDelta.x * -0.5f - anchorOffset.x - minPivotOffset.x + scaledSize.x;
        float maxX = vieportRect.sizeDelta.x * 0.5f - anchorOffset.x + maxPivotOffset.x;
        float minY = vieportRect.sizeDelta.y * -0.5f - anchorOffset.y - minPivotOffset.y + scaledSize.y;
        float maxY = vieportRect.sizeDelta.y * 0.5f - anchorOffset.y + maxPivotOffset.y;

        position.x = Mathf.Clamp(position.x, minX + padding.x, maxX - padding.x);
        position.y = Mathf.Clamp(position.y, minY + padding.y, maxY - padding.y);

        return position;
    }

    public static bool IsRectangleOutsideViewport(this RectTransform rectTransform, Canvas canvas, RectTransform vieportRect)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        RectTransform canvasRect = (RectTransform)canvas.transform;

        Camera camera = null;

        switch (canvas.renderMode)
        {
            case RenderMode.ScreenSpaceCamera:
            case RenderMode.WorldSpace:
                camera = canvas.worldCamera;
                break;
        }

        foreach (var corner in corners)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, corner);

            if (RectTransformUtility.RectangleContainsScreenPoint(vieportRect, screenPoint, camera))
                return false;
        }

        return true;
    }

    public static Vector3 WorldToUIPosition(Vector3 worldPos, Canvas canvas, Camera worldCam)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(worldCam, worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, screenPoint, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, out Vector2 result);
        return canvas.transform.TransformPoint(result);
    }
}
