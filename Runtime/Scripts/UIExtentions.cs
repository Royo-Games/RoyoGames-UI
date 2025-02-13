using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIExtentions
{
    private static List<RaycastResult> raycastResultList = new List<RaycastResult>();

    public static bool IsPointerOverUIObject(this EventSystem eventSystem)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(eventSystem);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        raycastResultList.Clear();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, raycastResultList);
        return raycastResultList.Count > 0;
    }
}
