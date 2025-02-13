using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum UIPanelState
{
    BeginShow,
    EndShow,
    BeginHide,
    EndHide
}

[RequireComponent(typeof(CanvasGroup), typeof(Animator))]
public class UIPanel : MonoBehaviour
{
    public UIPanelState State => state;
    public bool IsShow => state == UIPanelState.BeginShow || state == UIPanelState.EndShow;

    public bool EnableEscapeHide => enableEscapeHide;

    public string ShowAnimationName
    {
        get { return showTrigger; }
        set { showTrigger = value; }
    }
    public string HideAnimationName
    {
        get { return hideTrigger; }
        set { hideTrigger = value; }
    }

    public UnityEvent OnOpening => onBeginOpen;
    public UnityEvent OnOpened => onEndOpen;
    public UnityEvent OnClosing => onBeginHide;
    public UnityEvent OnClosed => onEndHide;

    [HideInInspector] private Animator animator;
    [HideInInspector] private CanvasGroup canvasGroup;
    private UIPanelState state;

    [SerializeField] private bool enableEscapeHide;
    [SerializeField] private bool editMode;
    [SerializeField] private string showTrigger;
    [SerializeField] private float showSpeed = 1;
    [SerializeField] private string hideTrigger;
    [SerializeField] private float hideSpeed = 1;
    [SerializeField][HideInInspector] private UnityEvent onBeginOpen;
    [SerializeField][HideInInspector] private UnityEvent onEndOpen;
    [SerializeField][HideInInspector] private UnityEvent onBeginHide;
    [SerializeField][HideInInspector] private UnityEvent onEndHide;

    private Coroutine showCoroutine;
    private Coroutine hideCoroutine;

#if UNITY_EDITOR
    [SerializeField][HideInInspector] private bool eventsShow;
    public bool IsEditMode => editMode;
#endif

    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();

        state = UIPanelState.EndHide;
        animator.enabled = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
    public virtual void OnDestroy()
    {
        if (IsShow)
        {
            UIManager.Instance?.ActivePanels.Remove(this);
        }
    }
    public void Show(float delay = 0)
    {
        Show(delay, null);
    }
    public void Show(UnityAction onOpened)
    {
        Show(0, onOpened);
    }
    public void Show(float delay, UnityAction onOpened)
    {
        if (IsShow)
            return;

        if (delay == 0)
            state = UIPanelState.BeginShow;

        if (showCoroutine != null)
        {
            StopCoroutine(showCoroutine);
            showCoroutine = null;
        }

        showCoroutine = StartCoroutine(IShow(delay, onOpened));
    }
    public IEnumerator ShowCoroutine()
    {
        yield return IShow(0, null);
    }
    private IEnumerator IShow(float delay, UnityAction onOpened)
    {
        yield return new WaitForSeconds(delay);

        state = UIPanelState.BeginShow;
        onBeginOpen.Invoke();

        UIManager.Instance.ActivePanels.Add(this);

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = true;
        animator.enabled = true;
        animator.speed = showSpeed;
        animator.SetTrigger(ShowAnimationName);

        yield return animator.WaitNormalizedTimeCoroutine(0, 1);

        state = UIPanelState.EndShow;
        animator.enabled = false;
        canvasGroup.alpha = 1;
        this.onEndOpen.Invoke();
        onOpened?.Invoke();
    }
    public void ShowImmediate()
    {
        if (IsShow)
            return;

        state = UIPanelState.EndShow;
        animator.enabled = false;
        canvasGroup.alpha = 1;
        onEndOpen?.Invoke();
    }
    public void Hide(float delay = 0)
    {
        Hide(delay, null);
    }
    public void Hide(UnityAction onClosed)
    {
        Hide(0, onClosed);
    }
    public void Hide(float delay, UnityAction onClosed)
    {
        if (!IsShow)
            return;

        if (delay == 0)
            state = UIPanelState.BeginHide;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        hideCoroutine = StartCoroutine(IHide(delay, onClosed));
    }
    public IEnumerator HideCoroutine()
    {
        yield return IHide(0, null);
    }
    private IEnumerator IHide(float delay, UnityAction onClosed)
    {
        yield return new WaitForSeconds(delay);

        UIManager.Instance.ActivePanels.Remove(this);

        state = UIPanelState.BeginHide;
        onBeginHide.Invoke();

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = false;
        animator.enabled = true;
        animator.speed = hideSpeed;
        animator.SetTrigger(HideAnimationName.Trim());

        yield return animator.WaitNormalizedTimeCoroutine(0, 1);

        state = UIPanelState.EndHide;
        animator.enabled = false;
        canvasGroup.alpha = 0;

        this.onEndHide.Invoke();
        onClosed?.Invoke();
    }
    public void HideImmediate()
    {
        if (!IsShow)
            return;

        state = UIPanelState.EndHide;
        animator.enabled = false;
        canvasGroup.alpha = 0;
        onEndHide?.Invoke();
    }
    public void Switch(float delay = 0)
    {
        if (IsShow)
            Hide(delay);
        else
            Show(delay);
    }
}