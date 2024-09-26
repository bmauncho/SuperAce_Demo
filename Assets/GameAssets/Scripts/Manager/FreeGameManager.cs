using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FreeGameManager : MonoBehaviour
{
    public ComboUI Combos;
    public BannerController BannerController_;
    public FreeGameIntro FreeGameIntro_;
    public GameObject FreeGameUi;
    public bool IsFreeGame = false;

    public int FreeSpinCounter = 10;
    public TMP_Text FreeSpinsAmount;
    public WinMoreMenu winMoreMenu_;

    public void ActivateFreeGameIntro ()
    {
        FreeGameIntro_.Activate();
        ToggleFreeGame(true);
        ToggleComboBanner(true);
    }

    public void DeactivateFreeGameIntro ()
    {
        FreeGameIntro_.Deactivate();
        FreeGameUi.SetActive(true);
    }

    public void ToggleFreeGame(bool toggle )
    {
        IsFreeGame = toggle ;
        CommandCentre.Instance.MainMenuController_.IsFreeGame = IsFreeGame;
    }

    public void ToggleComboBanner(bool toggle )
    {
        if ( toggle)
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
        resetFreeSpins();
        CommandCentre.Instance.WinLoseManager_.enableSpin = true;
        CommandCentre.Instance.SoundManager_.PlayAmbientSound("FunkCasino");
        float amount = float.Parse(CommandCentre.Instance.CashManager_.WinCashAmountText [1].text.ToString());
        if (amount > 0)
        {
            CommandCentre.Instance.PayOutManager_.WinUI_.ShowFreeGameWinUi_win();
        }
        FreeGameUi.SetActive(false);
    }


    public void DecreaseFreespins ()
    {
        if ( IsFreeGame )
        {
            FreeSpinCounter--;
            FreeSpinsAmount.text = FreeSpinCounter.ToString();

            if( FreeSpinCounter <= 0) 
            {
                CommandCentre.Instance.WinLoseManager_.enableSpin = false;
            }
        }
    }

    public void resetFreeSpins ()
    {
        FreeSpinCounter = 10;
        FreeSpinsAmount.text = FreeSpinCounter.ToString();
    }
}
