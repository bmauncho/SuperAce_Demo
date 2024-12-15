using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class BetUpDateData
{
    public int bet_id;
    public float amount_won;
}

[System.Serializable]
public class UpdateBetResponse
{
    [HideInInspector]public string message;
    public int bet_id;
    public float amount_won;
    public float new_wallet_balance;
    [HideInInspector] public string status;
    [HideInInspector] public string error; // For error handling
}
public class BetUpdaterAPI : MonoBehaviour
{
    private const string ApiUrl = "https://admin1.ibibe.africa/api/update_bet";
    public UpdateBetResponse updateBetResponse;

    [ContextMenu("UpdateBet")]
    public void UpdateBet ()
    {
        BetUpDateData Data = new BetUpDateData
        {
            bet_id = 1,
            amount_won = 1,
        };
        string jsonPayload =  JsonUtility.ToJson(Data , true);
        Debug.Log(jsonPayload);
        StartCoroutine(SendUpdateBetRequest(jsonPayload));
    }

    private IEnumerator SendUpdateBetRequest(string jsonPayload )
    {
        UnityWebRequest request = new UnityWebRequest(ApiUrl , "POST");
        byte [] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type" , "application/json");

        // Send the request
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);

            // Parse successful response
            UpdateBetResponse responseData = JsonUtility.FromJson<UpdateBetResponse>(request.downloadHandler.text);

            Debug.Log($"message : {responseData.message}," +
              $"betId : {responseData.bet_id}," +
              $"amountWon : {responseData.amount_won}" +
              $"newWalletBalance : {responseData.new_wallet_balance}," +
              $"status : {responseData.status}," +
              $"error : {responseData.error}");
        }
    }
}
