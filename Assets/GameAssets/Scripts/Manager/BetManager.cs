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
    public float AdjustedBetAmount;

    public float rtp = 0.95f;    // Return to Player, 95%
    public float highVariance = 0.25f;  // High risk, higher reward variance
    public int rounds = 0;

    private void Start ()
    {
        refreshBetSlip();
       
    }
    private void Update ()
    {
        if (CommandCentre.Instance)
        {
            if (BetAmount >=50)
            {
                rtp = CommandCentre.Instance.LargeBets_FetchValues.PercentageValue / 100;
            }
            else
            {
                rtp = .95f;
            }
            

            if (rounds <= 1)
            {
                rounds = 1;
            }
        }
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
        AdjustedBetAmount = BetAmount;
        UpdateBetAmount(index);
    }

    // Adjust bet based on round number, adding variance to larger bets
    float AdjustBet ( float baseAmount  , float maxBetMultiplier )
    {
        int round = rounds;
        rtp = CommandCentre.Instance.LargeBets_FetchValues.PercentageValue/100;
        // Scale variance with the round number, allowing for an increase over time
        float betMultiplier = Mathf.Clamp(1 + ( highVariance * round / 10 ) , 1 , maxBetMultiplier);
        return baseAmount * betMultiplier * rtp;
    }

    // Large bet system logic 
    public void LargeBetSystem ( bool lastRoundWon )
    {
        // Access managers
        CardManager cardManager = CommandCentre.Instance.CardManager_;
        CashManager cashManager = CommandCentre.Instance.CashManager_;

        // Initialize variables
        string riskProfile = DetermineRiskProfile(BetAmount , cardManager.winRate , cashManager.CashAmount);
        float baseBet = 10f;
        float playerBalance = cashManager.CashAmount;
        float maxBet = playerBalance * 0.1f; // 10% of player balance
        float aggressiveMultiplier = 1.5f;
        float moderateMultiplier = 1.2f;
        float conservativeMinBet = Mathf.Min(5f , baseBet);
        float adjustedBetAmount = BetAmount; // Default to current bet amount
        float rtp_ = rtp; // Assuming you have this function for RTP calculation

        // Risk profile adjustment
        switch (riskProfile)
        {
            case "aggressive":
                aggressiveMultiplier = 1.5f;
                break;
            case "moderate":
                moderateMultiplier = 1.2f;
                break;
            case "conservative":
                conservativeMinBet = baseBet;
                break;
            default:
                Debug.LogError("Unknown risk profile: " + riskProfile);
                return;
        }

        // Bet adjustment logic based on last round outcome
        if (lastRoundWon)
        {
            // Win scenario
            switch (riskProfile)
            {
                case "aggressive":
                    adjustedBetAmount = BetAmount * aggressiveMultiplier * rtp;
                    break;

                case "moderate":
                    adjustedBetAmount = BetAmount * moderateMultiplier * rtp;
                    break;

                default:
                    adjustedBetAmount = baseBet; // No change for other profiles
                    break;
            }
        }
        else
        {
            // Loss scenario with conservative approach
            adjustedBetAmount = Mathf.Max(conservativeMinBet , BetAmount * 0.75f);

            // Recovery mode for every 3 losses
            if (rounds % 3 == 0)
            {
                adjustedBetAmount *= 0.5f; // Halve the bet every 3 losses
            }
        }

        // Aggressive betting logic for high player balance
        if (playerBalance >= 1000f && rounds % 5 == 0)
        {
            adjustedBetAmount *= aggressiveMultiplier;
        }

        // Debug log to track bet changes
        Debug.Log($"Risk Profile: {riskProfile}, Adjusted Bet Amount: {adjustedBetAmount}, Player Balance: {playerBalance}");

        // Assign final adjusted bet amount
        AdjustedBetAmount = adjustedBetAmount;
    }



    public string DetermineRiskProfile ( float averageBet , float winRate , float balance )
    {
        if (averageBet > balance * 0.1f && winRate > 0.5f)
        {
            return "aggressive";
        }
        else if (averageBet <= balance * 0.05f && winRate < 0.5f)
        {
            return "conservative";
        }
        else
        {
            return "moderate";
        }
    }

    public float GetBetAmount ()
    {
        if (BetAmount > 0 && BetAmount<50)
        {
            if (CommandCentre.Instance.DemoManager_.IsDemo)
            {
                float betMultiplier = ( 1 + ( highVariance * rounds / 10 ) );
                BetAmount *= 2;
                AdjustedBetAmount = BetAmount * betMultiplier * rtp;
            }
            else
            {
                float betMultiplier = ( 1 + ( highVariance * rounds / 10 ) );
                //progressive betting
                if (CommandCentre.Instance.WinLoseManager_.IsLastRoundWon)
                {
                    BetAmount /= 2f;
                }
                else
                {
                    BetAmount *= 1.5f;
                }

                AdjustedBetAmount = BetAmount * betMultiplier * rtp;
            }
            
        }
        else
        {
            LargeBetSystem(CommandCentre.Instance.WinLoseManager_.IsLastRoundWon);
        }
        return AdjustedBetAmount;
    }
}
