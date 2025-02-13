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

    protected RectTransform rect;
    protected RectTransform canvasRect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }
    public virtual void Update()
    {
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera, Target.position + TargetOffset);
        transform.position = pos;

        if (EnableClamp)
            Clamp(canvasRect, rect, ClampOffset);
    }
    protected virtual void Clamp(RectTransform canvasRect, RectTransform rect, Vector2 offset)
    {
        UIUtility.ClampRect(canvasRect, rect, offset);
    }
}