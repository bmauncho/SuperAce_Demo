using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class BetRequest
{
    public int customer_id;
    public float amount;
}

[System.Serializable]
public class BetResponse
{
    [HideInInspector] public string message = "Bet placed successfully";
    public int bet_id = 1;
    public float new_wallet_balance = 448.0f;
    [HideInInspector]public string status = "lost";
    [HideInInspector]public string error = "Insufficient balance"; // Optional field for errors
}

public class BetPlacingAPI : MonoBehaviour
{
    private const string ApiUrl = "https://admin1.ibibe.africa/api/place_bet";
    public BetResponse response;
    public float BetAmount;
    public int customerId;

    private void Update ()
    {
        if (CommandCentre.Instance)
        {
            BetAmount = CommandCentre.Instance.BetManager_.BetAmount;
        }
    }

    [ContextMenu("Bet")]
    public void Bet()
    {
        BetRequest Data = new BetRequest
        {
            customer_id = customerId ,
            amount = BetAmount ,
        };
        string jsonString = JsonUtility.ToJson(Data,true);
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
        Debug.Log("Called");

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);

            // Parse successful response
            BetResponse responseData = JsonUtility.FromJson<BetResponse>(request.downloadHandler.text);
            //Debug.Log($"message : {responseData.message}," +
            //    $"betId : {responseData.bet_id}," +
            //    $"newWalletBalance : {responseData.new_wallet_balance}," +
            //    $"status : {responseData.status}," +
            //    $"error : {responseData.error}");

            BetResponse betResponse = new BetResponse
            {
                message = responseData.message ,
                bet_id = responseData.bet_id ,
                new_wallet_balance = responseData.new_wallet_balance ,
                status = responseData.status ,
                error = responseData.error ,
            };

            response = betResponse;

        }
        else
        {
            Debug.LogWarning("request is unsuccessfull");
        }
    }
    #region
    
    //private IEnumerator PlaceBet ( string jsonPayload )
    //{
    //    Debug.Log($"Sending request to: {ApiUrl}");
    //    Debug.Log($"Payload: {jsonPayload}");

    //    UnityWebRequest request = new UnityWebRequest(ApiUrl , "POST");
    //    byte [] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
    //    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //    request.downloadHandler = new DownloadHandlerBuffer();
    //    request.SetRequestHeader("Content-Type" , "application/json");

    //    // Send request
    //    yield return request.SendWebRequest();
    //    Debug.Log("Request sent.");

    //    if (request.result == UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log("Response: " + request.downloadHandler.text);
    //    }
    //    else
    //    {
    //        Debug.LogError($"Request failed: {request.error}");
    //        Debug.LogError($"Response: {request.downloadHandler.text}");
    //    }
    //}
    #endregion
}
