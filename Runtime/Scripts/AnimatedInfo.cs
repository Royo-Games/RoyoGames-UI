using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatedInfo : MonoBehaviour
{
    [SerializeField] string infoName;
    [SerializeField] string animationTriggerName;
    [SerializeField] float animationSpeed = 1;

    public string InfoName => infoName;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    internal void Play(AnimatedInfoPanel controller)
    {
        StartCoroutine(IPlay(controller));
    }
    private IEnumerator IPlay(AnimatedInfoPanel controller)
    {
        animator.speed = animationSpeed;
        animator.SetTrigger(animationTriggerName);
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => IsAnimationEnding());
        controller.EndInfo(this);
    }
    private bool IsAnimationEnding()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return !animator.IsInTransition(0) && stateInfo.normalizedTime >= 1;
    }
}
