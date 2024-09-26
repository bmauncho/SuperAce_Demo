using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class LargeBets_FetchValues : MonoBehaviour
{
    public string url;
    public float PercentageValue;
    void Start()
    {
        FetchData();
    }

   
    public void FetchData()
    {
        StartCoroutine(GetRequest(url));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    //Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    //Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    ParseValue(webRequest.downloadHandler.text);
                    break;
            }
        }
    }
    void ParseValue(string TheValue)
    {
        thedata gameData = new thedata();
        gameData = JsonUtility.FromJson<thedata>(TheValue);
       // Debug.Log(gameData.winrate);

        PercentageValue = float.Parse(gameData.winrate);

    }
}
