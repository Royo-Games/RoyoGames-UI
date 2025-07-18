using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupExample : MonoBehaviour
{
    [SerializeField] UIPopup popup;
    [SerializeField] RectTransform popupPoint;

    public Transform Cube;

    public void OpenPopup()
    {
        popup.SetPositionByWorld(Cube.position, 65, Camera.main);
        popup.Show();
    }
}
