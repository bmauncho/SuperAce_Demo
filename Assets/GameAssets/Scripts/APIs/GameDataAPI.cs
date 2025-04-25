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
    public string clientId = "12345";
    public string playerId = "22";
    public string action = "";
}

[System.Serializable]
public class _game
{
    public string id = "32";
    public string name = "SUPER_ACE";
    public string mode = "NORMAL";
}


[System.Serializable]
public class rowData
{
    public List<CardData> infos = new List<CardData>();
}
public class GameDataAPI : MonoBehaviour
{
    UnityWebRequest request;
    [Header("API Settings")]
    //public WinLoseManager winloseManager;
    public int game_id = 32;
    public int clientId = 12345;
    public int PlayerId = 22;

    private const string ApiUrl = "https://proxy.api.ibibe.africa/spin/superace/";

    [Header("API Response")]
    public ApiResponse finalData;
    public float BetAmount;
    public float AmountWon;
    public float FreeSpins;
    [Space(10)]
    public List<rowData> rows = new List<rowData>(5);
    List<CardData> infos = new List<CardData>();
    //public List<rowData> rowsInterchanged = new List<rowData>(5);
    public bool isDataFetched = false;
    public RefillCardsAPI refillCardsAPI;
    public List<bool> canRefill = new List<bool>();
    bool canshowSpins = false;
    private void Start ()
    {
        isDataFetched = false;
        Invoke(nameof(SetUP) , .25f);
    }

    void SetUP ()
    {
        clientId = CommandCentre.Instance.APIManager_.Client_id;
        game_id = CommandCentre.Instance.APIManager_.Game_Id;
        PlayerId = CommandCentre.Instance.APIManager_.Player_Id;
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
        isDataFetched = false;
        Debug.Log("Fetching Card Data!");
        if (CommandCentre.Instance.GridManager_.isRefilling) return; // Prevent API call during refilling
        _GameInfo Data = new _GameInfo();

        if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            Debug.Log("Fetch Free Game!");
            Data = new _GameInfo
            {
                game = new _game
                {
                    id = game_id.ToString() ,
                    name = "SUPER_ACE" ,
                    mode = "FREE"
                } ,
                betAmount = BetAmount ,
                clientId = clientId.ToString() ,
                playerId = PlayerId.ToString(),
                action = "freeSpins" ,
            };
        }
        else
        {
            Debug.Log("Fetch normal Game!");
            
            Data = new _GameInfo
            {
                game = new _game
                {
                    id = game_id.ToString() ,
                    name = "SUPER_ACE" ,
                    mode = "NORMAL"
                } ,
                betAmount = BetAmount ,
                clientId = clientId.ToString() ,
                playerId = PlayerId.ToString() ,
            };
        }
        

        string jsonString = JsonConvert.SerializeObject(Data , Formatting.Indented);
        Debug.Log($"Spin Payload : {jsonString}");
        //CommandCentre.Instance.WinLoseManager_.ResetWinDataList();
        StartCoroutine(_FetchGridInfo(ApiUrl , jsonString));
    }

    IEnumerator _FetchGridInfo ( string url , string bodyJsonString )
    {
        bool isDone = false;
        request = new UnityWebRequest(url , "POST");
        byte [] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type" , "application/json");

        yield return request.SendWebRequest();

        infos.Clear();
        rows.Clear();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Request failed: {request.error}");
            isDone = true;
            isDataFetched = false;
            yield break;
        }

        string output = request.downloadHandler.text;
        if (string.IsNullOrEmpty(output))
        {
            Debug.LogError("Empty response received.");
            isDone = true;
            isDataFetched = false;
            yield break;
        }
        //Debug.Log("Received: " + output);

        var response = JsonConvert.DeserializeObject<ApiResponse>(output);

        object parsedResponse = JsonConvert.DeserializeObject(output);
        string formattedOutput = JsonConvert.SerializeObject(parsedResponse , Formatting.Indented);

        Debug.Log("Received spin cards : " + formattedOutput);

        if (response?.message == "Could not process request at this time")
        {
            Debug.Log(response?.message);
            isDone = true;
            isDataFetched = false;
            yield break;
        }

        if (response?.data?.cards != null)
        {
            AmountWon = response.data.AmountWon;
            FreeSpins = response.data.freeSpins;
            //Debug.Log($"FreeSpins : {FreeSpins}");
            if (FreeSpins > 0)
            {
                Debug.Log("Free Spins: " + FreeSpins);
            }

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
                        logReceivedData(i , j , cardData_);

                        if (cardData_.transformed || ( IsFreeGame() && cardData_.name == "SCATTER" ))
                        {
                            //Debug.Log("Scatter found");
                            CommandCentre.Instance.WinLoseManager_.GetWinningCard(cardData_ , i , j);
                        }

                        if(cardData_.substitute == "BIG_JOKER")
                        {
                            //Debug.Log($"Big Joker SUB found COL: {j} ROW:{i}");
                        }
                    }
                }
            }

            isDone = true;
            isDataFetched = true;
            //Debug.Log("Is Data fetched :" + isDataFetched + "at GameDataApi");
        }
        Debug.Log("Finished");
    }


    public bool IsFreeGame ()
    {
        return FreeSpins >= 10;
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
        //Debug.Log("Rechecking wins...");
        int columnCount = rows [0].infos.Count;

        for (int col = 0 ; col < columnCount ; col++)
        {
            int winCardCount = 0;

            for (int row = 0 ; row < rows.Count ; row++)
            {
                CardData data = rows [row].infos [col];

                if (data.transformed /*|| !string.IsNullOrEmpty(data.substitute)*/)
                {
                    winningCards [data] = (row, col);
                    winCardCount++;
                }
            }

            // Add to refill list if any winning cards exist in this column
            canRefill.Add(winCardCount > 0);
        }

        // Check for valid winning columns: first three must be true and at least three consecutive columns
        int consecutiveCount = 0;
        bool firstThreeValid = canRefill.Count >= 3 && canRefill [0] && canRefill [1] && canRefill [2];

        if (firstThreeValid)
        {
            for (int i = 0 ; i < canRefill.Count ; i++)
            {
                if (canRefill [i])
                {
                    consecutiveCount++;
                }
                else
                {
                    if (consecutiveCount >= 3)
                    {
                        NotifyWinningCards(winningCards);
                        return; // Exit early after notifying
                    }
                    consecutiveCount = 0; // Reset count if a break occurs
                }
            }

            // Final check in case the last columns form a valid group
            if (consecutiveCount >= 3)
            {
                NotifyWinningCards(winningCards);
            }
        }
    }

    private void NotifyWinningCards ( Dictionary<CardData , (int row, int col)> winningCards )
    {
        foreach (var cardEntry in winningCards)
        {
            CardData card = cardEntry.Key;
            (int row, int col) = cardEntry.Value;

            // Notify the WinLoseManager
            CommandCentre.Instance.WinLoseManager_.GetWinningCard(card , row , col);
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

    public UnityWebRequest response ()
    {
        return request;
    }

    public List<Tuple<int , int>> GetBigJokerIndices ()
    {
        List<Tuple<int , int>> BigJokerCards = new List<Tuple<int , int>>();
        for (int i = 0 ; i < rows.Count ; i++)
        {
            for (int j = 0 ; j < rows [i].infos.Count ; j++)
            {
                if (rows [i].infos [j].substitute == "BIG_JOKER")
                {
                    BigJokerCards.Add(new Tuple<int , int>(i , j));
                }
            }
        }
        return BigJokerCards;
    }
}
