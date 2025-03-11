using System.Collections;
using UnityEngine;

public class UIPointer : MonoBehaviour
{
    [SerializeField][HideInInspector] private bool flip;
    [SerializeField][HideInInspector] private float angle;

    [SerializeField][HideInInspector] private string showTrigger;
    [SerializeField][HideInInspector] private string hideTrigger;
    [SerializeField][HideInInspector] private string highlightTrigger;

    [SerializeField][HideInInspector] private string beginDragTrigger;
    [SerializeField][HideInInspector] private string endDragTrigger;

    [SerializeField][HideInInspector] private float startToTargetDuration = 2;
    [SerializeField][HideInInspector] private float targetToStartDuration = 1;

    [SerializeField][HideInInspector] private Vector2 startToTargetAmplitude;
    [SerializeField][HideInInspector] private Vector2 targetToStartAmplitude;

    public virtual bool Flip
    {
        get
        {
            return flip;
        }
        set
        {
            flip = value;
            UpdateEulerAngles();
        }
    }
    public virtual float Angle
    {
        get
        {
            return angle;
        }
        set
        {
            angle = value;
            UpdateEulerAngles();
        }
    }

    public string ShowTrigger
    {
        get { return showTrigger; }
        set { showTrigger = value; }
    }

    public string HideTrigger
    {
        get { return hideTrigger; }
        set { hideTrigger = value; }
    }

    public string HighlightTrigger
    {
        get { return highlightTrigger; }
        set { highlightTrigger = value; }
    }

    public string BeginDragTrigger
    {
        get { return beginDragTrigger; }
        set { beginDragTrigger = value; }
    }
    public string EndDragTrigger
    {
        get { return endDragTrigger; }
        set { endDragTrigger = value; }
    }
    public float StartToTargetDuration
    {
        get { return startToTargetDuration; }
        set {  startToTargetDuration = value; }
    }
    public float TargeteToStartDuration
    {
        get { return targetToStartDuration; }
        set { targetToStartDuration = value; }
    }
    public Vector2 StartToTargetAmplitude
    {
        get { return startToTargetAmplitude; }
        set { startToTargetAmplitude = value; }
    }
    public Vector2 TargetToStartAmplitude
    {
        get { return targetToStartAmplitude; }
        set { targetToStartAmplitude = value; }
    }

    public bool IsShow { get; private set; }

    public Vector2 StartPosition { get; set; }
    public Vector2 TargetPosition { get; set; }

    private State currentState;
    public State CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;

            switch (currentState)
            {
                case State.StartToTarget:
                    currentDragTrigger = beginDragTrigger;
                    currentDragDuration = startToTargetDuration;
                    currentDragAmplitude = startToTargetAmplitude;
                    startPos = StartPosition;
                    targetPos = TargetPosition;
                    break;
                case State.TargetToStart:
                    currentDragTrigger = endDragTrigger;
                    currentDragDuration = targetToStartDuration;
                    currentDragAmplitude = targetToStartAmplitude;
                    startPos = TargetPosition;
                    targetPos = StartPosition;
                    break;
            }
        }
    }
    public float Progress { get; private set; }

    private Vector2 startPos;
    private Vector2 targetPos;
    private Animator animator;
    private Coroutine coroutine;

    private bool isDragging;
    private string currentDragTrigger;
    private float currentDragDuration;
    private Vector2 currentDragAmplitude;

    private Canvas canvas;

    protected virtual void OnValidate()
    {
        Flip = flip;
        Angle = angle;
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
        canvas = GetComponentInParent<Canvas>(true);
    }
    private void Update()
    {
        if (IsShow && isDragging)
        {
            Progress += 1.0f / currentDragDuration * Time.deltaTime;

            if (Progress > 1)
                Progress = 1;

            Vector3 pos = transform.position;

            float yCurveheight = RoyoMath.Parabola(Progress, currentDragAmplitude.y * canvas.scaleFactor);
            float xCurveheight = RoyoMath.Parabola(Progress, currentDragAmplitude.x * canvas.scaleFactor);

            pos = Vector2.Lerp(startPos, targetPos, Progress) + new Vector2(xCurveheight, yCurveheight);

            transform.position = pos;

            if (Progress == 1)
            {
                isDragging = false;
                Progress = 0;
                CurrentState = CurrentState == State.StartToTarget ? State.TargetToStart : State.StartToTarget;

                StopCurrentCoroutine();
                coroutine = StartCoroutine(PlayDragAnimationCoroutine());
            }
        }
    }
    public enum State
    {
        None,
        StartToTarget,
        TargetToStart
    }
    public void ShowHighlight()
    {
        gameObject.SetActive(true);

        StopCurrentCoroutine();
        coroutine = StartCoroutine(ShowHighlightCoroutine());
    }
    public IEnumerator ShowHighlightCoroutine()
    {
        bool tempIsShow = IsShow;
        IsShow = true;
        isDragging = false;

        if (!tempIsShow)
            yield return PlayAnimationCoroutine(showTrigger);

        yield return PlayAnimationCoroutine(highlightTrigger);
    }
    public void ShowDrag()
    {
        gameObject.SetActive(true);

        StopCurrentCoroutine();
        coroutine = StartCoroutine(ShowDragCoroutine());
    }
    public IEnumerator ShowDragCoroutine()
    {
        CurrentState = State.StartToTarget;
        Progress = 0;
        transform.position = StartPosition;

        bool tempIsShow = IsShow;
        IsShow = true;
        isDragging = false;

        if (!tempIsShow)
            yield return PlayAnimationCoroutine(showTrigger);

        yield return PlayDragAnimationCoroutine();
    }
    public void Hide()
    {
        if (!IsShow)
            return;

        StopCurrentCoroutine();
        coroutine = StartCoroutine(HideCoroutine());
    }
    public IEnumerator HideCoroutine()
    {
        IsShow = false;
        yield return PlayAnimationCoroutine(HideTrigger);
        isDragging = false;
    }
    private IEnumerator PlayDragAnimationCoroutine()
    {
        yield return PlayAnimationCoroutine(currentDragTrigger);
        isDragging = true;
    }
    private bool PlayAnimation(string triggerName)
    {
        if (string.IsNullOrEmpty(triggerName))
        {
            return false;
        }
        else
        {
            StopCurrentCoroutine();
            coroutine = StartCoroutine(PlayAnimationCoroutine(triggerName));
            return true;
        }
    }
    private IEnumerator PlayAnimationCoroutine(string triggerName)
    {
        animator.SetTrigger(triggerName);
        yield return animator.WaitNormalizedTimeCoroutine(0, 1);
    }
    private void UpdateEulerAngles()
    {
        transform.eulerAngles = new Vector3(0, Flip ? 180 : 0, -Angle);
    }
    private void StopCurrentCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
