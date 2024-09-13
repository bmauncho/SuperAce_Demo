using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerChecker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown ( PointerEventData eventData )
    {
        GetComponentInParent<AutoSpin>().isPointerDown = true;
        GetComponentInParent<AutoSpin>().isHolding = true;
        Debug.Log("Toggle pressed");
    }

    public void OnPointerUp ( PointerEventData eventData )
    {
        GetComponentInParent<AutoSpin>().isPointerDown = false;
        GetComponentInParent<AutoSpin>().isHolding = false;
        GetComponentInParent<AutoSpin>().holdTime = 0f;
        Debug.Log("Toggle released");
    }
}
