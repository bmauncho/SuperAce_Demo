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
public class AddCashResponse
{

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
    private const string ServerLink = "https://admin-api.ibibe.africa";

    public string Player_Id = "22";
    public string Game_Id = "1234";
    public string Client_id = "12345";

    public string CashAmount = string.Empty;

    public PlayerInfo playerInfo;
    //public TMP_Text [] TransactionsText;
    public bool isDataFetched = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake ()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public void manualStart ()
    {
        FetchPlayerInfo();
    }

    public void SetUpCashAmount ()
    {
        if (ConfigMan.Instance.IsDemo)
        {
            CashAmount = "2000";
        }
        else
        {
            CashAmount = playerInfo.wallet_balance;
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
                isDataFetched = true;
                // CommandCentre.Instance.CashManager_.UpdateCashAmount(float.Parse(playerInfo.wallet_balance));
            }
            else
            {
                isDataFetched = true;
                Debug.Log("Error: " + www.error);
            }
        } // The using block ensures www.Dispo
    }
    public void fetchConfigData ()
    {
        Debug.Log("FetchingConfig");
        if (ConfigMan.Instance.ReceivedConfigs)
        {

            if (!string.IsNullOrEmpty(Player_Id))
            {
                Player_Id = ConfigMan.Instance.PlayerId.ToString();
            }

            if (!string.IsNullOrEmpty(ConfigMan.Instance.GameId))
            {
                Game_Id = ConfigMan.Instance.GameId.ToString();
            }

            if (!string.IsNullOrEmpty(ConfigMan.Instance.ClientId))
            {
                Client_id = ConfigMan.Instance.ClientId.ToString();
            }

            if (ConfigMan.Instance.IsDemo)
            {
                CashAmount = "2000";
            }
            else
            {
                CashAmount = playerInfo.wallet_balance;
            }

            manualStart();
        }
        else
        {
            manualStart();
        }
    }

    public void UpdateAmount ()
    {
        if (ConfigMan.Instance.IsDemo)
        {
            CashAmount = "2000";
        }
        else
        {
            CashAmount = playerInfo.wallet_balance;
        }
    }
}
