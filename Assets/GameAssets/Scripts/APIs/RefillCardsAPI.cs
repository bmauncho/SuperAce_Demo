using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
[System.Serializable]
public class APIInfo
{
    public game game;
    public float betAmount;
    public string gameMode = "NORMAL";
    public refillData[] refillRows_;
}

[System.Serializable]
public class APIResponse
{
    public refillData [] refillRows_;
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
    public refillCardInfo[] cardInfo;
}

[System.Serializable]
public class refillCardInfo
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

    [ContextMenu("SetUp Data")]
    void SetUp ()
    {
        for (int i = 0 ; i < gameDataAPI_.rows.Count ; i++)
        {
            // Ensure that refillRows_ array is initialized only once before the loop
            if (apiInfo_.refillRows_ == null || apiInfo_.refillRows_.Length != gameDataAPI_.rows.Count)
            {
                apiInfo_.refillRows_ = new refillData [gameDataAPI_.rows.Count];
            }

            // Initialize refillData elements for each row
            if (apiInfo_.refillRows_ [i] == null)
            {
                apiInfo_.refillRows_ [i] = new refillData();  // Initialize the refillData instance
            }

            List<CardInfo> infos_ = new List<CardInfo>(gameDataAPI_.rows [i].infos);

            // Initialize cardInfo array for the current refillData element
            apiInfo_.refillRows_ [i].cardInfo = new refillCardInfo [infos_.Count];

            // Populate cardInfo array
            for (int j = 0 ; j < infos_.Count ; j++)
            {
                // Add logic to populate the cardInfo array, if necessary
                apiInfo_.refillRows_ [i].cardInfo [j] = new refillCardInfo();  // Initialize each refillCardInfo
                apiInfo_.refillRows_ [i].cardInfo [j].name = infos_ [j].name;

            }
        }

    }

    [ContextMenu("fetch Data")]
    public void FetchInfo ()
    {

        SetUp ();

        APIInfo apiInfo = new APIInfo
        {
            betAmount = betAmount_ ,
            refillRows_ = apiInfo_.refillRows_ ,
        };


        string jsonString = JsonUtility.ToJson(apiInfo);

        StartCoroutine(startFetchingData(jsonString));
    }


    public IEnumerator startFetchingData ( string bodyJsonString )
    {
        UnityWebRequest request = new UnityWebRequest(ApiUrl , "POST");
        byte [] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type" , "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request Successful");
            Debug.Log("Response: " + request.downloadHandler.text);
            string _Data = JsonHelper.GetJsonObject(request.downloadHandler.text , "data");
            Debug.Log(_Data);
            string [] newdata = _Data.Split('[');
            bool Firsskip = false;
            
            for (int i = 0 ; i < newdata.Length ; i++)
            {
                if (Firsskip)
                {
                    string [] _newdata = newdata [i].Split(']');
                    string [] Newdata1 = _newdata [0].Split('}');

                    Debug.Log(i);
                    for (int y = 0 ; y < Newdata1.Length ; y++)
                    {
                        
                        //Debug.Log(Newdata1 [y]);
                        string [] _newdata2 = Newdata1 [y].Split('{');
                        for (int r = 0 ; r < _newdata2.Length ; r++)
                        {
                            char [] lenght = _newdata2 [r].ToCharArray();
                            if (!string.IsNullOrEmpty(_newdata2 [r]) &&
                               lenght.Length > 3)
                            {
                                Debug.Log(_newdata2 [r]);
                                refillCardInfo refill_card = new refillCardInfo();
                                refill_card = JsonUtility.FromJson<refillCardInfo>("{" + _newdata2 [r] + "}");
                            }

                        }
                    }
                }
                Firsskip = true;
            }

        }
        else
        {
            Debug.LogWarning($"Request failed: {request.error}");
            Debug.Log($"Response Code: {request.responseCode}");
            Debug.Log($"Response: {request.downloadHandler.text}");
        }
    }


}
