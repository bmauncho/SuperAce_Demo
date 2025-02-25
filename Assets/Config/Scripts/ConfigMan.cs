using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
[System.Serializable]
public class ConfigRefresh : UnityEvent { }
public class ConfigMan : MonoBehaviour
{
    public static ConfigMan Instance;
    public bool ReceivedConfigs;
    [Header("Config Details")]
    public bool IsDemo;

    public string PlayerId;
    public string GameId;
    public string ClientId;
    public ConfigRefresh Refresh;


    [Header("Debug Canvas")]
    public GameObject TheDebugObj;
    public TMP_InputField PlayerIdText;
    public TMP_InputField GameIdText;
    public TMP_InputField ClientIdText;

    void Start()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        if (!Application.isEditor)
        {
            TheDebugObj.SetActive(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TheDebugObj.SetActive(true);
        }
    }
    public void ToggleDemoMode(Toggle which)
    {
        IsDemo = which.isOn;
    }
    void OnApplicationFocus(bool hasFocus)
    {
        Silence(!hasFocus);
    }

    void OnApplicationPause(bool isPaused)
    {
        Silence(isPaused);
    }

    private void Silence(bool silence)
    {
        AudioListener.pause = silence;
        // Or / And
        AudioListener.volume = silence ? 0 : 1;
    }
    public void PassPlayerId(string TheId)
    {
        PlayerId = TheId;
        ReceivedConfigs = true;
        Debug.Log("TheFetchedPlayerIdIs_" + TheId);
        Invoke(nameof(RefreshConfig), 0.1f);
    }
    void RefreshConfig()
    {
        Refresh.Invoke();

    }
    public void PassGameId(string Id)
    {
        GameId = Id;
        Debug.Log("TheFetchedGameIdIs_" + GameId);
    }
    public void PassClientId(string Id)
    {
        ClientId = Id;
        Debug.Log("TheFetchedClientIdIs_" + ClientId);
    }
    public void CheckTextInput()
    {
        if (!string.IsNullOrEmpty(PlayerIdText.text))
        {
            PassPlayerId(PlayerIdText.text);
        }
        if (!string.IsNullOrEmpty(GameIdText.text))
        {
            PassGameId(GameIdText.text);
        }
        if (!string.IsNullOrEmpty(ClientIdText.text))
        {
            PassClientId(ClientIdText.text);
        }
    }


}