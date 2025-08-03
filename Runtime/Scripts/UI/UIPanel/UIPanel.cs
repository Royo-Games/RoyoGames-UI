using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UIPanelState
{
    BeginShow,
    EndShow,
    BeginHide,
    EndHide
}

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    public UIPanelState State => state;
    public bool IsShow => state == UIPanelState.BeginShow || state == UIPanelState.EndShow;

    public UIPanelAnimation ShowAnimation
    {
        get { return showAnimation; }
        set { showAnimation = value; }
    }
    public UIPanelAnimation HideAnimation
    {
        get { return hideAnimation; }
        set { hideAnimation = value; }
    }

    public CanvasGroup CanvasGroup { get; private set; }
    public Transform Body { get; private set; }

    public UnityEvent OnBeginShow => onBeginShow;
    public UnityEvent OnEndShow => onEndShow;
    public UnityEvent OnBeginHide => onBeginHide;
    public UnityEvent OnEndHide => onEndHide;

    private UIPanelState state;

    [SerializeField] private bool editMode;
    [SerializeField] private UIPanelAnimation showAnimation;
    [SerializeField] private UIPanelAnimation hideAnimation;

    [SerializeField][HideInInspector] private UnityEvent onBeginShow;
    [SerializeField][HideInInspector] private UnityEvent onEndShow;
    [SerializeField][HideInInspector] private UnityEvent onBeginHide;
    [SerializeField][HideInInspector] private UnityEvent onEndHide;

    private Action onOpened;
    private Action onClosed;

    private Tween currentAnimation;

#if UNITY_EDITOR
    [SerializeField][HideInInspector] private bool eventsShow;
    public bool IsEditMode => editMode;
#endif

    protected virtual void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        Body = transform.Find("Body");

        state = UIPanelState.EndHide;
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.alpha = 0;
    }

    public void Show()
    {
        Show(null);
    }

    public void Show(Action onOpened = null)
    {
        if (IsShow)
            return;

        state = UIPanelState.BeginShow;
        this.onOpened = onOpened;

        onBeginShow.Invoke();
        CanvasGroup.blocksRaycasts = true;

        currentAnimation.Kill();

        if(ShowAnimation != null)
        {
            currentAnimation = ShowAnimation.Play(this);
            currentAnimation.OnComplete(EndAnimation);
        }
        else
        {
            EndAnimation();
        }
    }

    public void Hide()
    {
        Hide(null);
    }

    public void Hide(Action onClosed)
    {
        if (!IsShow)
            return;

        state = UIPanelState.BeginHide;
        this.onClosed = onClosed;
        onBeginHide.Invoke();

        CanvasGroup.blocksRaycasts = false;

        currentAnimation.Kill();

        if (HideAnimation != null)
        {
            currentAnimation = HideAnimation.Play(this);
            currentAnimation.OnComplete(EndAnimation);
        }
        else
        {
            EndAnimation();
        }
    }

    public void Switch()
    {
        if (IsShow)
            Hide();
        else
            Show();
    }

    internal void EndAnimation()
    {
        switch (State)
        {
            case UIPanelState.BeginShow:
                state = UIPanelState.EndShow;
                CanvasGroup.alpha = 1;
                onEndShow.Invoke();
                onOpened?.Invoke();
                onOpened = null;
                break;

                case UIPanelState.BeginHide:
                state = UIPanelState.EndHide;
                CanvasGroup.alpha = 0;
                onEndHide.Invoke();
                onClosed?.Invoke();
                onClosed = null;
                break;
        }
    }
}