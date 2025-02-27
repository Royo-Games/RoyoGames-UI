using UnityEditor;

[CustomEditor(typeof(AnimatedInfo))]
public class AnimatedInfoInspector : Editor
{
    private AnimatedInfo animatedInfo;
    private StringArrayPopupProperty triggerName;

    private void OnEnable()
    {
        animatedInfo = (AnimatedInfo)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
