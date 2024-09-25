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
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                // Internet reachability detected, proceed with network request check
                StartCoroutine(CheckInternetConnectionCoroutine());
            }
            else
            {
                // No internet reachability at all
                IsInternetEnabled = false;
                HandleNoInternetConnection();
            }
        }
    }

    private IEnumerator CheckInternetConnectionCoroutine ()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://www.cloudflare.com/cdn-cgi/trace")) // Lightweight endpoint
        {
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
