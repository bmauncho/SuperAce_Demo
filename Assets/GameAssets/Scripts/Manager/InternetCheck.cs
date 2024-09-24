using UnityEngine;
using UnityEngine.Networking; // Include this for UnityWebRequest
using System.Collections; // Required for using IEnumerator

public class InternetCheck : MonoBehaviour
{
    public GameObject internetErrorPanel;
    public bool IsInternetEnabled = false;
    private bool IsOffLine = false;
    private float checkInterval = 5f; // Time in seconds between checks
    private float timer = 0f; // Timer to track elapsed time

    void Update ()
    {
        // Increment the timer using unscaled delta time
        timer += Time.unscaledDeltaTime;

        // Check if the timer has reached the interval
        if (timer >= checkInterval)
        {
            timer = 0f; // Reset the timer
            StartCoroutine(CheckInternetConnectionCoroutine()); // Start the internet check coroutine
        }
    }

    private IEnumerator CheckInternetConnectionCoroutine ()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://www.google.com"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                IsInternetEnabled = false ;
                Debug.Log("No internet connection");
                HandleNoInternetConnection();
            }
            else
            {
                IsInternetEnabled = true;
                Debug.Log("Connected to the internet");
                HandleInternetConnection();
            }
        }
    }

    void HandleNoInternetConnection ()
    {
        // Optionally, show a message or disable gameplay elements
        if (internetErrorPanel != null)
        {
            internetErrorPanel.SetActive(true); // Display an error message (UI panel)
        }
        // Disable gameplay by pausing or stopping player input, etc.
        Time.timeScale = 0; // Pause the game
        IsOffLine = true; // Update offline status
        Debug.Log("No internet connection. Game is paused.");
    }

    void HandleInternetConnection ()
    {
        if (internetErrorPanel != null)
        {
            internetErrorPanel.SetActive(false); // Hide the error message (UI panel)
        }
        if (IsOffLine) // Check if we were offline before
        {
            Time.timeScale = 1; // Resume the game
            IsOffLine = false; // Update online status
            Debug.Log("Internet connection restored. Game is resumed.");
        }
    }
}
