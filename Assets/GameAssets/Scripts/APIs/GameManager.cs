using System.Collections;
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
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private const string ServerLink = "https://admin-api.ibibe.africa";
    [SerializeField] private bool isDataFetched = false;
    [SerializeField] private string Player_Id;
    [SerializeField] private string Game_Id;
    [SerializeField] private string Client_id;
    [SerializeField] private string CashAmount = string.Empty;
    [SerializeField] private PlayerInfo playerInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    public void FetchConfigData ()
    {
        Debug.Log("FetchingConfig");
        if (ConfigMan.Instance.ReceivedConfigs)
        {

            if (!string.IsNullOrEmpty(Player_Id))
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

            FetchPlayerInfo();
        }
        else
        {
            FetchPlayerInfo();
        }
    }

    public void FetchPlayerInfo ()
    {
        isDataFetched = false;
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
                Debug.Log("Error: " + www.error);
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
}
