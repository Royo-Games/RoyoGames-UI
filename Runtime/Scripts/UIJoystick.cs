using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum JoystickType
{
    Fixed,
    Floating,
    Dynamic
}

public enum JoystickAxis
{
    Both,
    Horizontal,
    Vertical
}

public enum JoystickVisibility
{
    Visible,
    Auto,
    UnVisible
}

[DefaultExecutionOrder(1000)]
public class UIJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Settings")]
    [SerializeField] JoystickType _joystickType = JoystickType.Fixed;
    [SerializeField] JoystickAxis _axis = JoystickAxis.Both;
    [SerializeField] JoystickVisibility _visibility = JoystickVisibility.Visible;
    [Space]
    [SerializeField] float _handleRange = 1f;
    [SerializeField] float _deadZone = 0f;
    [SerializeField] private bool _backToStartPos = true;

    [Header("References")]
    [SerializeField] RectTransform _background;
    [SerializeField] RectTransform _handle;

    [Header("Events")]
    [SerializeField] UnityEvent<UIJoystick> _onDragDownEvent;
    [SerializeField] UnityEvent<UIJoystick> _onDragUpEvent;
    [SerializeField] UnityEvent<UIJoystick> _onDraggingEvent;

    public Vector2 Direction { get; private set; }
    public Vector2 Delta { get; private set; }
    public Vector2 TotalDelta { get; private set; }

    public bool IsDragDown { get; private set; }
    public bool IsDragUp { get; private set; }
    public bool IsDragging { get; private set; }

    public UnityEvent<UIJoystick> OnDragDownEvent => _onDragDownEvent;
    public UnityEvent<UIJoystick> OnDragUpEvent => _onDragUpEvent;
    public UnityEvent<UIJoystick> OnDraggingEvent => _onDraggingEvent;

    private Vector2 _startPosition;
    private Camera _cam;
    private Canvas _canvas;

    public virtual void Start()
    {
        _canvas = GetComponentInParent<Canvas>();
        _startPosition = _background.anchoredPosition;

        UpdateVisibility();
    }

    public virtual void LateUpdate()
    {
        Delta = Vector2.zero;
        IsDragDown = false;
        IsDragging = false;
        IsDragUp = false;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (_joystickType != JoystickType.Fixed)
        {
            _background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        }

        if(_visibility != JoystickVisibility.UnVisible)
        _background.gameObject.SetActive(true);

        OnDrag(eventData);

        _onDragDownEvent?.Invoke(this);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        _cam = null;
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            _cam = _canvas.worldCamera;

        Vector2 position = ScreenPointToAnchoredPosition(eventData.position);
        Vector2 delta = position - _background.anchoredPosition;

        float radius = _background.sizeDelta.x / 2f;

        Direction = delta / (_background.sizeDelta.x / 2f * _handleRange);
        Delta = eventData.delta;
        TotalDelta += Delta;

        if (Direction.magnitude < _deadZone)
            Direction = Vector2.zero;

        Direction = Vector2.ClampMagnitude(Direction, 1f);

        if (_axis == JoystickAxis.Horizontal)
        {
            Direction = new Vector2(Direction.x, 0f);
        }
        else if (_axis == JoystickAxis.Vertical)
        {
            Direction = new Vector2(0f, Direction.y);
        }

        _handle.anchoredPosition = Direction * radius * _handleRange;

        if (_joystickType == JoystickType.Dynamic)
        {
            float maxDistance = (_background.sizeDelta.x / 2f) * _handleRange;
            if (delta.magnitude > maxDistance)
            {
                Vector2 excess = delta.normalized * (delta.magnitude - maxDistance);
                _background.anchoredPosition += excess;
            }
        }

        IsDragging = true;
        _onDraggingEvent?.Invoke(this);
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Direction = Vector2.zero;
        TotalDelta = Vector2.zero;

        _handle.anchoredPosition = Vector2.zero;

        if (_backToStartPos)
            _background.anchoredPosition = _startPosition;

        UpdateVisibility();

        _onDragUpEvent?.Invoke(this);
    }
    protected virtual void UpdateVisibility()
    {
        switch (_visibility)
        {
            case JoystickVisibility.Visible:
                _background.gameObject.SetActive(true);
                break;
            case JoystickVisibility.UnVisible:
            case JoystickVisibility.Auto:
                _background.gameObject.SetActive(false);
                break;
        }
    }
    protected virtual Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        RectTransform parentRect = _background.parent as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPosition, _cam, out Vector2 localPoint);

        Vector2 anchorReference = new Vector2(
            Mathf.Lerp(parentRect.rect.xMin, parentRect.rect.xMax, _background.anchorMin.x),
            Mathf.Lerp(parentRect.rect.yMin, parentRect.rect.yMax, _background.anchorMin.y)
        );

        return localPoint - anchorReference;
    }
}
