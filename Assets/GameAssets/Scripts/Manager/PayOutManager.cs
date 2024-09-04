using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        CurrentWinAmount.text = $"{CurrentWin.ToString("F2")}";
    }

    public void ShowCurrentWin ()
    {
        CurrentWin = TotalWinnings(GetCardPayOut(winLoseManager.GetWinningCardType(),winLoseManager.GetNumberOfColumnsWithWinningCards()) 
            , winLoseManager.GetPayLines(),betManager.BetAmount,comboManager.GetCombo());
        WinUI_.ActivateCurrentWinings();
        CommandCentre.Instance.CashManager_.IncreaseWinings(CurrentWin);
    }

    public void HideCurrentWin ()
    {
        WinUI_.DeactivateCurrentWinings();
    }

    public List<float> GetCardPayOut ( List<string> card , int column )
    {
        List<float> result = new List<float>();
        for( int i = 0;i<card.Count ; i++)
        {
            if (cardToIndex.TryGetValue(card [i] , out int index))
            {
                if (column >= 3 && column <= 5)
                {
                    result.Add(PayList [index].Payouts [column - 3]);
                    Debug.Log(card [i] + " : " + string.Join(", " , result));
                }
            }
        }
     

        // If no payout is found, return an empty list
        return result;
    }


    public float TotalWinnings ( List<float> Payout , int PayLines , float Bet , int Combo )
    {
        float Total = 0;
        for (int i = 0 ; i < Payout.Count ; i++)
        {
            Total += Payout [i] * PayLines * Bet * Combo;
        }
        return Total;
    }

}
