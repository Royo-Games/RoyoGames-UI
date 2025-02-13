using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle), typeof(Animator))]
public class ToggleAnimation : MonoBehaviour
{
    private Toggle toggle;
    private Animator animator;

    [SerializeField] string onTrigger;
    [SerializeField] string offTrigger;

    private Coroutine coroutine;

    public virtual void Start()
    {
        toggle = GetComponent<Toggle>();
        animator= GetComponent<Animator>(); 
        toggle.onValueChanged.AddListener(OnChanged);
        SetTrigger(toggle.isOn);
    }
    public virtual void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnChanged);
    }
    public virtual void OnChanged(bool isOn)
    {
        SetTrigger(isOn);
    }
    private void SetTrigger(bool isOn)
    {
        SetTrigger(isOn ? onTrigger : offTrigger);
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
