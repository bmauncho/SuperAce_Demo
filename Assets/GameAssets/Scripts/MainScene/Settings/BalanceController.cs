using TMPro;
using UnityEngine;

public class BalanceController : MonoBehaviour
{
    public int CurrentBalance = 0;
    public TMP_Text Balance;

    public void IncreaseBalance ()
    {
        CurrentBalance+=10;

        Balance.text = $"{CurrentBalance}";
    }

    public void DecreaseBalance ()
    {
        CurrentBalance-=10;
        if (CurrentBalance <= 1) { CurrentBalance = 1; }
        Balance.text = $"{CurrentBalance}";
    }
}
