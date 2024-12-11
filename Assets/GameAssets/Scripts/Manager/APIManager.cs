using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

[System.Serializable]
public class _GameInfo
{
    public _game game;
    public float betAmount = 1000;
    public string gameMode = "Normal";
}

[System.Serializable]
public class _game
{
    public int id = 2;
    public string name = "Super Ace";
}

[System.Serializable]
public class FinalData
{
    public float freeSpins;
    public float AmountWon;
   [HideInInspector] public Rows [] rows;
}

[System.Serializable]
public class Rows
{
    public CardInfo [] cards;
}

[System.Serializable]
public class CardInfo
{
    public string name;
    public bool golden;
}
[System.Serializable]
public class rowData
{
    public List<CardInfo> infos = new List<CardInfo>();
}


public class APIManager : MonoBehaviour
{
    private const string ApiUrl = "https://slots.ibibe.africa/spin/superace";
    public FinalData finalData;
    public float BetAmount;
    [Space(10)]
    public List<rowData> rows = new List<rowData>(5);
    List<CardInfo> infos = new List<CardInfo>();

    private void Update ()
    {
        if (CommandCentre.Instance)
        {
            BetAmount = CommandCentre.Instance.BetManager_.BetAmount;
        }
    }

    [ContextMenu("FetchInfo")]
    public void FetchInfo ()
    {
        _GameInfo Data = new _GameInfo();
        Data.betAmount = BetAmount;
        string jsonString = JsonUtility.ToJson(Data);
        StartCoroutine(_FetchGridInfo(ApiUrl , jsonString , () =>
        {
            populateRows();
        }));
    }

    IEnumerator _FetchGridInfo ( string url , string bodyJsonString,Action OnComplete =null )
    {
        var request = new UnityWebRequest(url , "POST");
        byte [] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type" , "application/json");
        yield return request.SendWebRequest();
        infos.Clear();
        rows.Clear();
        //Debug.Log("Status Code: " + request.responseCode);
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);
            string _Data = JsonHelper.GetJsonObject(request.downloadHandler.text , "data");
            Debug.Log(_Data);
            finalData = JsonUtility.FromJson<FinalData>(_Data);
            string [] newdata = _Data.Split('[');
            bool Firsskip = false;
            for (int i = 0 ; i < newdata.Length ; i++)
            {
                if (Firsskip)
                {
                    string [] _newdata = newdata [i].Split(']');
                    Debug.Log(_newdata [0]);
                    string [] Newdata1 = _newdata [0].Split('}');
                    rowData rowData = new rowData();
                    rows.Add(rowData);
                    for (int y = 0 ; y < Newdata1.Length ; y++)
                    {
                        Debug.Log(Newdata1 [y]);
                        string [] _newdata2 = Newdata1 [y].Split('{');

                        for (int r = 0 ; r < _newdata2.Length ; r++)
                        {
                            char [] lenght = _newdata2 [r].ToCharArray();
                            if (!string.IsNullOrEmpty(_newdata2 [r]) &&
                               lenght.Length > 3)
                            {
                                Debug.Log(_newdata2 [r]);
                                CardInfo _card = new CardInfo();
                                _card = JsonUtility.FromJson<CardInfo>("{" + _newdata2 [r] + "}");
                                infos.Add(_card);
                            }

                        }
                    }
                }
                Firsskip = true;
            }
            OnComplete?.Invoke();
        }
    }

    void populateRows ()
    {
        for(int i = 0 ; i < rows.Count ; i++)
        {
            for(int j = 0 ;j<5 ;j++)
            {
                rows [i].infos.Add(infos [j]);
            }
        }
    }

    public bool IsFreeGame ()
    {
        return finalData.freeSpins >= 10;
    }
}


