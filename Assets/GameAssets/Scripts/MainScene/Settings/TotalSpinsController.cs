using TMPro;
using UnityEngine;

public class TotalSpinsController : MonoBehaviour
{
    public int TotalSpins = 10;
    public int CurrentSpns = 0;
    public TMP_Text Spins;
    public void IncreaseSpins ()
    {
        CurrentSpns++;
        if(CurrentSpns >= TotalSpins)
        {
            CurrentSpns = TotalSpins;
        }
        Spins.text = CurrentSpns.ToString();
    }

    public void DecreaseSpins ()
    {
        CurrentSpns--;
        if (CurrentSpns <= 5) { CurrentSpns = 5; }
        Spins.text = CurrentSpns.ToString();
    }

    public void SetStaticSpins (int amount)
    {
        CurrentSpns = amount;
        Spins.text = CurrentSpns.ToString();
    }
}
