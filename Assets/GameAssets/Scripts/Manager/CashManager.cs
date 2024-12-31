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
    public float CashAmount = 0;
    public float CurrentWinings;

    // Start is called before the first frame update
    void Start()
    {
        float MoneyIntheBank = 0;
        if (PlayerPrefs.HasKey("TotalCash"))
        {
            MoneyIntheBank = PlayerPrefs.GetFloat("TotalCash");
        }
        else
        {
            MoneyIntheBank = 2000;
        }
        
        CashAmount = MoneyIntheBank;
        updateThecashUi();
    }

    private void Update ()
    {
        if (CommandCentre.Instance)
        {
            if (CommandCentre.Instance.DemoManager_.IsDemo)
            {
                CashAmount = 2000;
            }
            else
            {
                if (CashAmount <= 0)
                {
                    CashAmount = 2000;
                }
                else
                {
                    if (CommandCentre.Instance.WinLoseManager_.IsWin())
                    {
                        CashAmount = CommandCentre.Instance.APIManager_.betUpdaterAPI_.updateBetResponse.new_wallet_balance;
                    }
                    else
                    {
                        CashAmount = CommandCentre.Instance.APIManager_.betPlacingAPI_.response.new_wallet_balance;
                    }
                    
                }
            }
            updateThecashUi();
        }
    }

    public void SaveCashAmount()
    {
        PlayerPrefs.SetFloat("TotalCash", Mathf.FloorToInt(CashAmount));
    }

    public void updateThecashUi ()
    {
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            CashAmountText [1].SetText("DEMO MODE");
        }
        else
        {
            CashAmountText [0].SetText(Mathf.FloorToInt(CashAmount).ToString("F2"));
        }
        SaveCashAmount();
    }

    public void UpdateWinnings ()
    {
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            WinCashAmountText [1].text = CurrentWinings.ToString("F2");
        }
        else
        {
            WinCashAmountText [0].text = CurrentWinings.ToString("F2");
        }
    }

    public void IncreaseWinings (float Amount)
    {
        CurrentWinings = CurrentWinings + Amount;
        UpdateWinnings();
    }

    public void ResetWinings ()
    {
        CurrentWinings = 0;
        UpdateWinnings ();
    }
}
