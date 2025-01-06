using TMPro;
using UnityEngine;

public class InternetBehaviour : MonoBehaviour
{
    public GameObject UnavailableWifiBar;
    public GameObject badWifiBar;
    public GameObject OkayWifiBar;
    public GameObject GoodWifiBar;
    public GameObject BestWifiBar;

    public InternetCheck currentConnection;
    public TextMeshProUGUI versionText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string gameVersion = Application.version;
        versionText.text = $"V_{gameVersion}";
    }

    // Update is called once per frame
    void Update()
    {
        if (currentConnection)
        {
            // Reset all bars first
            UnavailableWifiBar.SetActive(false);
            badWifiBar.SetActive(false);
            OkayWifiBar.SetActive(false);
            GoodWifiBar.SetActive(false);
            BestWifiBar.SetActive(false);

            // Activate bars based on connection status
            switch (currentConnection.connection)
            {
                case InternetConnection.Unavailable:
                    UnavailableWifiBar.SetActive(true);
                    break;
                case InternetConnection.Bad:
                    badWifiBar.SetActive(true);
                    break;
                case InternetConnection.Okay:
                    badWifiBar.SetActive(true);
                    OkayWifiBar.SetActive(true);
                    break;
                case InternetConnection.Good:
                    badWifiBar.SetActive(true);
                    OkayWifiBar.SetActive(true);
                    GoodWifiBar.SetActive(true);
                    break;
                case InternetConnection.Best:
                    badWifiBar.SetActive(true);
                    OkayWifiBar.SetActive(true);
                    GoodWifiBar.SetActive(true);
                    BestWifiBar.SetActive(true);
                    break;
            }
        }
    }
}
