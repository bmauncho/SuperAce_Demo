using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class BetButtonsController : MonoBehaviour
{
    public List<float> BetAmounts = new List<float>();
    public List<Bet> BetButtons = new List<Bet>();
    CultureInfo cultureInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUp();
    }

    private void Update ()
    {
        if (LanguageMan.instance)
        {
            TheLanguage lan = LanguageMan.instance.ActiveLanguage;
            cultureInfo = null;
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

            for (int i = 0 ; i < BetAmounts.Count ; i++)
            {
                float amount = BetAmounts [i];
                string formattedAmount;

                if (amount % 1 == 0)
                {
                    // Whole number: no decimal point
                    formattedAmount = amount.ToString("N0" , cultureInfo);
                }
                else
                {
                    // Fractional number: one decimal place
                    formattedAmount = amount.ToString("N1" , cultureInfo);
                }

                BetButtons [i].Amount = formattedAmount;
                BetButtons [i].SetBet();
            }


        }
    }
    void SetUp ()
    {

        GetAllButtons();


        for (int i = 0 ; i < BetAmounts.Count ; i++)
        {
            float amount = BetAmounts [i];
            string formattedAmount;

            if (amount % 1 == 0)
            {
                // Whole number: no decimal point
                formattedAmount = amount.ToString("N0" , cultureInfo);
            }
            else
            {
                // Fractional number: one decimal place
                formattedAmount = amount.ToString("N1" , cultureInfo);
            }

            BetButtons [i].Amount = formattedAmount;
            BetButtons [i].SetBet();
        }


    }

    void GetAllButtons ()
    {
        foreach(Transform tr in transform)
        {
            if (tr.GetComponent<Bet>())
            {
                BetButtons.Add(tr.GetComponent<Bet>());
            }
        }

    }
}
