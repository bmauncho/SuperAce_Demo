using UnityEngine;
using UnityEngine.UI;

public class AutoSpin : MonoBehaviour
{
    public bool IsAutoSpin = false;

    public void IsAutoSpinPressed ()
    {
        if (GetComponentInChildren<Toggle>().isOn)
        {
            CommandCentre.Instance.AutoSpinManager_.EnableAutoSpin();
        }
        else
        {
            CommandCentre.Instance.AutoSpinManager_.DisableAutoSpin();
        }
        IsAutoSpin = GetComponentInChildren<Toggle>().isOn;
    }
}
