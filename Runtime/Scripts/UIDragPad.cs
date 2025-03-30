using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[DefaultExecutionOrder(1000)]
public class UIDragPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Space]
    [SerializeField] UnityEvent<UIDragPad> _onDragDownEvent;
    [SerializeField] UnityEvent<UIDragPad> _onDragUpEvent;
    [SerializeField] UnityEvent<UIDragPad> _onDraggingEvent;

    public UnityEvent<UIDragPad> OnDragDownEvent => _onDragDownEvent;
    public UnityEvent<UIDragPad> OnDragUpEvent => _onDragUpEvent;
    public UnityEvent<UIDragPad> OnDraggingEvent => _onDraggingEvent;

    public Vector2 Delta { get; private set; }
    public Vector2 TotalDelta { get; private set; }

    public bool IsDragDown { get; private set; }
    public bool IsDragUp { get; private set; }
    public bool IsDragging { get; private set; }

    public virtual void LateUpdate()
    {
        Delta = Vector2.zero;
        IsDragDown = false;
        IsDragging = false;
        IsDragUp = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        IsDragDown = true;
        _onDragDownEvent?.Invoke(this);
    }
    public void OnDrag(PointerEventData eventData)
    {
        IsDragging = true;
        Delta = eventData.delta;
        TotalDelta += Delta;
        OnDraggingEvent?.Invoke(this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        IsDragUp = true;
        _onDragUpEvent?.Invoke(this);
    }
}
