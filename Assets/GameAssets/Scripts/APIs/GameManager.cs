using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
[System.Serializable]
public class PlayerInfo
{
    public string id;
    public string names;
    public string msisdn;
    public string account_number;
    public string email_address;
    public string is_blacklisted;
    public string wallet_balance;
    public string created_at;
    public string last_bet_date;
    public string last_win_date;
}

[System.Serializable]
public class AddCashData
{
    public int customer_id = 12;
    public string payment_method = "visa";
    public string transaction_id = "HGFBTNNRKgagagaT";
    public float amount = 20;
}

[System.Serializable]
public class MakeWithdrawalData
{
    public int customer_id = 12;
    public float amount = 20;
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string ServerLink = "https://admin-api.ibibe.africa";
    [SerializeField] private bool IsDemoMode;
    [SerializeField] private bool isDataFetched = false;
    [SerializeField] private string Player_Id;
    [SerializeField] private string Game_Id;
    [SerializeField] private string Client_id;
    [SerializeField] private string CashAmount = string.Empty;
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private TMP_Text [] TransactionsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }
    #region[api setup]
    public void FetchConfigData ()
    {
        Debug.Log("FetchingConfig");
        if (ConfigMan.Instance)
        {
            IsDemoMode = ConfigMan.Instance.IsDemo;
            if (ConfigMan.Instance.ReceivedConfigs)
            {

                if (!string.IsNullOrEmpty(ConfigMan.Instance.PlayerId))
                {
                    Player_Id = ConfigMan.Instance.PlayerId;
                }

                if (!string.IsNullOrEmpty(ConfigMan.Instance.GameId))
                {
                    Game_Id = ConfigMan.Instance.GameId;
                }

                if (!string.IsNullOrEmpty(ConfigMan.Instance.ClientId))
                {
                    Client_id = ConfigMan.Instance.ClientId;
                }

                if (ConfigMan.Instance.IsDemo)
                {
                    CashAmount = "2000";
                }

                manualStart();
            }
            else
            {
                manualStart();
            }
        }
    }

    public void FetchPlayerInfo ()
    {
        isDataFetched = false;
        ServerLink = ConfigMan.Instance.Base_url;
        StartCoroutine(_FetchPlayerInfo(ServerLink + "/api/v1/customer/details?customer_id=" + Player_Id));
    }

    IEnumerator _FetchPlayerInfo ( string url )
    {
        using (UnityWebRequest www = UnityWebRequestHelper.GetWithTimestamp(url))
        {
            www.useHttpContinue = false;
            www.SetRequestHeader("Cache-Control" , "no-cache, no-store, must-revalidate");
            www.SetRequestHeader("Pragma" , "no-cache");
            www.SetRequestHeader("Expires" , "0");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Received: " + www.downloadHandler.text);
                playerInfo = JsonUtility.FromJson<PlayerInfo>(www.downloadHandler.text);
                CashAmount = playerInfo.wallet_balance;
                isDataFetched = true;
            }
            else
            {
                isDataFetched = true;
                Debug.Log("Error: " + www.result);
                CashAmount = "2000";
            }
        }
    }

    public bool IsDataFetched ()
    {
        return isDataFetched;
    }

    public string GetPlayerId ()
    {
        return Player_Id;
    }

    public string GetGameId ()
    {
        return Game_Id;
    }

    public string GetClientId ()
    {
        return Client_id;
    }
    public string GetCashAmount ()
    {
        return CashAmount;
    }
    #endregion  

    public void manualStart ()
    {
        for (int i = 0 ; i < TransactionsText.Length ; i++)
        {
            TransactionsText [i].text = "";
        }

        if (IsDemoMode)
        {
            playerInfo.names = "Demo";
            playerInfo.wallet_balance = 2000.ToString();
        }
        FetchPlayerInfo();
    }

    public void AddTransactionText ( TMP_Text theText )
    {
        TransactionsText = new TMP_Text [] { theText };
    }
    string transaction = string.Empty;

    private void Update ()
    {
        if (LanguageMan.instance)
        {
            transaction = LanguageMan.instance.RequestForText("L_124");
        }
    }
    public void ShowTransaction ( string thetrans )
    {
        thetrans = transaction + " 15614 - 040024 -" + thetrans;
        for (int i = 0 ; i < TransactionsText.Length ; i++)
        {
            TransactionsText [i].text = thetrans;
        }
    }


    [ContextMenu("ListCustomers")]
    public void ListCustomers ()
    {
        StartCoroutine(_FetchCustomers(ServerLink + "/api/v1/customer/details"));
    }

    IEnumerator _FetchCustomers ( string url )
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.useHttpContinue = false;
            www.SetRequestHeader("Cache-Control" , "no-cache, no-store, must-revalidate");
            www.SetRequestHeader("Pragma" , "no-cache");
            www.SetRequestHeader("Expires" , "0");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Received: " + www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Error: " + www.error);
            }
        } // The using block ensures www.Dispo
    }
    public void MakeWithdrawal ( float _Amount )
    {
        MakeWithdrawalData Data = new MakeWithdrawalData();
        Data.customer_id = int.Parse(Player_Id);
        Data.amount = _Amount;
        string jsonString = JsonUtility.ToJson(Data);
        string TheUrl = ServerLink + "/api/withdraw/money";
        StartCoroutine(_MakeWithdrawal(ServerLink + "/api/v1/withdraw/money" , jsonString));
    }
    IEnumerator _MakeWithdrawal ( string url , string bodyJsonString )
    {
        var request = new UnityWebRequest(url , "POST");
        byte [] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type" , "application/json");
        request.SetRequestHeader("Cache-Control" , "no-cache, no-store, must-revalidate");
        request.SetRequestHeader("Pragma" , "no-cache");
        request.SetRequestHeader("Expires" , "0");
        yield return request.SendWebRequest();
        //Debug.Log("Status Code: " + request.responseCode);
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);

            FetchPlayerInfo();

        }
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }
    public void AddCashAmount ( float _Amount )
    {
        AddCashData Data = new AddCashData();
        Data.transaction_id = UnityEngine.Random.Range(100 , 10000000).ToString() + "_" + UnityEngine.Random.Range(100 , 10000000).ToString();
        Data.customer_id = int.Parse(Player_Id);
        Data.amount = _Amount;
        string jsonString = JsonUtility.ToJson(Data);
        StartCoroutine(_AddCashAmount(ServerLink + "/api/v1/add_game_payment" , jsonString));
    }
    IEnumerator _AddCashAmount ( string url , string bodyJsonString )
    {
        var request = new UnityWebRequest(url , "POST");
        byte [] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type" , "application/json");
        request.SetRequestHeader("Cache-Control" , "no-cache, no-store, must-revalidate");
        request.SetRequestHeader("Pragma" , "no-cache");
        request.SetRequestHeader("Expires" , "0");
        yield return request.SendWebRequest();
        //Debug.Log("Status Code: " + request.responseCode);
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);

            FetchPlayerInfo();

        }
        else
        {
            Debug.Log("Error: " + request.error);
            //show ServerError
        }
    }
    public bool IsDemo ()
    {
        return IsDemoMode;
    }
    public PlayerInfo GetPlayerInfo ()
    {
        return playerInfo;
    }
    public void SetPlayerInfo ( PlayerInfo info )
    {
        playerInfo = info;
        CashAmount = info.wallet_balance;
        FetchPlayerInfo();
    }
}
