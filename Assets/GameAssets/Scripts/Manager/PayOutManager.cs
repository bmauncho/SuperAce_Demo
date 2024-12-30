using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class PayOutManager : MonoBehaviour
{
    WinLoseManager winLoseManager;
    BetManager betManager;
    ComboManager comboManager;
    public float CurrentWin;
    public TMP_Text CurrentWinAmount;
    public WinUI WinUI_;

    private void Start ()
    {
        winLoseManager = CommandCentre.Instance.WinLoseManager_;
        betManager = CommandCentre.Instance.BetManager_;
        comboManager = CommandCentre.Instance.ComboManager_;
    }

    [ContextMenu("Get PayOutAmount")]
    public void Test ()
    {
       // GetCardPayOut("Jack" , 4);
    }
    private void Update ()
    {
        if (CurrentWin % 1 == 0)
        {
            CurrentWinAmount.text = CurrentWin.ToString();
        }
        else
        {
            CurrentWinAmount.text = $"{CurrentWin.ToString("F2")}";
        }

        if (CommandCentre.Instance)
        {
            //CurrentWin = CommandCentre.Instance.APIManager_.GameDataAPI_.finalData.AmountWon;
            CurrentWin = CommandCentre.Instance.APIManager_.GameDataAPI_.AmountWon;
        }
        
        if (CurrentWin >= 10000000)
        {
            CurrentWin = 10000000;
        }
    }

    public void ShowCurrentWin ()
    {
        StartCoroutine(showinnings());
    }

    IEnumerator showinnings ()
    {
        WinUI_.ActivateCurrentWinings();
        CommandCentre.Instance.CashManager_.IncreaseWinings(CurrentWin);
        yield return new WaitForSeconds(1f);
        HideCurrentWin();
    }

    public void HideCurrentWin ()
    {
        WinUI_.DeactivateCurrentWinings();
    }


    public void ShowTotalWinings ()
    {
        WinUI_.ActivateTotalWinnings();
        CommandCentre.Instance.SoundManager_.PlaySound("Winmusic" , false , .3f);
    }

    public void HideTotalWinnings ()
    {
        WinUI_.DeactivateTotalWinnings();
    }
}

