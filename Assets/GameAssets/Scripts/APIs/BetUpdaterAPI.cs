using Newtonsoft.Json;
using System.Collections;
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
    public UpdateBetResponse updateBetResponse_;


    private void Start ()
    {
        updateBetResponse_.new_wallet_balance = CommandCentre.Instance.CashManager_.CashAmount;
    }
    [ContextMenu("setUpBalance")]
    void setUpBalance ()
    {
        updateBetResponse_.new_wallet_balance = CommandCentre.Instance.CashManager_.CashAmount;
    }

    [ContextMenu("UpdateBet")]
    public void UpdateBet ()
    {
        Debug.Log(CommandCentre.Instance.APIManager_.betPlacingAPI_.response.bet_id);
        BetUpDateData Data = new BetUpDateData
        {
            bet_id = CommandCentre.Instance.APIManager_.betPlacingAPI_.response.bet_id,
            amount_won = CommandCentre.Instance.APIManager_.GameDataAPI_.AmountWon,
        };
        string jsonPayload = JsonConvert.SerializeObject(Data , Formatting.Indented);
        //Debug.Log(jsonPayload);
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
            //Debug.Log("Received: " + request.downloadHandler.text);

            // Parse successful response
            UpdateBetResponse responseData = JsonConvert.DeserializeObject<UpdateBetResponse>(request.downloadHandler.text);

           // Debug.Log($"message : {responseData.message}," +
             // $"betId : {responseData.bet_id}," +
             // $"amountWon : {responseData.amount_won}" +
             // $"newWalletBalance : {responseData.new_wallet_balance}," +
             // $"status : {responseData.status}," +
             // $"error : {responseData.error}");


            UpdateBetResponse data = new UpdateBetResponse
            {
                message = responseData.message,
                bet_id = responseData.bet_id,
                amount_won = responseData.amount_won,
                new_wallet_balance = responseData.new_wallet_balance,
                status = responseData.status,
                error = responseData.error,
            };

            updateBetResponse_ = data;
            CommandCentre.Instance.CashManager_.CashAmount = updateBetResponse_.new_wallet_balance;
        }
    }
}
