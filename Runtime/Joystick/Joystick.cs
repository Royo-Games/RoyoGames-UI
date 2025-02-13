using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum AxisOptions { Both, Horizontal, Vertical }
public enum JoystickType { Fixed, Floating, Dynamic }

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float MoveThreshold { get { return _moveThreshold; } set { _moveThreshold = Mathf.Abs(value); } }

    [HideInInspector]
    [SerializeField]
    private float _moveThreshold = 1;
    [HideInInspector]
    [SerializeField]
    private JoystickType _joystickType = JoystickType.Fixed;
    [HideInInspector]
    [SerializeField]
    private bool _autoHide;
    [HideInInspector]
    [SerializeField]
    private float _visibleMagnitude;

    private Vector2 _fixedPosition = Vector2.zero;

    public float Horizontal { get { return _input.x; } }
    public float Vertical { get { return _input.y; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

    public bool IsVisible
    {
        get
        {
            return _isVisible;
        }

        set
        {
            if (_isVisible == value)
                return;
            SetVisible(value);
        }
    }
    public float HandleRange
    {
        get { return _handleRange; }
        set { _handleRange = Mathf.Abs(value); }
    }
    public AxisOptions AxisOptions { get { return AxisOptions; } set { _axisOptions = value; } }

    [HideInInspector]
    [SerializeField]
    private float _handleRange = 1;

    [HideInInspector]
    [SerializeField]
    private AxisOptions _axisOptions = AxisOptions.Both;

    [HideInInspector]
    [SerializeField]
    protected RectTransform _background = null;

    [HideInInspector]
    [SerializeField]
    private RectTransform _handle = null;

    private RectTransform _baseRect = null;

    private Canvas _canvas;
    private Camera _cam;

    private Vector2 _input = Vector2.zero;

    private bool _isVisible;

    public virtual void Start()
    {
        HandleRange = _handleRange;
        _baseRect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
            Debug.LogError("The Joystick is not placed inside a tutorialCanvas");

        Vector2 center = new Vector2(0.5f, 0.5f);
        _background.pivot = center;
        _handle.anchorMin = center;
        _handle.anchorMax = center;
        _handle.pivot = center;
        _handle.anchoredPosition = Vector2.zero;

        _fixedPosition = _background.anchoredPosition;
        SetMode(_joystickType);
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (_joystickType != JoystickType.Fixed)
        {
            _background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);

            if (!_autoHide)
                _background.gameObject.SetActive(true);
        }

        OnDrag(eventData);
    }
    public virtual void OnDrag(PointerEventData eventData)
    {
        _cam = null;
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            _cam = _canvas.worldCamera;

        if (_joystickType != JoystickType.Fixed && _autoHide && Direction.magnitude >= _visibleMagnitude)
            IsVisible = true;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(_cam, _background.position);
        Vector2 radius = _background.sizeDelta / 2;
        _input = (eventData.position - position) / (radius * _canvas.scaleFactor);
        FormatInput();
        HandleInput(_input.magnitude, _input.normalized, radius, _cam);
        _handle.anchoredPosition = _input * radius * _handleRange;
    }
    public void SetMode(JoystickType joystickType)
    {
        _joystickType = joystickType;
        if (joystickType == JoystickType.Fixed)
        {
            _background.anchoredPosition = _fixedPosition;
            IsVisible = true;
        }
        else
        if (_autoHide)
            SetVisible(false);
    }
    public virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (_joystickType == JoystickType.Dynamic && magnitude > _moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - _moveThreshold) * radius;
            _background.anchoredPosition += difference;
        }

        if (magnitude > 0)
        {
            if (magnitude > 1)
                _input = normalised;
        }
        else
            _input = Vector2.zero;
    }
    private void FormatInput()
    {
        if (_axisOptions == AxisOptions.Horizontal)
            _input = new Vector2(_input.x, 0f);
        else if (_axisOptions == AxisOptions.Vertical)
            _input = new Vector2(0f, _input.y);
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (_joystickType != JoystickType.Fixed && _autoHide)
            IsVisible = false;

        _input = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
        _background.anchoredPosition = Vector2.zero;
    }
    protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition, _cam, out localPoint))
        {
            Vector2 pivotOffset = _baseRect.pivot * _baseRect.sizeDelta;
            return localPoint - (_background.anchorMax * _baseRect.sizeDelta) + pivotOffset;
        }
        return Vector2.zero;
    }
    public virtual void SetVisible(bool isVisible)
    {
        _background.gameObject.SetActive(isVisible);
        _isVisible = isVisible;
    }
}