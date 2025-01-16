using UnityEngine;

public class SoundSettings : MonoBehaviour
{
    SoundManager soundManager;
    public GameObject BackgroundMusic;
    public GameObject SoundEffects;
    public bool isBgFirstTime = true;
    public bool isSFXFirstTime = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundManager = GetComponent<SoundManager>();
    }

    public float setBagroundMusicVolume ()
    {
        float value = 0f;
        if(isBgFirstTime)
        {
            isBgFirstTime = false;
            value = 1;
        }
        else
        {
            value = BackgroundMusic.GetComponentInChildren<SettingSlider>().sliderValue;
        }
        return value;
    }

    public float setSoundEffectsVolume ()
    {
        float value = 0f;
        if (isSFXFirstTime)
        {
            isSFXFirstTime = false;
            value = 1;
        }
        else
        {
            value = SoundEffects.GetComponentInChildren<SettingSlider>().sliderValue;
        }
        return value;
    }
}
