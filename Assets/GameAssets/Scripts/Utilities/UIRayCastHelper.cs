using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIRaycastHelper
{
    /// <summary>
    /// Returns true if the pointer is over a UI GameObject with the given component.
    /// </summary>
    public static bool IsPointerOverUI<T> () where T : MonoBehaviour
    {
        if (EventSystem.current == null) return false;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData , results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<T>() != null)
            {
                return true;
            }
        }

        return false;
    }
}
