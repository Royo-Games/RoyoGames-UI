using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UIPanelFadeAnimation", menuName = "Scriptable Objects/UIPanelFadeAnimation")]
public class UIPanelFadeAnimation : UIPanelAnimation
{
    public float Duration;
    [Range(0, 1)]
    public float TargetAlpha;
    public Ease Ease = Ease.Linear;

    public override Tween Play(UIPanel panel)
    {
        return panel.CanvasGroup.DOFade(TargetAlpha, Duration).SetEase(Ease);
    }
}
