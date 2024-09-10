using TMPro;
using UnityEngine;

public class WinRatioController : MonoBehaviour
{
    public int CurrentWinRatio = 0;
    public TMP_Text WinRatio;

    public void IncreaseWinRatio ()
    {
        CurrentWinRatio++;

        WinRatio.text = $"{CurrentWinRatio}X";
    }

    public void DecreaseWinRatio ()
    {
        CurrentWinRatio--;
        if (CurrentWinRatio <= 1) { CurrentWinRatio = 1; }
        WinRatio.text = $"{CurrentWinRatio}X";
    }
}
