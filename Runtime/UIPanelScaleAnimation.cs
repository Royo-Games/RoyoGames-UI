using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UIPanelScaleAnimation", menuName = "Scriptable Objects/UIPanelScaleAnimation")]
public class UIPanelScaleAnimation : UIPanelAnimation
{
    public float Duration;

    [Header("Alpha Settings")]
    public float TargetAlpha;
    public Ease AlphaEase = Ease.Linear;

    [Header("Scale Settings")]
    public Vector3 StartScale = Vector3.zero; 
    public Vector3 TargetScale = Vector3.zero;
    public Ease ScaleEase = Ease.Linear;
    public float ScaleEaseOverShoot = 3;

    public override Tween Play(UIPanel panel)
    {
        panel.Body.localScale = StartScale;

        Sequence sequence = DOTween.Sequence();
        sequence.Join(panel.CanvasGroup.DOFade(TargetAlpha, Duration).SetEase(AlphaEase));
        sequence.Join(panel.Body.DOScale(TargetScale, Duration).SetEase(ScaleEase, ScaleEaseOverShoot));

        return sequence;
    }
}
