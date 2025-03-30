using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class UIToggleSwitchAnimation : MonoBehaviour
{
    private Toggle toggle;
    private Animator animator;

    [SerializeField] private string onTrigger = "On";
    [SerializeField] private string offTrigger = "Off";

    public string OnTrigger
    {
        get
        {
            return onTrigger;
        }
        set
        {
            onTrigger = value;
        }
    }
    public string OffTrigger
    {
        get
        {
            return offTrigger;
        }
        set
        {
            offTrigger = value;
        }
    }


    private Coroutine coroutine;

    public virtual void Start()
    {
        toggle = GetComponentInParent<Toggle>();
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
        //animator.enabled = true;

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(ISetTrigger(triggerName));
    }
    private IEnumerator ISetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
        yield return animator.WaitNormalizedTimeCoroutine(0, 1);
        //animator.enabled = false;
    }
}
