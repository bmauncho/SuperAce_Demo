using TMPro;
using UnityEngine;

public class BalanceController : MonoBehaviour
{
    public int CurrentBalance = 0;
    public TMP_Text Balance;

    public void IncreaseBalance ()
    {
        CurrentBalance++;

        Balance.text = $"{CurrentBalance}";
    }

    public void DecreaseBalance ()
    {
        CurrentBalance--;
        if (CurrentBalance <= 1) { CurrentBalance = 1; }
        Balance.text = $"{CurrentBalance}";
    }
}
