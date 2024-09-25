using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            rtp = CommandCentre.Instance.LargeBets_FetchValues.PercentageValue / 100;
        }

        if (rounds <= 1)
        {
            rounds = 1;
        }
        Debug.Log(AdjustedBetAmount);
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
        UpdateBetAmount();
        DeactivateAllBets();
    }

    public void UpdateBetAmount ()
    {
        int index = CommandCentre.Instance.DemoManager_.IsDemo ? 1 : 0;
        CurrentBetAmount [index].text = BetAmount.ToString();
    }

    public void refreshBetSlip ()
    {
        BetAmount = CommandCentre.Instance.DemoManager_.IsDemo ? 10f : 2f;
        AdjustedBetAmount = BetAmount;
        UpdateBetAmount();
    }

    // Adjust bet based on round number, adding variance to larger bets
    float AdjustBet ( float baseAmount  , float maxBetMultiplier )
    {
        int round = rounds;
        rtp = CommandCentre.Instance.LargeBets_FetchValues.PercentageValue/100;
        // Scale variance with the round number, allowing for an increase over time
        float betMultiplier = Mathf.Clamp(1 + ( highVariance * round / 10 ) , 1 , maxBetMultiplier);
        return baseAmount * betMultiplier* rtp;
    }

    // Add a more complex betting logic here (progressive betting system)
    public void ProgressiveBetting ( bool isLoss)
    {
        // Base bet amount
        float baseBetAmount = BetAmount;  // Change this to your desired base bet amount
        if (isLoss)
        {
            // Increase bet on loss
            AdjustedBetAmount = Mathf.Min(AdjustBet(BetAmount * 1.5f , 100) , 100);  // Apply AdjustBet to scale with round
        }
        else
        {
            // Decrease bet on win
            AdjustedBetAmount = AdjustBet(Mathf.Max(BetAmount / 2f , 2) , 100);  // Apply AdjustBet to scale with round
        }

        UpdateBetAmount();
    }


    // Large bet system logic 
    public void LargeBetSystem ( bool lastRoundWon )
    {
        int round = rounds;
        float playerBalance = CommandCentre.Instance.CashManager_.CashAmount;
        // Determine the risk profile based on the player's current betting behavior
        string riskProfile = DetermineRiskProfile(BetAmount , CommandCentre.Instance.CardManager_.winRate , CommandCentre.Instance.CashManager_.CashAmount);
        rtp = CommandCentre.Instance.LargeBets_FetchValues.PercentageValue / 100;
        float baseBet = 10;
        float maxBet = playerBalance * 0.1f; // 10% max of player balance
        float aggressiveMultiplier = riskProfile == "aggressive" ? 1.5f : 1.1f;
        float moderateMultiplier = riskProfile == "moderate" ? 1.2f : 1.1f; // Moderate risk multiplier
        float conservativeMinBet = riskProfile == "conservative" ? Mathf.Min(5 , baseBet) : baseBet;

        // Adjust the bet based on the outcome of the last round
        if (lastRoundWon)
        {
            // Increase bet based on risk profile after a win
            if (riskProfile == "aggressive")
            {
                AdjustedBetAmount = Mathf.Min(BetAmount * aggressiveMultiplier * rtp, maxBet); // More aggressive scaling on win
            }
            else if (riskProfile == "moderate")
            {
                AdjustedBetAmount = Mathf.Min(BetAmount * moderateMultiplier *rtp, maxBet); // Moderate scaling on win
            }
        }
        else
        {
            // Enter recovery mode on loss streak, smaller bets
            AdjustedBetAmount = Mathf.Max(conservativeMinBet , BetAmount * 0.75f);

            // Additional recovery logic for every 3 losses
            if (!lastRoundWon && round % 3 == 0)
            {
                AdjustedBetAmount *= 0.5f; // Recovery mode every 3 losses
            }
        }

        // Additional aggressive betting logic for high player balances
        if (playerBalance > 1000 && round % 5 == 0)
        {
            AdjustedBetAmount *= aggressiveMultiplier; // Bet boost every 5th round for high-risk players
        }

        Debug.Log($"Adjusted Bet Amount for {riskProfile} profile: {AdjustedBetAmount}");
        UpdateBetAmount();
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

}
