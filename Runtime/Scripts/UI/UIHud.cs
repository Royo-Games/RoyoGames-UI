using System;
using UnityEngine;

public class UIHud : MonoBehaviour
{
    [SerializeField] private Camera _worldCamera;

    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _positionOffset;

    [SerializeField] private bool _enableClamp;
    [SerializeField] private Vector2 _clampPadding;

    [SerializeField] private bool _enableAutoScale;
    [SerializeField] private float _scaleFactor = 10;
    [SerializeField] private float _minScale = 0.1f;

    public Action<bool> OnChangedClampStateEvent;
    public Action<bool> OnChangedOutsideVieportEvent;

    protected RectTransform rect;
    protected RectTransform vieportRect;
    protected Canvas canvas;

    public bool IsClamping { get; private set; }
    public bool IsOutsideViewport { get; private set; }

    public Transform Target
    {
        get => _target;
        set => _target = value;
    }

    public Camera WorldCamera
    {
        get => _worldCamera;
        set => _worldCamera = value;
    }

    public Vector2 PositionOffset
    {
        get => _positionOffset;
        set => _positionOffset = value;
    }

    public bool EnableClamp
    {
        get=> _enableClamp;
        set => _enableClamp = value;
    }

    public Vector2 ClampPadding
    {
        get => _clampPadding;
        set => _clampPadding = value;
    }

    public bool EnableAutoScale
    {
        get => _enableAutoScale; 
        set => _enableAutoScale = value;
    }

    public float ScaleFactor
    {
        get => _scaleFactor; 
        set => _scaleFactor = value;
    }

    public float MinScale
    {
        get => _minScale;
        set => _minScale = value;
    }

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        vieportRect = transform.parent.GetComponent<RectTransform>();
    }

    protected virtual void OnEnable()
    {
        UpdatePosition();
    }

    protected virtual void Update()
    {
        UpdatePosition();
    }

    public virtual void UpdatePosition()
    {
        if (_worldCamera == null || _target == null)
            return;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(_worldCamera, _target.position);

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            transform.position = screenPoint;
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(vieportRect, screenPoint,
                canvas.renderMode == RenderMode.ScreenSpaceCamera ? _worldCamera : null, out Vector2 localPoint);
            rect.localPosition = localPoint;
        }

        rect.anchoredPosition += PositionOffset;

        if (_enableAutoScale)
            AutoScale();

        if (_enableClamp)
            Clamp(vieportRect, rect);
        else
            CheckOutSideViewport();
    }

    protected void AutoScale()
    {
        float distance = Vector3.Distance(_worldCamera.transform.position, _target.position);
        float scale = Mathf.Clamp(_scaleFactor / distance, _minScale, 1);
        rect.localScale = Vector3.one * scale;
    }

    protected void CheckOutSideViewport()
    {
        Camera usedCamera = WorldCamera;

        switch (canvas.renderMode)
        {
            case RenderMode.ScreenSpaceCamera:
            case RenderMode.WorldSpace:
                usedCamera = canvas.worldCamera;
                break;
        }

        bool isOutsideViewport = rect.IsRectangleOutsideViewport(canvas, vieportRect);

        if (IsOutsideViewport != isOutsideViewport)
        {
            IsOutsideViewport = isOutsideViewport;
            OnChangedOutsideVieportEvent?.Invoke(isOutsideViewport);
            OnChangedOutsideVieport(isOutsideViewport);
        }
    }

    protected virtual void Clamp(RectTransform canvasRect, RectTransform rect)
    {
        Vector2 newPos = rect.Clamp(vieportRect, _clampPadding);
        bool isClamping = newPos != rect.anchoredPosition;

        rect.anchoredPosition = newPos;

        if (IsClamping != isClamping)
        {
            IsClamping = isClamping;
            OnChangedClampStateEvent?.Invoke(isClamping);
        }
    }

    protected virtual void OnChangedOutsideVieport(bool isOutsideViewport)
    {
    }

    protected virtual void OnChangedClampState(bool isClamping)
    {
    }
}
