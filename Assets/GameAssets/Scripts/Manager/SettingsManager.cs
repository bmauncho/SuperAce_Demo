using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public bool Sound = false;
    public SettingsController SettingsController_;

    private void OnEnable ()
    {
        RefreshSettings();
    }

    public void Start ()
    {
        RefreshSettings();
    }

    public void RefreshSettings ()
    {
        if (PlayerPrefs.HasKey("Sound_Setting"))
        {
            if (PlayerPrefs.GetFloat("Sound_Setting") > 0)
            {
                Sound = true;
            }
            else
            {
                Sound = false;
            }
        }
        else
        {
            PlayerPrefs.SetFloat("Sound_Setting" , 1);
            Sound = true;
        }
    }
    public void ToogleSound ( bool IsOn )
    {
        Sound = IsOn;
        if (IsOn)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
        PlayerPrefs.SetFloat("Sound_Setting" , AudioListener.volume);
    }
    public float SoundVolume ()
    {
        return PlayerPrefs.GetFloat("Sound_Setting");
    }
}
