using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class CashManager : MonoBehaviour
{
    public List<TextMeshProUGUI> CashAmountText = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> WinCashAmountText = new List<TextMeshProUGUI>();
    public float CashAmount = 0;
    public float CurrentWinings;

    // Start is called before the first frame update
    void Start()
    {
        float MoneyIntheBank = PlayerPrefs.GetFloat("TotalCash");
        MoneyIntheBank = 2000;
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
        }
    }
    public void IncreaseCash(float amount)
    {
        CashAmount += amount;

        updateThecashUi();
    }

    public void DecreaseCash(float amount)
    {
        CashAmount -= amount;
        if(CashAmount < 0)
        {
            CashAmount = 0;
        }

        updateThecashUi();
    }

    [ContextMenu("BreakTheBank")]
    public void BreakTheBank()
    {
        CashAmount = 1000;
        PlayerPrefs.SetFloat("TotalCash", CashAmount);
        updateThecashUi();
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
            CashAmountText [0].SetText(Mathf.FloorToInt(CashAmount).ToString());
        }
        
    }

    public void UpdateWinnings ()
    {
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            WinCashAmountText [1].text = CurrentWinings.ToString();
        }
        else
        {
            WinCashAmountText [0].text = CurrentWinings.ToString();
        }
    }

    [ContextMenu("IncreaseCash")]
    public void TestIncreasecash ()
    {
        IncreaseCash(50);
        SaveCashAmount();
    }

    [ContextMenu("DecreaseCash")]
    public void TestDecreasecash ()
    {
        DecreaseCash(50);
        SaveCashAmount();
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
