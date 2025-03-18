using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class BetRequest
{
    public string player_id;
    public string amount;
    public string bet_id;
    public string game_id;
    public string client_id;
}

[System.Serializable]
public class BetResponse
{
    public string message = "Bet placed successfully";
    public int bet_id;
    public int game_id;
    public float new_wallet_balance;
    public ExternalResponse externalResponse_;
}

[System.Serializable]
public class ExternalResponse
{
    public string client_id;
    public float totalBets;
    public float totalWins;
}

public class BetPlacingAPI : MonoBehaviour
{
    [Header("API Settings")]
    private const string ApiUrl = "https://admin-api.ibibe.africa/api/v1/bet/place_bet";
    public BetResponse response;
    public float BetAmount;
    public int customerId;
    public int game_id = 32;
    public int client_id = 12345;

    [Header("Retry Settings")]
    public int tries;
    public int maxtries;
    public bool IsUpdated;

    private void Start ()
    {
        response = new BetResponse
        {
            new_wallet_balance = PlayerPrefs.GetFloat("TotalCash")
        };
    }
    private void Update ()
    {
        if (CommandCentre.Instance)
        {
            BetAmount = CommandCentre.Instance.BetManager_.BetAmount;
        }
    }

    [ContextMenu("Bet")]
    public void Bet ()
    {
        IsUpdated = false;
        int bet_id = Random.Range(100 , 10000000);

        BetRequest Data = new BetRequest
        {
            player_id = customerId.ToString() ,
            amount = BetAmount.ToString() ,
            bet_id = bet_id.ToString() ,
            game_id = game_id.ToString() ,
            client_id = client_id.ToString()
        };

        string jsonString = JsonUtility.ToJson(Data , true);
        Debug.Log("Bet placing Payload: " + jsonString);  // Debug the JSON sent
        StartCoroutine(PlaceBet(jsonString));
    }

    private IEnumerator PlaceBet ( string jsonPayload )
    {
        // Create UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(ApiUrl , "POST");
        byte [] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type" , "application/json");

        // Send request
        yield return request.SendWebRequest();
        //Debug.Log("Called");
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log("Response: " + request.downloadHandler.text);  // Print the API error response
        if (request.result == UnityWebRequest.Result.Success)
        {
            string output = request.downloadHandler.text;
            var responseData = JsonConvert.DeserializeObject<BetResponse>(output);

            // Pretty-print JSON response
            string formattedOutput = JsonConvert.SerializeObject(responseData , Formatting.Indented);
            Debug.Log("PlaceBet API Received: " + formattedOutput);

            BetResponse betResponse = new BetResponse
            {
                message = responseData.message ,
                bet_id = responseData.bet_id ,
                game_id = responseData.game_id ,
                new_wallet_balance = responseData.new_wallet_balance ,
                externalResponse_ = responseData.externalResponse_ != null ? new ExternalResponse
                {
                    client_id = responseData.externalResponse_.client_id ,
                    totalBets = responseData.externalResponse_.totalBets ,
                    totalWins = responseData.externalResponse_.totalWins
                } : null // Handle case where externalResponse_ is null
            };

            Debug.Log($"previous cashAmount : {CommandCentre.Instance.CashManager_.CashAmount} : current amount : {responseData.new_wallet_balance}");
            response = betResponse;
            IsUpdated = true;
            float newCashAmount = response.new_wallet_balance;
            CommandCentre.Instance.CashManager_.UpdateCashAmount(( newCashAmount ));
        }
        else
        {
            HandleRetry();
        }
    }

    private void HandleRetry ()
    {
        if (tries < maxtries)
        {
            customerId++;
            tries++;
            Debug.Log($"Retrying... Attempt {tries}/{maxtries}");
            Bet();
        }
        else
        {
            Debug.LogWarning("Request unsuccessful after maximum retries.");
        }
    }
}
