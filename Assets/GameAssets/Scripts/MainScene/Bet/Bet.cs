using TMPro;
using UnityEngine;

public class Bet : MonoBehaviour
{
    public TMP_Text BetAmount;
    public float Amount;
    public bool IsPressed;
    
    public void SetBet ()
    {
        SetBetAmount(Amount);
    }
   
    void SetBetAmount(float amount )
    {
        BetAmount.text = amount.ToString();
    }

    public void BetIsSet ()
    {
        IsPressed = true;
        CommandCentre.Instance.BetManager_.SetCurrentbetAmount(Amount);
    }
}
