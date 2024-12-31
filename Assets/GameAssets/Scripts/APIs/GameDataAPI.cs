using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class _GameInfo
{
    public _game game;
    public float betAmount = 1000;
    public string gameMode = "Normal";
}

[System.Serializable]
public class _game
{
    public int id = 2;
    public string name = "Super Ace";
}

[System.Serializable]
public class rowData
{
    public List<CardData> infos = new List<CardData>();
}
public class GameDataAPI : MonoBehaviour
{
    public WinLoseManager winloseManager;
    private const string ApiUrl = "https://proxy.api.ibibe.africa/spin/superace";
    public ApiResponse finalData;
    public float BetAmount;
    public float AmountWon;
    [Space(10)]
    public List<rowData> rows = new List<rowData>(5);
    List<CardData> infos = new List<CardData>();
    public bool isDataFetched = false;
    public RefillCardsAPI refillCardsAPI;
    private void Start ()
    {
        isDataFetched = false;
    }

    private void Update ()
    {
        if (CommandCentre.Instance)
        {
            BetAmount = CommandCentre.Instance.BetManager_.BetAmount;
        }
    }

    [ContextMenu("FetchInfo")]
    public void FetchInfo ()
    {

        _GameInfo Data = new _GameInfo();
        Data.betAmount = BetAmount;
        string jsonString = JsonConvert.SerializeObject(Data , Formatting.Indented);
        Debug.Log(jsonString);
        //CommandCentre.Instance.WinLoseManager_.ResetWinDataList();
        StartCoroutine(_FetchGridInfo(ApiUrl , jsonString));
    }

    IEnumerator _FetchGridInfo ( string url , string bodyJsonString , Action OnComplete = null )
    {
        var request = new UnityWebRequest(url , "POST");
        byte [] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type" , "application/json");
        //Debug.Log("Sending data...");
        yield return request.SendWebRequest();
        infos.Clear();
        rows.Clear();
        //Debug.Log("Status Code: " + request.responseCode);
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);
            string output = request.downloadHandler.text;

            var response = JsonConvert.DeserializeObject<ApiResponse>(output);
            if (response?.data?.cards != null)
            {
                // Debug the deserialized data
                //Debug.Log("Status: " + response.status);
                //Debug.Log("Free Spins: " + response.data.freeSpins);
                //Debug.Log("Amount Won: " + response.data.AmountWon);
                
                for (int i = 0 ; i < response.data.cards.Length ; i++)
                {
                    var cardRow = response.data.cards [i];
                    if (cardRow != null)
                    {
                        for (int j = 0 ; j < cardRow.Length ; j++)
                        {
                            var card = cardRow [j];

                            CardData cardData_ = new CardData
                            {
                                name = card.name ,
                                golden = card.golden ,
                                substitute = card.substitute ,
                                transformed = card.transformed ,
                            };
                            logReceivedData(i,j,cardData_);

                            if (cardData_.transformed)
                            {
                                winloseManager.GetWinningCard(cardData_ , i , j);
                            }
                        }
                    }
                    AmountWon = response.data.AmountWon;
                }
            }
            isDataFetched = true;
            refillCardsAPI.FetchData();
        }
    }

    public bool IsFreeGame ()
    {
        return finalData.data.freeSpins >= 10;
    }

    public CardData GetCardInfo ( int col , int row )
    {
        CardData info = null;
        info = rows [row].infos [col];
        return info;
    }

    void logReceivedData ( int rows_ , int cols_ , CardData cardData )
    {
        // Make sure the list has enough rows
        while (rows.Count <= rows_)
        {
            rows.Add(new rowData());
        }

        // Ensure the row has enough columns
        while (rows [rows_].infos.Count <= cols_)
        {
            rows [rows_].infos.Add(new CardData());
        }

        // Now safely assign the cardData
        rows [rows_].infos [cols_] = cardData;
    }

    public void recheckWin ()
    {
        Debug.Log("recheck");
        for(int i = 0;i<rows.Count ; i++)
        {
            for(int j = 0 ; j < rows [i].infos.Count ; j++)
            {
                CardData data = rows [i].infos [j];
                if (data.transformed)
                {
                    winloseManager.GetWinningCard(data , i , j);
                }
            }
        }
    }
}
