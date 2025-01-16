using UnityEngine;
using UnityEngine.UI;

public class SettingSlider : MonoBehaviour
{
    public Image slider;
    public float sliderValue;

    private readonly float [] markers = { 0f , 1f / 3f , 2f / 3f , 1f }; // Marker positions

    void Start ()
    {
        if(PlayerPrefs.HasKey("sliderValue" + gameObject.transform.parent.name))
        {
            sliderValue = PlayerPrefs.GetFloat("sliderValue" + gameObject.transform.parent.name);
        }
        else
        {
            sliderValue = 1;
        }
        
        slider.fillAmount = sliderValue;
    }

    void Update ()
    {
        slider.fillAmount = Mathf.MoveTowards(slider.fillAmount , sliderValue , 5 * Time.deltaTime);
    }

    [ContextMenu("Decrease")]
    public void Decrease ()
    {
        // Find the current marker index and move to the previous one if possible
        for (int i = markers.Length - 1 ; i >= 0 ; i--)
        {
            if (sliderValue > markers [i])
            {
                sliderValue = markers [i];
                PlayerPrefs.SetFloat("sliderValue" + gameObject.transform.parent.name , sliderValue);
                break;
            }
        }
        
    }
    [ContextMenu("Increase")]
    public void Increase ()
    {
        // Find the current marker index and move to the next one if possible
        for (int i = 0 ; i < markers.Length ; i++)
        {
            if (sliderValue < markers [i])
            {
                sliderValue = markers [i];
                PlayerPrefs.SetFloat("sliderValue" + gameObject.transform.parent.name , sliderValue);
                break;
            }
        }
    }

    public void Marker_1 ()
    {
        sliderValue = markers [0];
        PlayerPrefs.SetFloat("sliderValue" + gameObject.transform.parent.name , sliderValue);
    }

    public void Marker_2 ()
    {
        sliderValue = markers [1];
        PlayerPrefs.SetFloat("sliderValue" + gameObject.transform.parent.name , sliderValue);
    }

    public void Marker_3 ()
    {
        sliderValue = markers [2];
        PlayerPrefs.SetFloat("sliderValue" + gameObject.transform.parent.name , sliderValue);
    }

    public void Marker_4 ()
    {
        sliderValue = markers [3];
        PlayerPrefs.SetFloat("sliderValue" + gameObject.transform.parent.name , sliderValue);
    }
}
