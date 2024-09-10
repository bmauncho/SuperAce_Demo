using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] GameObject SettingBtn;
    public GameObject SettingsBttons;
    public GameObject Info;
    public GameObject AutoSpinSetting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateButtons ()
    {
        SettingsBttons.SetActive(true);
    }

    public void DeactivateButtons ()
    {
        SettingsBttons.SetActive(false);
        SettingBtn.GetComponent<Toggle>().isOn = false;
    }

    public void SettingPressed ()
    {
        if(SettingBtn.GetComponent<Toggle>().isOn)
        {
            ActivateButtons();
        }
        else
        {
            DeactivateButtons();
        }
    }

    public void ActivateInfo ()
    {
        Info.SetActive(true);
    }

    public void DeactivateInfo ()
    {
        Info.SetActive(false );
    }

    public void ActivateAutoSpinSetting ()
    {
        AutoSpinSetting.SetActive(true);
    }

    public void DeactivateAutoSpinSetting ()
    {
        AutoSpinSetting.SetActive(false);
    }
}
