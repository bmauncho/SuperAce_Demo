using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
public class FetchUserInfo : MonoBehaviour
{
    public ConfigMan configMan;
    public Toggle DemoToggle;
   
    public void SetPlayerId(string userId)
    {
        configMan.PassPlayerId(userId);
       configMan.PlayerIdText.text = userId;

        // Use the userId in your game logic
    }
    public void SetMode(int mode)
    {
        Debug.Log("TheMode: " +mode);
        if (mode == 0)
        {
            configMan.IsDemo = true;
        }
        else
        {
            configMan.IsDemo = false;
        }
        if (DemoToggle)
        {
            DemoToggle.isOn = configMan.IsDemo;
        }
    }
    public void SetGameId(int id)
    {

        //Debug.Log("TheGameId: " + id.ToString());
        configMan.PassGameId(id.ToString());
       configMan.GameIdText.text =  id.ToString();

    }
    public void SetClientId(int id)
    {
        //Debug.Log("TheGameId: " + id.ToString());
        configMan.PassClientId(id.ToString());
       configMan.ClientIdText.text = id.ToString();

    }
    public void SetLanguage(string id)
    {
        Debug.Log("TheLanguage: " + id.ToString());
        if (id == "en")
        {
            LanguageMan.instance._SetLanguage(TheLanguage.English);
        }
        else if (id == "zh") {
            LanguageMan.instance._SetLanguage(TheLanguage.Chinese);
        }
        else if (id == "es")
        {
            LanguageMan.instance._SetLanguage(TheLanguage.Spanish);
        }
        else if (id == "ja")
        {
            LanguageMan.instance._SetLanguage(TheLanguage.Japan);
        }
        else if (id == "sw")
        {
            LanguageMan.instance._SetLanguage(TheLanguage.Swahili);
        }
        else if (id == "da")
        {
            LanguageMan.instance._SetLanguage(TheLanguage.Danish);
        }
        else if (id == "th")
        {
            LanguageMan.instance._SetLanguage(TheLanguage.Thai);
        }
        else if (id == "id")
        {
            LanguageMan.instance._SetLanguage(TheLanguage.Indonesia);
        }
        else if (id == "vi")
        {
            LanguageMan.instance._SetLanguage(TheLanguage.Vietnam);
        }
        else if (id == "pt-PT")
        {
            LanguageMan.instance._SetLanguage(TheLanguage.Portoguese);
        }
        else if (id == "ko")
        {
            LanguageMan.instance._SetLanguage(TheLanguage.Korea);
        }
        else if (id == "my")
        {
            LanguageMan.instance._SetLanguage(TheLanguage.Burmese);
        }
    }
}
