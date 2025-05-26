using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening.Core.Easing;
using UnityEngine.UI;

public class BetManager : MonoBehaviour
{
    public TMP_Text [] CurrentBetAmount;
    public BetMenu BetMenu_;
    public string BetAmount;
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
        //Debug.Log("Deactivated all bets.");
    }

    public void SetCurrentBetAmount ( string amount )
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
        CurrentBetAmount [index].text = BetAmount;
    }

    public void refreshBetSlip ()
    {
        int index = 0;
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            index = 1;
            BetAmount = "10";
        }
        else
        {
            index = 0;
            BetAmount = "2";
        }
        UpdateBetAmount(index);
    }

    [ContextMenu("showBetSelected")]
    public void showBetSelected ()
    {
        BetButtonsController bbc = BetMenu_.betButtonsController_;
        for (int i = 0;i<bbc.BetButtons.Count;i++)
        {
            string amount = bbc.BetButtons [i].Amount;
            if (amount == BetAmount)
            {
                bbc.BetButtons [i].GetComponentInChildren<Button>().Select ();
            }
        }
    }
}
