using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Net.WebRequestMethods;

[System.Serializable]
public class Pay
{
    public string SymbolName;
    public List<float> Payouts = new List<float>();
}
public class PayOutManager : MonoBehaviour
{
    WinLoseManager winLoseManager;
    BetManager betManager;
    ComboManager comboManager;
    public List<Pay> PayList = new List<Pay>();
    public float CurrentWin;
    public TMP_Text CurrentWinAmount;
    public WinUI WinUI_;
    private Dictionary<string , int> cardToIndex = new Dictionary<string , int>()
    {
        {"Ace", 0},
        {"King", 1},
        {"Queen", 2},
        {"Jack", 3},
        {"Hearts", 4},
        {"Spades", 5},
        {"Diamonds", 6},
        {"Clubs", 7}
    };

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

        if (CurrentWin >= 10000000)
        {
            CurrentWin = 10000000;
        }
    }

    public void ShowCurrentWin ()
    {
        Debug.Log($"the combo is X{comboManager.GetCombo()} the real combo is{comboManager.ComboCounter}");
        CurrentWin = TotalWinnings(GetCardPayOut(winLoseManager.GetWinningCardType(),winLoseManager.GetNumberOfWinningCards()) 
            , winLoseManager.GetPayLines(),betManager.GetBetAmount() ,comboManager.GetCombo());
        WinUI_.ActivateCurrentWinings();
        CommandCentre.Instance.CashManager_.IncreaseWinings(CurrentWin);
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
    public List<float> GetCardPayOut ( List<string> card , List<int> No_ofCards )
    {
        List<float> result = new List<float>();
        for (int i = 0 ; i < card.Count ; i++)
        {
            if (cardToIndex.TryGetValue(card [i] , out int index))
            {
                int payoutIndex = No_ofCards [i] - 3;
                if (payoutIndex >= 0 && payoutIndex < PayList [index].Payouts.Count)
                {
                    result.Add(PayList [index].Payouts [payoutIndex]);
                    //Debug.Log(card [i] + " : " + string.Join(", " , result));
                }
                else if(payoutIndex >= 0 && payoutIndex >= PayList [index].Payouts.Count)
                {
                    result.Add(PayList [index].Payouts [PayList [index].Payouts.Count-1]);
                   // Debug.Log(string.Join(", " , result));
                    Debug.LogWarning($"Invalid payout index for card {card [i]} with {No_ofCards [i]} cards.");
                }
            }
        }

        // If no payout is found, return an empty list
        return result;
    }



    public float TotalWinnings ( List<float> Payout , int PayLines , float Bet , int Combo )
    {
        float Total = 0;

        Debug.Log(Bet);
        for (int i = 0 ; i < Payout.Count ; i++)
        {
            Total += Payout [i] * PayLines * Bet * Combo;
        }
        return Total;
    }

}

