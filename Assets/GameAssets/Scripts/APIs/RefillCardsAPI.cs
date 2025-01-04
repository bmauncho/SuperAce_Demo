using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
[System.Serializable]
public class refillApi
{
    public _game game;
    public float betAmount;
    public string gameMode = "NORMAL";
    public CardData [] [] cards;
}

[System.Serializable]
public class sentData
{
    public List<CardData> data = new List<CardData>();
}

[System.Serializable]
public class receivedData
{
    public List<CardData> data = new List<CardData>();
}

public class RefillCardsAPI : MonoBehaviour
{
    public const string ApiUrl = "https://proxy.api.ibibe.africa/spin/superace?transform=true";
    public refillApi api;
    public GameDataAPI gameDataAPI_;
    public List<sentData> sentData_ = new List<sentData>();
    public List<receivedData> receivedData_ = new List<receivedData>();
    public int maxtries = 3;
    public int tries;
    public bool refillDataFetched=false;
    public bool isError;

    [ContextMenu("Fetch Data")]
    public void FetchData ()
    {
        isError = false;
        refillDataFetched = false;
        sentData_.Clear();
        receivedData_.Clear();
        // Define the jagged array
        CardData [] [] data = new CardData [5] [];  // 5 rows (or columns depending on how you want to structure it)
        for (int j = 0 ; j < 5 ; j++)  // Loop through columns (5)
        {
            data [j] = new CardData [4];  // 4 rows (or items per column)
            for (int i = 0 ; i < 4 ; i++)  // Loop through rows (4)
            {

                Debug.Log(gameDataAPI_.rows.Count);


                //Ensure indices are valid
                if (i < gameDataAPI_.rows.Count && j < gameDataAPI_.rows [i].infos.Count)
                {
                    data [j] [i] = new CardData
                    {
                        name = gameDataAPI_.rows [i].infos [j].name ,
                        golden = gameDataAPI_.rows [i].infos [j].golden ,
                        substitute = gameDataAPI_.rows [i].infos [j].substitute ,
                        transformed = gameDataAPI_.rows [i].infos [j].transformed ,
                    };
                    logSentData(i , j , data [j] [i]);
                }
            }
        }


        api = new refillApi
        {
            betAmount = 2 ,
            cards = data ,
        };
        // Serialize to JSON
        string jsonString = JsonConvert.SerializeObject(api , Formatting.Indented);
        Debug.Log(jsonString);

        StartCoroutine(StartFetchingData(jsonString));
    }

    private IEnumerator StartFetchingData ( string jsonData )
    {
        var request = new UnityWebRequest(ApiUrl , "POST");
        byte [] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("accept" , "application/json");
        request.SetRequestHeader("Content-Type" , "application/json");

        Debug.Log($"Sending data...: {jsonData}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {

            Debug.Log("Data successfully sent!");
            Debug.Log($"Response: {request.downloadHandler.text}");
            string output = request.downloadHandler.text;

            var response = JsonConvert.DeserializeObject<ApiResponse>(output);

            if (response?.data?.cards != null)
            {
                
                tries = 0;
                foreach (var cardRow in response.data.cards)
                {
                    if (cardRow != null)
                    {
                        foreach (var card in cardRow)
                        {
                            if (card != null)
                            {
                                var cardData_ = new CardData
                                {
                                    name = card.name ,
                                    golden = card.golden ,
                                    transformed = card.transformed ,
                                    substitute = card.substitute ,
                                };
                                logReceivedData(Array.IndexOf(cardRow , card) , Array.IndexOf(response.data.cards , cardRow) , cardData_);
                            }
                        }
                    }
                }
                refillDataFetched = true;
            }
            else
            {
                Debug.LogWarning("Response data invalid or cards array is null.");
                HandleRetry("Invalid cards array.");
                isError = true;
            }
        }
        else
        {
            Debug.LogError($"Error sending data: {request.error} | Response Code: {request.responseCode}");
            HandleRetry("Error sending data: " + request.error);
        }

    }

    private void HandleRetry ( string errorMessage )
    {
        if (tries < maxtries)
        {
            tries++;
            Debug.Log($"Retrying... Attempt {tries}/{maxtries}");
            FetchData();
        }
        else
        {
            Debug.LogWarning(errorMessage);
            for (int i = 0 ; i < sentData_.Count ; i++)
            {
                // Initialize a new receivedData object
                var newReceivedData = new receivedData();

                // Add it to the list
                receivedData_.Add(newReceivedData);

                for (int j = 0 ; j < sentData_ [i].data.Count ; j++)
                {
                    // Initialize a new CardData object
                    var newCardData = new CardData();
                      
                    // Set the value of newCardData
                    newCardData = sentData_ [i].data [j];
                    if (!string.IsNullOrEmpty(sentData_ [i].data [j].substitute))
                    {
                        newCardData = new CardData();
                        newCardData.name = newCardData.substitute;
                        newCardData.substitute = null;
                    }
                    // Add the new CardData to the receivedData's data list
                    newReceivedData.data.Add(newCardData);
                }
            }
            refillDataFetched=true;
        }
    }


    void logSentData ( int rows , int cols , CardData cardData )
    {
        // Make sure the list has enough rows
        while (sentData_.Count <= rows)
        {
            sentData_.Add(new sentData());
        }

        // Ensure the row has enough columns
        while (sentData_ [rows].data.Count <= cols)
        {
            sentData_ [rows].data.Add(new CardData());
        }

        // Now safely assign the cardData
        sentData_ [rows].data [cols] = cardData;
    }


    void logReceivedData ( int rows , int cols , CardData cardData )
    {
        // Make sure the list has enough rows
        while (receivedData_.Count <= rows)
        {
            receivedData_.Add(new receivedData());
        }

        // Ensure the row has enough columns
        while (receivedData_ [rows].data.Count <= cols)
        {
            receivedData_ [rows].data.Add(new CardData());
        }

        // Now safely assign the cardData
        receivedData_ [rows].data [cols] = cardData;
    }

    public CardData GetCardInfo ( int col , int row )
    {
        CardData info = null;
        info = receivedData_ [row].data [col];
        return receivedData_.Count > 0 ? info : null ;
    }
}
