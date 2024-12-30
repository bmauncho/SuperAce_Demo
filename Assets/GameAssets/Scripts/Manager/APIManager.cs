using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public string name;
    public string substitute;
    public bool golden;
    public bool transformed;
}

[System.Serializable]
public class ApiResponse
{
    public bool status { get; set; }
    public string message { get; set; }
    public Data data { get; set; }
}

[System.Serializable]
public class Data
{
    public int freeSpins { get; set; }
    public float AmountWon { get; set; }
    public CardData [] [] cards { get; set; }
}

public class APIManager : MonoBehaviour
{
    public GameDataAPI GameDataAPI_;
    public BetPlacingAPI betPlacingAPI_;
    public BetUpdaterAPI betUpdaterAPI_;
    public RefillCardsAPI refillCardsAPI_;

    public void FetchGameData ()
    {
        GameDataAPI_.FetchInfo ();
    }

    public void PlaceBet ()
    {
        betPlacingAPI_.Bet ();
    }

    public void UpdateBet ()
    {
        betUpdaterAPI_.UpdateBet ();
    }
}


