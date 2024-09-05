using UnityEngine;

public class FreeGameManager : MonoBehaviour
{
    public ComboUI Combos;
    public BannerController BannerController_;
    public FreeGameIntro FreeGameIntro_;
    public bool IsFreeGame = false;

    public void ActivateFreeGameIntro ()
    {
        FreeGameIntro_.Activate();
        ToggleFreeGame(true);
        ToggleComboBanner(true);
    }

    public void DeactivateFreeGameIntro ()
    {
        FreeGameIntro_.Deactivate();
    }

    public void ToggleFreeGame(bool toggle )
    {
        IsFreeGame = toggle ;
        CommandCentre.Instance.MainMenuController_.IsFreeGame = IsFreeGame;
    }

    public void ToggleComboBanner(bool toggle )
    {
        if ( !toggle)
        {
            Combos.DeactivateNormalCombo();
            BannerController_.DeactivateNormalBanner();

            Combos.ActivateFreeGameCombo();
            BannerController_.ActivateFreeGameBanner();
        }
        else
        {
            Combos.DeactivateFreeGameCombo();
            BannerController_.DeactivateFreeGameBanner();

            Combos.ActivateNormalCombo();
            BannerController_.ActivateNormalBanner();
            
        }
    }


    public void ActivateFreeGame ()
    {
        ActivateFreeGameIntro();
    }

    public void DeactivateFreeGame ()
    {
        ToggleFreeGame(false);
        ToggleComboBanner(false);
        DeactivateFreeGameIntro();
    }

}
