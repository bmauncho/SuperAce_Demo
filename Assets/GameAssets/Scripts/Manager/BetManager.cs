using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening.Core.Easing;

public class BetManager : MonoBehaviour
{
    public TMP_Text [] CurrentBetAmount;
    public BetMenu BetMenu_;
    public float BetAmount;
    public int rounds = 0;

    private void Start ()
    {
        refreshBetSlip();
       
    }
    private void Update ()
    {
      
    }

    // Deactivates all bets without clearing the list
    void DeactivateAllBets ()
    {
        List<Bet> betButtons = new List<Bet>(BetMenu_.betButtonsController_.BetButtons);
        foreach (var button in betButtons)
        {
            button.IsPressed = false;
        }
        Debug.Log("Deactivated all bets.");
    }

    public void SetCurrentBetAmount ( float amount )
    {
        BetAmount = amount;
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            UpdateBetAmount(1);
        }
        else
        {
            UpdateBetAmount(0);
        }
        DeactivateAllBets();
    }

    public void UpdateBetAmount (int index)
    {
        CurrentBetAmount [index].text = BetAmount.ToString();
    }

    public void refreshBetSlip ()
    {
        int index = 0;
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            index = 1;
            BetAmount = 10f;
        }
        else
        {
            index = 0;
            BetAmount = 2;
        }
        UpdateBetAmount(index);
    }
}
