using UnityEngine;

public static class UIUtility
{
    public static Vector2 Clamp(this RectTransform rect, RectTransform vieportRect)
    {
        return Clamp(rect, vieportRect, Vector2.zero);
    }

    public static Vector2 Clamp(this RectTransform rect, RectTransform vieportRect, Vector2 padding)
    {
        Vector2 parentSize = vieportRect.rect.size;
        Vector2 childSize = rect.rect.size;
        Vector2 scaledChildSize = Vector2.Scale(childSize, rect.localScale);

        float pivotOffsetMinX = scaledChildSize.x * rect.pivot.x;
        float pivotOffsetMaxX = scaledChildSize.x * (1 - rect.pivot.x);
        float pivotOffsetMinY = scaledChildSize.y * rect.pivot.y;
        float pivotOffsetMaxY = scaledChildSize.y * (1 - rect.pivot.y);

        float minX = -parentSize.x * 0.5f + pivotOffsetMinX + padding.x;
        float maxX = parentSize.x * 0.5f - pivotOffsetMaxX - padding.x;
        float minY = -parentSize.y * 0.5f + pivotOffsetMinY + padding.y;
        float maxY = parentSize.y * 0.5f - pivotOffsetMaxY - padding.y;

        Vector2 pos = rect.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        return pos;
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

    public static Vector3 UIToWorldPosition(Vector3 uiWorldPos, Canvas canvas, Camera worldCam, float targetZ = 0)
    {
        Vector2 screenPoint;
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            screenPoint = new Vector2(uiWorldPos.x, uiWorldPos.y);
        }
        else
        {
            screenPoint = RectTransformUtility.WorldToScreenPoint(
                canvas.worldCamera,
                uiWorldPos
            );
        }

        Ray ray = worldCam.ScreenPointToRay(screenPoint);

        Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, targetZ));

        if (plane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }
}
