using UnityEngine;
using UnityEngine.UI;

public class DontShowNeextTime : MonoBehaviour
{
    public Toggle Toggle;

    public void DontShow ()
    {
        if (Toggle.isOn)
        {
            PlayerPrefs.SetInt("DontshowNextTime" , 1);
        }
        else
        {
            PlayerPrefs.SetInt("DontshowNextTime" , 0);
        }
    }

    public bool CheckDontShow ()
    {
        if (PlayerPrefs.GetInt("DontshowNextTime") == 1)
        {
            return true;
        }
        return false;
    }
}
