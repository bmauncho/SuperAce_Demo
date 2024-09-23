using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsPointerChecker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown ( PointerEventData eventData )
    {
        if (GetComponentInParent<WinRatioController>())
        {
            //GetComponentInParent<WinRatioController>().isPointerDown = true;
            //GetComponentInParent<WinRatioController>().isHolding = true;
        }
        else if (GetComponentInParent<BalanceController>())
        {
            GetComponentInParent<BalanceController>().isPointerDown = true;
            GetComponentInParent<BalanceController>().isHolding = true;
        }

        Debug.Log("Toggle pressed");
    }

    public void OnPointerUp ( PointerEventData eventData )
    {
        if (GetComponentInParent<WinRatioController>())
        {
            //GetComponentInParent<WinRatioController>().isPointerDown = false;
            //GetComponentInParent<WinRatioController>().isHolding = true;
            //GetComponentInParent<WinRatioController>().holdTime = 0;
        }
        else if (GetComponentInParent<BalanceController>())
        {
            GetComponentInParent<BalanceController>().isPointerDown = false;
            GetComponentInParent<BalanceController>().isHolding = false;
            GetComponentInParent<BalanceController>().holdTime = 0;
        }
        Debug.Log("Toggle released");
    }
}
