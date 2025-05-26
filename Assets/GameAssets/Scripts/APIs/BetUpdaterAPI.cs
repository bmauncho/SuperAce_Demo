using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class BetUpDateData
{
    public string bet_id;
    public string amount_won;
    public string client_id;
}

[System.Serializable]
public class UpdateBetResponse
{
    [HideInInspector]public string message;
    public string bet_id;
    public float amount_won;
    public double new_wallet_balance;
    [HideInInspector] public string status;
    [HideInInspector] public string error; // For error handling
}
public class BetUpdaterAPI : MonoBehaviour
{
    //private const string ApiUrl = "https://admin-api.ibibe.africa/api/v1/update_bet";
    public UpdateBetResponse updateBetResponse_;
    public double CashAmount;
    public double previousCashAmount;
    public double NewCashAmount;
    public bool IsBetUpdated = false;

    private void Start ()
    {
        //updateBetResponse_.new_wallet_balance = CommandCentre.Instance.CashManager_.CashAmount;
    }
    [ContextMenu("setUpBalance")]
    void setUpBalance ()
    {
        updateBetResponse_.new_wallet_balance = CommandCentre.Instance.CashManager_.CashAmount;
    }

    [ContextMenu("UpdateBet")]
    public void UpdateBet (string betid,string AmountWon,string Clientid)
    {
        IsBetUpdated = false;
        //Debug.Log(CommandCentre.Instance.APIManager_.betPlacingAPI_.response.bet_id); 
        BetUpDateData Data = new BetUpDateData
        {
            bet_id = betid,
            amount_won = AmountWon,
            client_id = Clientid
        };
        string jsonPayload = JsonConvert.SerializeObject(Data , Formatting.Indented);
        Debug.Log($" Bet Updater Payload : {jsonPayload}");
        StartCoroutine(SendUpdateBetRequest(jsonPayload));
    }

    private IEnumerator SendUpdateBetRequest(string jsonPayload )
    {
        string ApiUrl = ConfigMan.Instance.Base_url + "/api/v1/update_bet";

        UnityWebRequest request = new UnityWebRequest(ApiUrl , "POST");
        byte [] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type" , "application/json");

        // Send the request
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string output = request.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<UpdateBetResponse>(output);
            object parsedResponse = JsonConvert.DeserializeObject(output);
            string formattedOutput = JsonConvert.SerializeObject(parsedResponse , Formatting.Indented);

           //Debug.Log("UpdateBet api Received: " + formattedOutput);
            // Parse successful response
            UpdateBetResponse responseData = JsonConvert.DeserializeObject<UpdateBetResponse>(output);

      
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
            Debug.Log($"previous cashAmount : {CommandCentre.Instance.CashManager_.CashAmount} : current amount : {updateBetResponse_.new_wallet_balance}");
            double newCashAmount = updateBetResponse_.new_wallet_balance;
            CashAmount = newCashAmount;
            NewCashAmount = CashAmount;
            float amountWon = responseData.amount_won;
            //CommandCentre.Instance.CashManager_.IncreaseWinings(amountWon);
            IsBetUpdated = true;
        }
        else
        {
            Debug.Log("Error : " + request.error);
            IsBetUpdated = true;
        }
    }
}
