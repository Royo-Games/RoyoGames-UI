using UnityEngine;

public class AnimatedInfoExample : MonoBehaviour
{
    [SerializeField] UIPanel warning;
    [SerializeField] RectTransform button;

    public void Create()
    {
        warning.transform.position = button.position + new Vector3(0,500,0);
        warning.Show();
    }
}
