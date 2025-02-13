using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ScrollRectSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public bool IsDragging { get; private set; }
    public int CurrentElementIndex
    {
        get
        {
            return currentItemIndex;
        }
        private set
        {
            if (IsReady)
            currentItemIndex = Mathf.Clamp(value, 0, content.childCount - 1);
        }
    }
    public RectTransform CurrentElement => currentElement;
    public float Smoot
    {
        get
        {
            return smoot;
        }
        set
        {
            smoot = MathF.Max(value, 0);
        }
    }

    public RectTransform Viewport => viewport;
    public RectTransform Content => content;

    public List<RectTransform> Items => items;

    private RectTransform currentElement;
    private Vector2 targetPos;

    [SerializeField] private float smoot = 1;
    [SerializeField] private int currentItemIndex;
    [SerializeField] private Direction direction;
    [SerializeField] private TransitionMode transitionMode;
    [Space]
    [SerializeField] private RectTransform viewport;
    [SerializeField] private RectTransform content;

    [Space]
    public UnityEvent<int> OnCenterElement;
    private List<RectTransform> items = new List<RectTransform>();

    internal bool IsReady
    {
        get
        {
            return viewport != null && content != null;
        }
    }

    public enum TransitionMode
    {
        Free,
        OneStep
    }
    public enum Direction
    {
        Horizontal,
        Vertical
    }
    public void Awake()
    {
        currentElement = null;
    }
    public void Start()
    {
        Init();
    }
    private void OnValidate()
    {
        smoot = Math.Max(smoot, 0);
    }
    private void Init()
    {
        Canvas.ForceUpdateCanvases();
        UpdateItemList();
    }
    private void Update()
    {
        if (!IsReady)
            return;

        if (!IsDragging && currentElement != null)
        {
            targetPos = CalculateContentPos(currentElement.transform as RectTransform);
            content.anchoredPosition = Vector3.Lerp(content.anchoredPosition, targetPos, 10 / smoot * Time.unscaledDeltaTime);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!IsReady)
            return;

        var pos = content.position;
        switch (direction)
        {
            case Direction.Horizontal:
                pos.x += eventData.delta.x;
                break;
            case Direction.Vertical:
                pos.y += eventData.delta.y;
                break;
        }
        content.position = pos;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragging = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsReady)
            return;

        switch (transitionMode)
        {
            case TransitionMode.OneStep:

                float velocity = direction == Direction.Horizontal ? eventData.delta.x : -eventData.delta.y;

                if (Mathf.Abs(velocity) > 10)
                {
                    if (velocity < 0)
                        Next();
                    else
                        Previous();
                }
                else
                {
                    UpdateCenterElement(Vector2.zero);
                }

                break;
            case TransitionMode.Free:
                UpdateCenterElement(eventData.delta);
                break;
        }

        IsDragging = false;
    }
    private void UpdateCenterElement(Vector2 delta)
    {
        float lastDistance = float.MaxValue;
        RectTransform nearestItem = null;

        var viewportPos = viewport.TransformPoint(viewport.rect.center);

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var deltaPos = item.TransformPoint(item.rect.center) + (new Vector3(delta.x, delta.y, 0) * 10);
            var distance = Vector3.Distance(deltaPos, viewportPos);

            if (distance <= lastDistance)
            {
                lastDistance = distance;
                nearestItem = item;
            }
        }

        CenterTo(nearestItem);
    }
    public void CenterTo(int elementIndex)
    {
        CenterTo(elementIndex, true);
    }
    public void CenterTo(int elementIndex, bool isSmoot = true)
    {
        CurrentElementIndex = elementIndex;

        if (!IsReady || items.Count == 0)
            return;

        CenterTo(items[CurrentElementIndex], isSmoot);
    }
    public void CenterTo(RectTransform item, bool isSmoot = true)
    {
        if (!IsReady)
            return;

        if (Application.isPlaying)
        {
            if (currentElement == item || item == null)
                return;

            OnCenterElement?.Invoke(CurrentElementIndex);
        }

        CurrentElementIndex = item.transform.GetSiblingIndex();
        currentElement = item;

        targetPos = CalculateContentPos(currentElement);

        if (!isSmoot)
        {
            content.anchoredPosition = targetPos;
        }
    }
    public void CenterTo()
    {
        CenterTo(currentItemIndex, false);
    }
    public void Next()
    {
        var index = CurrentElementIndex + 1;

        if (index < items.Count)
            CenterTo(items[index]);
    }
    public void Previous()
    {
        var index = CurrentElementIndex - 1;

        if (index >= 0)
            CenterTo(items[index]);
    }
    public void UpdateItemList()
    {
        if (!IsReady)
            return;

        items.Clear();

        for (int i = 0; i < content.childCount; i++)
        {
            var item = content.GetChild(i);
            items.Add(item as RectTransform);
        }

        CenterTo();
    }
    private Vector2 CalculateContentPos(RectTransform item)
    {
        var contentPos = viewport.InverseTransformPoint(content.position);
        var newPos = viewport.InverseTransformPoint(item.TransformPoint(item.rect.center));

        return contentPos - newPos;
    }
}