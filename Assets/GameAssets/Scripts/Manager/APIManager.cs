using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;



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

    public string Player_Id;
    public string Game_Id;
    public string Client_id ;
    public string bet_id;
    public float Amountwon;

    public string CashAmount = string.Empty;
    public TMP_Text TranscationalText;
    public GameObject ServerError;
    private void Start ()
    {
        Invoke(nameof(SetUP) , .2f);
    }

    void SetUP ()
    {
        Client_id = GameManager.Instance.GetClientId();
        Game_Id = GameManager.Instance.GetGameId();
        Player_Id = GameManager.Instance.GetPlayerId();
        CashAmount = GameManager.Instance.GetCashAmount();
        CommandCentre.Instance.CashManager_.UpdateCashAmount(float.Parse(CashAmount));
        GameManager.Instance.AddTransactionText(TranscationalText);
    }

    public void PlaceBet ()
    {
        betPlacingAPI_.Bet ();
    }

    public void UpdateBet ()
    {
        string betid = bet_id;
        string AmountWon = string.Empty;

        if (CommandCentre.Instance.GridManager_.isRefillingSequence())
        {
            float winnings = CommandCentre.Instance.APIManager_.refillCardsAPI_.response.data.AmountWon;
            AmountWon = winnings.ToString();
        }
        else
        {
            float winnings2 = CommandCentre.Instance.APIManager_.GameDataAPI_.AmountWon;
            AmountWon = winnings2.ToString();
        }
       
        //Debug.Log("Amount won = " + AmountWon);
        Amountwon = float.Parse(AmountWon);
        string clientid = CommandCentre.Instance.APIManager_.betPlacingAPI_.client_id.ToString();
        betUpdaterAPI_.UpdateBet (betid,AmountWon,clientid);
    }

    public void UpdateBetAfterFreeGame (string AmoutWon)
    {
        string betid = bet_id;
        string AmountWon = AmoutWon;
        string clientid = CommandCentre.Instance.APIManager_.betPlacingAPI_.client_id.ToString();
        betUpdaterAPI_.UpdateBet(betid , AmountWon , clientid);
    }

    public void ShowWaring ()
    {
        StartCoroutine(serverError());
    }

    private IEnumerator serverError ()
    {
        ServerError.SetActive(true);
        yield return new WaitForSeconds(2f);
        ServerError.SetActive(false);
        yield return null;
    }
}


