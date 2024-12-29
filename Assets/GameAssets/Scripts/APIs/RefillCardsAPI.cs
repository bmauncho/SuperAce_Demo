using Newtonsoft.Json;
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
    public const string ApiUrl = "https://api.ibibe.africa:8500/spin/superace?transform=true";
    public refillApi api;
    public GameDataAPI gameDataAPI_;
    public List<sentData> sentData_ = new List<sentData>();
    public List<receivedData> receivedData_ = new List<receivedData>();

    [ContextMenu("Fetch Data")]
    public void FetchData ()
    {
        sentData_.Clear();
        receivedData_.Clear();
        // Define the jagged array
        // Define the jagged array
        CardData [] [] data = new CardData [5] [];  // 5 rows (or columns depending on how you want to structure it)
        for (int j = 0 ; j < 5 ; j++)  // Loop through columns (5)
        {
            data [j] = new CardData [4];  // 4 rows (or items per column)
            for (int i = 0 ; i < 4 ; i++)  // Loop through rows (4)
            {
                data [j] [i] = new CardData
                {
                    golden = gameDataAPI_.rows [i].infos [j].golden ,  // Access data by row then column
                    transformed = false ,
                    name = gameDataAPI_.rows [i].infos [j].name ,
                };
                logSentData(i , j , data [j] [i]);
            }
        }


        api = new refillApi
        {
            betAmount = 2 ,
            cards = data ,
        };


        // Serialize to JSON
        string jsonString = JsonConvert.SerializeObject(api , Formatting.Indented);
        //Debug.Log(jsonString);

        StartCoroutine(StartFetchingData(jsonString));
    }

    private IEnumerator StartFetchingData ( string jsonData )
    {
        var request = new UnityWebRequest(ApiUrl , "POST");
        byte [] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type" , "application/json");

        Debug.Log("Sending data...");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data successfully sent!");
            Debug.Log("Response: " + request.downloadHandler.text);
            string output = request.downloadHandler.text;

            var response = JsonConvert.DeserializeObject<ApiResponse>(output);

            if (response?.data?.cards != null)
            {
                // Debug the deserialized data
                //Debug.Log("Status: " + response.status);
                //Debug.Log("Free Spins: " + response.data.freeSpins);
                //Debug.Log("Amount Won: " + response.data.AmountWon);


                // Iterate through cards and log their names
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
                                transformed = card.transformed ,
                            };
                            //Debug.Log(cardData_.name);
                            logReceivedData(i , j , cardData_);
                        }
                    }
                }

            }
            else
            {
                Debug.LogError("Cards array is null.");
            }
        }
        else
        {
            Debug.LogError($"Error sending data: {request.error}");
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
}
