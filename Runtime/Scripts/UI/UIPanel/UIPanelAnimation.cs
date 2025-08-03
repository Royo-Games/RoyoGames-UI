using DG.Tweening;
using UnityEngine;

public abstract class UIPanelAnimation : TweenAnimation
{
    public abstract Tween Play(UIPanel panel);

    public override Tween Play(object target)
    {
        return Play((UIPanel)target);
    }
}
