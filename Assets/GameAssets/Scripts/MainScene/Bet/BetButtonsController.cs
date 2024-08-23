using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetButtonsController : MonoBehaviour
{
    public List<float> BetAmounts = new List<float>();
    public List<Bet> BetButtons = new List<Bet>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUp();
    }

    void SetUp ()
    {

        GetAllButtons();

        for(int i = 0; i < BetAmounts.Count; i++)
        {
            BetButtons[i].Amount = BetAmounts[i];
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
