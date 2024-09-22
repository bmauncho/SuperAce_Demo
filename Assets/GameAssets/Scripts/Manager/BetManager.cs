using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BetManager : MonoBehaviour
{
    public TMP_Text []CurrentBetAmount;
    public BetMenu BetMenu_;
    public float BetAmount;

    private void Start ()
    {
       refreshBetSlip();
    }
    void DeactivateAllBets ()
    {
        List<Bet> Betbuttons = new List<Bet>(BetMenu_.betButtonsController_.BetButtons);
        if(Betbuttons.Count > 0) { Betbuttons.Clear(); }
        for (int i = 0;i<Betbuttons.Count;i++)
        {
            if (Betbuttons [i].IsPressed)
            {
                Betbuttons [i].IsPressed = false;
            }
        }
        Debug.Log("Deactivate isPressed");
    }

    public void SetCurrentbetAmount (float Amount)
    {
        BetAmount = Amount;
        UpdateBetAmount();
        DeactivateAllBets ();
    }

    public void UpdateBetAmount ()
    {
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            CurrentBetAmount [1].text = BetAmount.ToString();
        }
        else
        {
            CurrentBetAmount [0].text = BetAmount.ToString();
        }
    }

    public void refreshBetSlip ()
    {
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            BetAmount = 10;
        }
        else
        {
            BetAmount = 2;
        }

        UpdateBetAmount();
    }
}
