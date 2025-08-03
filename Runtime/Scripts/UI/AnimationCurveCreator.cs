using UnityEngine;

public enum CurveType
{
    Linear,
    InBack,
    OutBack,
    InOutBack,
    InQuad,
    OutQuad,
    InOutQuad,
    HalfSine,
    FullSine
}

public static class AnimationCurveCreator
{
    public static AnimationCurve Create(CurveType curveType)
    {
        switch (curveType)
        {
            case CurveType.Linear:
                return new AnimationCurve(
                    new Keyframe(0f, 0f, 1f, 1f),
                    new Keyframe(1f, 1f, 1f, 1f));

            case CurveType.InBack:
                return new AnimationCurve(
                    new Keyframe(0f, 0f, 0f, 0f),
                    new Keyframe(0.3f, -0.5f, 0f, 0f),
                    new Keyframe(1f, 1f, 0f, 0f));

            case CurveType.OutBack:
                return new AnimationCurve(
                    new Keyframe(0f, 0f, 0f, 0f),
                    new Keyframe(0.7f, 1.5f, 0f, 0f),
                    new Keyframe(1f, 1f, 0f, 0f));

            case CurveType.InOutBack:
                return new AnimationCurve(
                    new Keyframe(0f, 0f, 0f, 0f),
                    new Keyframe(0.3f, -0.5f, 0f, 0f),
                    new Keyframe(0.7f, 1.5f, 0f, 0f),
                    new Keyframe(1f, 1f, 0f, 0f));

            case CurveType.InQuad:
                return new AnimationCurve(
                    new Keyframe(0f, 0f, 0f, 0f),
                    new Keyframe(1f, 1f, 2f, 2f));

            case CurveType.OutQuad:
                return new AnimationCurve(
                    new Keyframe(0f, 0f, 2f, 2f),
                    new Keyframe(1f, 1f, 0f, 0f));

            case CurveType.InOutQuad:
                return new AnimationCurve(
                    new Keyframe(0f, 0f, 0f, 0f),
                    new Keyframe(0.5f, 0.5f, 2f, 2f),
                    new Keyframe(1f, 1f, 0f, 0f));

            case CurveType.HalfSine:
                return new AnimationCurve(
                    new Keyframe(0f, 0f, 0f, 1f),
                    new Keyframe(0.5f, 1f, 0f, 0f),
                    new Keyframe(1f, 0f, -1f, 0f));

            case CurveType.FullSine:
                return new AnimationCurve(
                    new Keyframe(0f, 0f, 2f * Mathf.PI, 2f * Mathf.PI),
                    new Keyframe(0.25f, 1f, 0f, 0f),
                    new Keyframe(0.5f, 0f, -2f * Mathf.PI, -2f * Mathf.PI),
                    new Keyframe(0.75f, -1f, 0f, 0f),
                    new Keyframe(1f, 0f, 2f * Mathf.PI, 2f * Mathf.PI));
        }

        return null;
    }
}
