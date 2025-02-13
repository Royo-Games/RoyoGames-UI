using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string DownTrigger = "Down";
    public string UpTrigger = "Up";

    private Animator animator;

    private Coroutine coroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        SetTrigger(DownTrigger);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        SetTrigger(UpTrigger);
    }
    private void SetTrigger(string triggerName)
    {
        animator.enabled = true;

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(ISetTrigger(triggerName));
    }
    private IEnumerator ISetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
        yield return animator.WaitNormalizedTimeCoroutine(0, 1);
        animator.enabled = false;
    }
}
