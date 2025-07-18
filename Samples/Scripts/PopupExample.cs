using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupExample : MonoBehaviour
{
    [SerializeField] UIPopup popup;
    [SerializeField] RectTransform popupPoint;

    public void OpenPopup()
    {
        popup.SetPosition(popupPoint.position, 65);
        popup.Show();
    }
}
