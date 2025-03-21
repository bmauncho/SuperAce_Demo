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
    public bool IsSpinInit= false;

    public int FreeSpinCounter = 10;
    public TMP_Text FreeSpinsAmount;
    public WinMoreMenu winMoreMenu_;
    public float winAmount = 0;

    public void ActivateFreeGameIntro ()
    {
        FreeGameIntro_.Activate();
        ToggleFreeGame(true);
        ToggleComboBanner(true);
        //resetFreeSpins();
        Invoke(nameof(ShowFreeComboUi) , 1f);
    }
    [ContextMenu("Deactivate FreeGameIntro")]
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

    void ShowFreeComboUi ()
    {
        showComboUi();
    }

    public void showComboUi (bool freeGame = true)
    {
        if (freeGame)
        {
            Combos.ShowFreeGameCombo();
        }
        else
        {
            Combos.ShowNormalGameCombo();
        }
    }

    [ContextMenu("Activate FreeGameIntro")]
    public void ActivateFreeGame ()
    {
        ActivateFreeGameIntro();
    }

    [ContextMenu("Deactivate FreeGame")]
    public void DeactivateFreeGame ()
    {
        ToggleFreeGame(false);
        ToggleComboBanner(false);
        //resetFreeSpins();
        CommandCentre.Instance.SoundManager_.PlayAmbientSound("FunkCasino");
        float amount = winAmount;
        Debug.Log(amount);
        if (amount > 0)
        {
            CommandCentre.Instance.PayOutManager_.WinUI_.ShowFreeGameWinUi_win();
        }
        FreeGameUi.SetActive(false);
        IsSpinInit = false;
    }


    public void DecreaseFreespins ()
    {
        if ( IsFreeGame )
        {
            FreeSpinCounter--;
            FreeSpinsAmount.text = FreeSpinCounter.ToString();

        }
    }

    public void resetFreeSpins ()
    {
        FreeSpinCounter = 10;
        FreeSpinsAmount.text = FreeSpinCounter.ToString();
    }

    public void increaseSpins ()
    {
        Debug.Log("Adding free spins");
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            FreeSpinCounter = 10;
            FreeSpinsAmount.text = FreeSpinCounter.ToString();
        }
        else
        {
            FreeSpinCounter += Mathf.FloorToInt(APIManager.instance.GameDataAPI_.FreeSpins);
            FreeSpinsAmount.text = FreeSpinCounter.ToString();
        }  
    }

}
