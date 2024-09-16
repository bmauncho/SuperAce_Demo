using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class CashManager : MonoBehaviour
{
    public TextMeshProUGUI CashAmountText;
    public TextMeshProUGUI WinCashAmountText;
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
        CashAmountText.SetText(Mathf.FloorToInt(CashAmount).ToString());
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
        WinCashAmountText.text = CurrentWinings.ToString();
    }

    public void ResetWinings ()
    {
        CurrentWinings = 0;
        WinCashAmountText.text = CurrentWinings.ToString();
    }
}
