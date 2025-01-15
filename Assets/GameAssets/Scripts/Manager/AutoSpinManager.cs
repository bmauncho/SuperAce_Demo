using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoSpinManager : MonoBehaviour
{
    public bool IsAutoSpin = false;
    public GameObject AutoSpinUI;
    public TMP_Text AutoSpinText;
    public AutoSpinSettings AutoSpinSettings_;
    public int AutoSpinIndex_;
    public AutoSpin Autospin;
    public Toggle AutospinToggle;
    public Toggle DemoAutospinToggle;


    public void EnableAutoSpin ()
    {
        ResetAutoSpins ();
        ActivateAutoSpinUI();

        IsAutoSpin = true;
        AutoSpinUI.GetComponent<CanvasGroup>().alpha = 0;
        AutoSpinText.text = "Auto Spin Enabled";
        AutoSpinUI.GetComponent<CanvasGroup>().DOFade(1 , 1f)
            .OnComplete(() =>
            {
                Invoke(nameof(DisableAutoSpinUI) , 2f);
            });
        CommandCentre.Instance.MainMenuController_.Spin();
    }

    public void DisableAutoSpin ()
    {
        IsAutoSpin = false;
        ActivateAutoSpinUI();
        AutoSpinUI.GetComponent<CanvasGroup>().alpha = 0;
        AutoSpinText.text = "Auto Spin Disabled";
       AutoSpinUI.GetComponent<CanvasGroup>().DOFade(1 , .5f)
          .OnComplete(() =>
          {
              AutospinToggle.isOn = false;
              if (CommandCentre.Instance.DemoManager_.IsDemo)
              {
                  DemoAutospinToggle.isOn = false;
              }
              Invoke(nameof(DisableAutoSpinUI) , 2f);
          });
        
    }

    public void ActivateAutoSpinUI ()
    {
        AutoSpinUI.SetActive(true);
    }

    public void DisableAutoSpinUI ()
    {
        AutoSpinUI.GetComponent<CanvasGroup>().DOFade(0 , .5f)
         .OnComplete(() =>
         {
             AutoSpinUI.SetActive(false);
         });
    }

    public void DecreaseAutoSpins ()
    {
        AutoSpinIndex_--;
        if (AutoSpinIndex_ <= 0)
        {
            AutoSpinIndex_ = 0;
            DisableAutoSpin();
        }
    }

    public void ResetAutoSpins ()
    {
        AutoSpinIndex_ = 10;
    }
}
