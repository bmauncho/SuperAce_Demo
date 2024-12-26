using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
[System.Serializable]
public class APIInfo
{
    public game game;
    public float betAmount;
    public string gameMode = "NORMAL";
    public refillData[][] cards;
}

[System.Serializable]
public class APIResponse
{
    public refillData [] [] cards;
}


[System.Serializable]
public class game
{
    public int id = 2;
    public string name = "Super Ace";
}

[System.Serializable]
public class refillData
{
    public string name;
    public bool transform;
}

public class RefillCardsAPI : MonoBehaviour
{
    public const string ApiUrl = "https://slots.ibibe.africa/spin/superace?transform=true";
    public float betAmount_;
    public APIInfo apiInfo_;
    public GameDataAPI gameDataAPI_;
    public APIResponse apiResponse_;
    [SerializeField]List<APIResponse> refillCards_ = new List<APIResponse>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDataAPI_)
        {
            betAmount_ = gameDataAPI_.BetAmount;
        }
    }

    [ContextMenu("fetch Data")]
    public void FetchInfo ()
    {
        refillCards_.Clear();
        refillData [] [] data = new refillData [4] [];
        for (int i = 0 ; i < data.Length ; i++)
        {
            data [i] = new refillData [5];
            for (int j = 0 ; j < data [i].Length ; j++)
            {
                data [i] [j] = new refillData
                {
                    name = gameDataAPI_.rows [i].infos [j].name ,
                    transform = false
                };
            }
        }

        apiInfo_ = new APIInfo
        {
            betAmount = betAmount_ ,
            cards = data ,
        };

        string jsonString = JsonConvert.SerializeObject(apiInfo_ , Formatting.Indented);
        Debug.Log(jsonString);
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
            var response = JsonConvert.DeserializeObject<APIResponse>(request.downloadHandler.text);
            Debug.Log(response);
        }
        else
        {
            Debug.LogError($"Error sending data: {request.error}");
        }
    }

    //void GetResponse ( List<refillData> cardInfo_ )
    //{
    //    const int rows = 4;
    //    const int columns = 5;

    //    apiResponse_.refillRows_ = new refillData [rows];
    //    int index = 0;
    //    for (int i = 0 ; i < rows ; i++)
    //    {
    //        apiResponse_.refillRows_ [i] = new refillData
    //        {
    //            cardInfo = new refillCardInfo [columns]
    //        };

    //        for (int j = 0 ; j < columns ; j++)
    //        {
    //            apiResponse_.refillRows_ [i].cardInfo [j] = new refillCardInfo
    //            {
    //                 Copy the properties from cardInfo_ to create a unique instance.
    //                name = cardInfo_ [index].name ,
    //                transform = cardInfo_[index].transform
    //            };
    //            index++;
    //        }
    //    }
    //}



}
