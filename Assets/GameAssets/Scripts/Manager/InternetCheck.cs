using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class InternetCheck : MonoBehaviour
{
    public GameObject internetErrorPanel;
    public bool IsInternetEnabled = false;
    private bool IsOffLine = false;
    private float checkInterval = 5f; // Time in seconds between checks
    private float timer = 0f;
    private int maxRetries = 3; // Number of retry attempts on failure
    private int retryCount = 0;

    void Update ()
    {
        timer += Time.unscaledDeltaTime;

        if (timer >= checkInterval)
        {
            timer = 0f;
            // Check the internet reachability before making requests
            CheckInternetReachability();
        }
    }

    void CheckInternetReachability ()
    {
        // Check if the device has any form of internet connection (WiFi, Mobile data)
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            // No internet available on the device
            IsInternetEnabled = false;
            HandleNoInternetConnection();
        }
        else
        {
            // Internet is reachable (WiFi or Mobile data), proceed to network request check
            StartCoroutine(CheckInternetConnectionCoroutine());
        }
    }

    private IEnumerator CheckInternetConnectionCoroutine ()
    {
        yield return StartCoroutine(checkforInternet_OptionA());

        yield return StartCoroutine(checkforInternet_OptionB());
    }

    IEnumerator checkforInternet_OptionA ()
    {
        string url = "https://www.cloudflare.com/cdn-cgi/trace?" + Random.Range(0 , 100000); // Append random query string

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.timeout = 5;
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                retryCount++;
                if (retryCount >= maxRetries)
                {
                    IsInternetEnabled = false;
                    HandleNoInternetConnection();
                    retryCount = 0; // Reset retry counter
                }
                else
                {
                    Debug.Log("Retrying connection...");
                }
            }
            else
            {
                IsInternetEnabled = true;
                retryCount = 0; // Reset retry counter on success
                HandleInternetConnection();
            }
        }
    }

    IEnumerator checkforInternet_OptionB ()
    {
        WWW www_ = new WWW("https://google.com");
        yield return www_;
        if (www_.error != null)
        {
            retryCount++;
            if (retryCount >= maxRetries)
            {
                IsInternetEnabled = false;
                HandleNoInternetConnection();
                retryCount = 0; // Reset retry counter
            }
            else
            {
                Debug.Log("Retrying connection...");
            }
        }
        else
        {
            IsInternetEnabled = true;
            retryCount = 0; // Reset retry counter on success
            HandleInternetConnection();
        }
    }


    void HandleNoInternetConnection ()
    {
        if (internetErrorPanel != null)
        {
            internetErrorPanel.SetActive(true);
        }
        CommandCentre.Instance.SoundManager_.PauseAllSounds();
        Time.timeScale = 0;
        IsOffLine = true;
        Debug.Log("No internet connection. Game is paused.");
    }

    void HandleInternetConnection ()
    {
        if (internetErrorPanel != null)
        {
            internetErrorPanel.SetActive(false);
        }
        if (IsOffLine)
        {
            Time.timeScale = 1;
            IsOffLine = false;
            CommandCentre.Instance.SoundManager_.UnPauseAllSounds();
            Debug.Log("Internet connection restored. Game is resumed.");
        }
    }
}
