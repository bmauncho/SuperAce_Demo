using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    //public List<rowData> rowsInterchanged = new List<rowData>(5);
    public bool isDataFetched = false;
    public RefillCardsAPI refillCardsAPI;
    public List<bool> canRefill = new List<bool>();

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
        Data.game = new _game();
        Data.betAmount = BetAmount;
        string jsonString = JsonConvert.SerializeObject(Data , Formatting.Indented);
        //Debug.Log(jsonString);
        //CommandCentre.Instance.WinLoseManager_.ResetWinDataList();
        StartCoroutine(_FetchGridInfo(ApiUrl , jsonString));
    }

    IEnumerator _FetchGridInfo ( string url , string bodyJsonString , Action OnComplete = null )
    {
        bool isDone = false;
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
            //Debug.Log("Received: " + request.downloadHandler.text);
            string output = request.downloadHandler.text;
            object parsedResponse = JsonConvert.DeserializeObject(output);
            string formattedOutput = JsonConvert.SerializeObject(parsedResponse , Formatting.Indented);

            Debug.Log("Received: " + formattedOutput);
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
                    isDone = true;
                    isDataFetched = true;
                }
            }
            
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

    public void RecheckWin ()
    {
        canRefill.Clear();
        Dictionary<CardData , (int row, int col)> winningCards = new Dictionary<CardData , (int row, int col)>();
        Debug.Log("Rechecking wins...");
        int columnCount = rows [0].infos.Count;

        for (int col = 0 ; col < columnCount ; col++)
        {
            int winCardCount = 0;

            for (int row = 0 ; row < rows.Count ; row++)
            {
                CardData data = rows [row].infos [col];

                if (data.transformed || !string.IsNullOrEmpty(data.substitute))
                {
                    winningCards [data] = (row, col);
                    winCardCount++;
                }
            }

            // Add to refill list if any winning cards exist in this column
            canRefill.Add(winCardCount > 0);
        }

        // Check for 3 or more consecutive rows with winning cards
        for (int i = 0 ; i < canRefill.Count - 2 ; i++)
        {
            if (canRefill [i] && canRefill [i + 1] && canRefill [i + 2])
            {
                foreach (var cardEntry in winningCards)
                {
                    CardData card = cardEntry.Key;
                    (int row, int col) = cardEntry.Value;

                    // Notify the WinLoseManager
                    winloseManager.GetWinningCard(card , row , col);
                }
            }
        }
    }


    public List<rowData> ReverseRows ( List<rowData> originalRows )
    {
        // Check if the list is null or empty
        if (originalRows == null || originalRows.Count == 0)
        {
            Debug.LogError("The rows list is null or empty.");
            return null;
        }

        // Create a deep copy of the original list
        List<rowData> modifiedRows = new List<rowData>(originalRows);

        // Reverse the list
        modifiedRows.Reverse();

        return modifiedRows;
    }


}
