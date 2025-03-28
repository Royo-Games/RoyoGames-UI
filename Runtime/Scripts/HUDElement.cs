using UnityEngine;

public class HUDElement : MonoBehaviour
{
    [SerializeField] private Camera _worldCamera;

    [Space]
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _targetOffset;

    [Space]
    [SerializeField] private bool _enableClamp;
    [SerializeField] private Vector2 _clampOffset;

    [Space]
    [SerializeField] private bool _enableAutoScale;
    [SerializeField] private float _scaleReference = 10;
    [SerializeField] private float _minScale = 0.1f;

    protected RectTransform rect;
    protected RectTransform canvasRect;
    protected Canvas canvas;

    public Transform Target
    {
        get=> _target;
        set=> _target = value;
    }

    public Camera WorldCamera
    {
        get => _worldCamera; 
        set => _worldCamera = value;
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    public virtual void LateUpdate()
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(_worldCamera, _target.position + _targetOffset);

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            transform.position = screenPoint;
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint,
                canvas.renderMode == RenderMode.ScreenSpaceCamera ? _worldCamera : null, out Vector2 localPoint);
            rect.localPosition = localPoint;
        }

        if(_enableAutoScale)
        {
            float distance = Vector3.Distance(_worldCamera.transform.position, _target.position);
            float scale = Mathf.Clamp(_scaleReference / distance, _minScale, 1);
            rect.localScale = Vector3.one * scale;
        }


        if (_enableClamp)
            Clamp(canvasRect, rect, _clampOffset);
    }

    protected virtual void Clamp(RectTransform canvasRect, RectTransform rect, Vector2 offset)
    {
        UIUtility.ClampRect(canvasRect, rect, offset);
    }
}
