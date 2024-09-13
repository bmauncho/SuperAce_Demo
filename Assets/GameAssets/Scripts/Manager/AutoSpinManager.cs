using DG.Tweening;
using TMPro;
using UnityEngine;

public class AutoSpinManager : MonoBehaviour
{
    public bool IsAutoSpin = false;
    public GameObject AutoSpinUI;
    public TMP_Text AutoSpinText;
    public AutoSpinSettings AutoSpinSettings_;
    public int AutoSpinIndex_;

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
        }
    }

    public void ResetAutoSpins ()
    {
        AutoSpinIndex_ = 10;
    }
}
