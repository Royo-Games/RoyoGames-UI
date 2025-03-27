using UnityEngine;

public class HUDElement : MonoBehaviour
{
    public Camera Camera;

    [Space]
    public Transform Target;
    public Vector3 TargetOffset;

    [Space]
    public bool EnableClamp;
    public Vector2 ClampOffset;

    [Space]
    public bool EnableAutoScale;
    public float minDistance = 5;
    public float maxDistance = 50;
    public float minScale = 0.5f;
    public float maxScale = 1f;

    protected RectTransform rect;
    protected RectTransform canvasRect;
    protected Canvas canvas;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    public virtual void Update()
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera, Target.position + TargetOffset);

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            transform.position = screenPoint;
        }
        else
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint,
                canvas.renderMode == RenderMode.ScreenSpaceCamera ? Camera : null, out localPoint);
            rect.localPosition = localPoint;
        }

        if(EnableAutoScale)
        {
            float distance = Vector3.Distance(Camera.transform.position, Target.position);
            float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
            float scale = Mathf.Lerp(maxScale, minScale, t);
            rect.localScale = new Vector3(scale, scale, scale);
        }


        if (EnableClamp)
            Clamp(canvasRect, rect, ClampOffset);
    }

    protected virtual void Clamp(RectTransform canvasRect, RectTransform rect, Vector2 offset)
    {
        UIUtility.ClampRect(canvasRect, rect, offset);
    }
}
