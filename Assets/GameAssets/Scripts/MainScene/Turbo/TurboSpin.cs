using UnityEngine;
using UnityEngine.UI;

public class TurboSpin : MonoBehaviour
{
    public bool IsTurboSpin = false;

    public void IsTutboSpinPressed ()
    {
        if (GetComponentInChildren<Toggle>().isOn)
        {
           CommandCentre.Instance.TurboManager_.EnableTurbospin();
        }
        else
        {
            CommandCentre.Instance.TurboManager_.DisableTurbospin();
        }
        IsTurboSpin = GetComponentInChildren<Toggle>().isOn;
    }
}
