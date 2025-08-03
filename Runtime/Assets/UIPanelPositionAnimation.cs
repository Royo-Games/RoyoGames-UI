using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UIPanelPositionAnimation", menuName = "Scriptable Objects/UIPanelPositionAnimation")]
public class UIPanelPositionAnimation : UIPanelAnimation
{
    public float Duration;

    [Header("Alpha Settings")]
    public float TargetAlpha;
    public Ease AlphaEase = Ease.Linear;

    [Header("Position Settings")]
    public Vector3 StartPosition = Vector3.zero;
    public Vector3 TargetPosition = Vector3.zero;
    public Ease ScaleEase = Ease.Linear;
    public float ScaleEaseOverShoot = 3;

    public override Tween Play(UIPanel panel)
    {
        panel.Body.localPosition = StartPosition;

        Sequence sequence = DOTween.Sequence();
        sequence.Join(panel.CanvasGroup.DOFade(TargetAlpha, Duration).SetEase(AlphaEase));
        sequence.Join(panel.Body.DOLocalMove(TargetPosition, Duration).SetEase(ScaleEase, ScaleEaseOverShoot));

        return sequence;
    }
}
