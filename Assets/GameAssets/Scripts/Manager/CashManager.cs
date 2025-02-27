using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions.Must;

public class CashManager : MonoBehaviour
{
    public List<TextMeshProUGUI> CashAmountText = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> WinCashAmountText = new List<TextMeshProUGUI>();
    public double CashAmount = 0f;
    public float CurrentWinings;

    public void UpdateCashAmount(float amount )
    {
        CashAmount = amount;
        updateThecashUi();
    }

    private void Update ()
    {
        if (CommandCentre.Instance)
        {
            updateThecashUi();
        }
    }

    public void IncreaseCash ( float amount )
    {
        Debug.Log("Increasing Cash by: " + amount);
        CashAmount += amount;
        updateThecashUi();
        Debug.Log("New Cash Amount: " + CashAmount);
    }


    public void DecreaseCash ( float amount )
    {
        Debug.Log("Decreasing Cash by: " + amount);
        CashAmount -= amount;
        if (CashAmount < 0)
        {
            CashAmount = 0;
        }

        updateThecashUi();
        Debug.Log("New Cash Amount: " + CashAmount);
    }


    public void SaveCashAmount()
    {
        PlayerPrefs.SetString("TotalCash", CashAmount.ToString());
    }

    public void updateThecashUi ()
    {
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            CashAmountText [1].text = "DEMO MODE";
        }
        else
        {
            CashAmountText [0].text = CashAmount.ToString("N2");
        }
        SaveCashAmount();
    }

    public void UpdateWinnings ()
    {
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            WinCashAmountText [1].text = CurrentWinings.ToString("N2");
        }
        else
        {
            WinCashAmountText [0].text = CurrentWinings.ToString("N2");
        }
    }

    public void IncreaseWinings (float Amount)
    {
        CurrentWinings = CurrentWinings + Amount;
        UpdateWinnings();
        if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            CommandCentre.Instance.FreeGameManager_.winAmount = CurrentWinings;
        }
    }

    public void ResetWinings ()
    {
        if(CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            CommandCentre.Instance.FreeGameManager_.winAmount = CurrentWinings;
        }
        CurrentWinings = 0;
        UpdateWinnings ();
    }
}
