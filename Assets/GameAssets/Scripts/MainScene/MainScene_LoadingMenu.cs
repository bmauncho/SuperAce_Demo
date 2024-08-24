using UnityEngine;

public class MainScene_LoadingMenu : MonoBehaviour
{
    
    private void Awake ()
    {
        DontShowNeextTime DontShowNextTime_ = GetComponentInChildren<DontShowNeextTime>();
        if (DontShowNextTime_.CheckDontShow())
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public void Deactivate ()
    {
        this.gameObject.SetActive(false);
    }

    public void Activate ()
    {
        this .gameObject.SetActive(true);   
    }
}
