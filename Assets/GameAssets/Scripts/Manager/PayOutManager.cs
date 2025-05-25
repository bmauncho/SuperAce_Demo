using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
            if (LanguageMan.instance) 
            {
                TheLanguage lan = LanguageMan.instance.ActiveLanguage;
                CultureInfo cultureInfo = null;
                switch (lan)
                {
                    case TheLanguage.English:
                        cultureInfo = new CultureInfo("en-US");
                        break;
                    case TheLanguage.Chinese:
                        cultureInfo = new CultureInfo("zh-CN"); // Simplified Chinese
                        break;
                    case TheLanguage.Portoguese:
                        cultureInfo = new CultureInfo("pt-BR");
                        break;
                    default:
                        break;
                }
                CurrentWinAmount.text = $"{CurrentWin.ToString("N2" , cultureInfo)}";
            }
            else
            {
                CurrentWinAmount.text = $"{CurrentWin.ToString("N2")}";
            }
            
        }

        if (CommandCentre.Instance)
        {
            if (CommandCentre.Instance.DemoManager_.IsDemo)
            {
                if (CommandCentre.Instance.DemoManager_.winIndex < CommandCentre.Instance.DemoManager_.winAmount.Length)
                {
                    CurrentWin = float.Parse(CommandCentre.Instance.DemoManager_.winAmount [CommandCentre.Instance.DemoManager_.winIndex]);
                }
            }
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
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {

        }
        else
        {
            CurrentWin = CommandCentre.Instance.APIManager_.Amountwon;
        }

        if (CurrentWin <= 0)
        {
            yield break;
        }
        WinUI_.ActivateCurrentWinings();
        Debug.Log("winnings ;" + CurrentWin);
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
        CommandCentre.Instance.SoundManager_.PlaySound("Winmusic" , false);
    }

    public void HideTotalWinnings ()
    {
        WinUI_.DeactivateTotalWinnings();
    }

    public void resetCurrentWinings ()
    {
        CurrentWin = 0;
        WinUI_.CurrentWinnings.SetActive(false);
    }
}

