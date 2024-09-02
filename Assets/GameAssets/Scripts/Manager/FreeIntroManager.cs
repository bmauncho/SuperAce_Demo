using UnityEngine;

public class FreeIntroManager : MonoBehaviour
{

    public GameObject NormalBanner;
    public GameObject FreeIntroBanner;
    public FreeGameIntro FreeGameIntro_;
    public bool IsFreeGame = false;

    public void Activate ()
    {
        ToggleFreeGame(true);
        ToggleBanner(true);
        FreeGameIntro_.Activate();
    }

    public void Deactivate ()
    {
        FreeGameIntro_.Deactivate();
        ToggleFreeGame(false);
        ToggleBanner(false);
    }

    public void ToggleFreeGame(bool toggle )
    {
        IsFreeGame = toggle ;
        CommandCentre.Instance.MainMenuController_.IsFreeGame = IsFreeGame;
    }

    public void ToggleBanner(bool toggle )
    {
        if ( !toggle)
        {
            NormalBanner.SetActive(true);
            FreeIntroBanner.SetActive(false);
        }
        else
        {
            NormalBanner.SetActive(false);
            FreeIntroBanner.SetActive(true);
        }
    }
}
