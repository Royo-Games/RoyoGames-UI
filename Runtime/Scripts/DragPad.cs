using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Space]
    [SerializeField] UnityEvent<DragPad> _onBeginDragEvent;
    [SerializeField] UnityEvent<DragPad> _onEndDragEvent;
    [SerializeField] UnityEvent<DragPad> _onDragEvent;

    public UnityEvent<DragPad> OnBeginDragEvent => _onBeginDragEvent;
    public UnityEvent<DragPad> OnEndDragEvent => _onEndDragEvent;
    public UnityEvent<DragPad> OnDragEvent => _onDragEvent;

    public bool IsDragging { get; private set; }
    public Vector2 Delta { get; private set; }
    public Vector2 TotalDelta { get; private set; }

    public virtual void LateUpdate()
    {
        Delta = Vector2.zero;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        IsDragging = true;
        _onBeginDragEvent?.Invoke(this);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Delta = eventData.delta;
        TotalDelta += Delta;
        OnDragEvent?.Invoke(this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        TotalDelta = Vector2.zero;
        IsDragging = false;
        _onEndDragEvent?.Invoke(this);
    }
}
