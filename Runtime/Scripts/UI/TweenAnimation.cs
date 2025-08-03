using DG.Tweening;
using UnityEngine;

public abstract class TweenAnimation : ScriptableObject
{
    public abstract Tween Play(object target);
}
