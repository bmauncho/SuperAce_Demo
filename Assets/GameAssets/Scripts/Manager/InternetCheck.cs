using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public enum InternetConnection
{
    Unavailable,
    Bad,
    Okay,
    Good,
    Best
}

public class InternetCheck : MonoBehaviour
{
    public GameObject internetErrorPanel;
    public bool IsInternetEnabled = false;
    private bool IsOffLine = false;
    private float checkInterval = 5f; // Time in seconds between checks
    private float timer = 0f;
    private int maxRetries = 3; // Number of retry attempts on failure
    private int retryCount = 0;

    public InternetConnection connection;

    void Update ()
    {
        timer += Time.unscaledDeltaTime;

        if (timer >= checkInterval)
        {
            timer = 0f;
           // Debug.Log("Starting internet reachability check.");
            CheckInternetReachability();
        }
    }

    void CheckInternetReachability ()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            IsInternetEnabled = false;
            connection = InternetConnection.Unavailable;
           // Debug.Log("No internet reachability detected.");
            HandleNoInternetConnection();
        }
        else
        {
            //Debug.Log("Internet reachability detected. Proceeding to connection quality check.");
            StartCoroutine(CheckInternetConnectionCoroutine());
        }
    }

    private IEnumerator CheckInternetConnectionCoroutine ()
    {
        //Debug.Log("Starting connection quality test using Option A.");
        yield return StartCoroutine(CheckInternetQuality());

        if (!IsInternetEnabled)
        {
            connection = InternetConnection.Bad;
          //  Debug.Log("Connection deemed BAD after quality test.");
            HandleNoInternetConnection();
        }
    }

    IEnumerator CheckInternetQuality ()
    {
        string testUrl = "https://www.cloudflare.com/cdn-cgi/trace";

        using (UnityWebRequest www = UnityWebRequest.Get(testUrl))
        {
            www.timeout = 5; // Timeout for the request
            float startTime = Time.time;

           // Debug.Log($"Sending test request to {testUrl}...");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                float responseTime = Time.time - startTime;
               // Debug.Log($"Test request successful. Response time: {responseTime:F2} seconds.");

                IsInternetEnabled = true;
                retryCount = 0;

                // Determine connection quality based on response time
                if (responseTime < 0.5f)
                {
                    connection = InternetConnection.Best;
                   // Debug.Log("Connection quality determined: BEST.");
                }
                else if (responseTime < 1f)
                {
                    connection = InternetConnection.Good;
                   // Debug.Log("Connection quality determined: GOOD.");
                }
                else
                {
                    connection = InternetConnection.Okay;
                   // Debug.Log("Connection quality determined: OKAY.");
                }

                HandleInternetConnection();
            }
            else
            {
                retryCount++;
                //Debug.Log($"Test request failed. Error: {www.error}. Retry count: {retryCount}/{maxRetries}");

                IsInternetEnabled = false;

                if (retryCount >= maxRetries)
                {
                    connection = InternetConnection.Bad;
                    Debug.Log("Max retries reached. Connection quality determined: BAD.");
                }
                else
                {
                    Debug.Log("Retrying connection...");
                }
            }
        }
    }

    void HandleNoInternetConnection ()
    {
        //Debug.Log("Handling no internet connection...");
        if (internetErrorPanel != null)
        {
            internetErrorPanel.SetActive(true);
        }

        CommandCentre.Instance.SoundManager_.PauseAllSounds();
        Time.timeScale = 0;
        IsOffLine = true;

        //Debug.Log($"Connection State: {connection} - Game is paused due to no internet.");
    }

    void HandleInternetConnection ()
    {
        //Debug.Log("Handling internet connection restoration...");
        if (internetErrorPanel != null)
        {
            internetErrorPanel.SetActive(false);
        }

        if (IsOffLine)
        {
            Time.timeScale = 1;
            IsOffLine = false;
            CommandCentre.Instance.SoundManager_.UnPauseAllSounds();

            //Debug.Log($"Connection State: {connection} - Game is resumed with internet restored.");
        }
    }
}
