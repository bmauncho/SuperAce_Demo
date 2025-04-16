using UnityEngine;
using UnityEngine.Networking;
using System;

public static class UnityWebRequestHelper
{
    public static UnityWebRequest GetWithTimestamp(string url)
    {
        string urlWithTimestamp = url + "&textid=" + DateTime.Now.Ticks;
        return UnityWebRequest.Get(urlWithTimestamp);
    }

    public static UnityWebRequest PostWithTimestamp(string url, WWWForm formData)
    {
        string urlWithTimestamp = url + "&textid=" + DateTime.Now.Ticks;
        return UnityWebRequest.Post(urlWithTimestamp, formData);
    }
  
}
