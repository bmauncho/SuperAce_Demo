using TMPro;
using UnityEngine;

public class Bet : MonoBehaviour
{
    public TMP_Text BetAmount;
    public string Amount;
    public bool IsPressed;
    
    public void SetBet ()
    {
        SetBetAmount(Amount);
    }
   
    void SetBetAmount(string amount )
    {
        BetAmount.text = amount.ToString();
    }

    public void BetIsSet ()
    {
        IsPressed = true;
        CommandCentre.Instance.BetManager_.SetCurrentBetAmount(Amount);
        Invoke(nameof(DeactivateBetMenu) , .25f);
    }

    public void DeactivateBetMenu ()
    {
        CommandCentre.Instance.MainMenuController_.CloseBets();
    }
    

}
