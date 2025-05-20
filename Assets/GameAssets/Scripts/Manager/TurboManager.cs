using DG.Tweening;
using TMPro;
using UnityEngine;

public class TurboManager : MonoBehaviour
{
    public bool IsNormalMode = true;
    public bool IsTurboSpin_ = false;
    public bool IsSuperTurboSpin_ = false;
    public GameObject TurboUI;
    public TMP_Text TurboSpinText;
    public TurboSpin TurboSpin;
    private Tween mytween;
    public AutoSpinFx[] AutoSpinFx_;
    public void EnableTurbospin ()
    {
        mytween.Kill();
        ActivateTurboUI();
        TurboUI.GetComponent<CanvasGroup>().alpha = 0;
        if(IsTurboSpin_)
        {
            TurboSpinText.text = LanguageMan.instance.RequestForText("L_100");
        }
        else if (IsSuperTurboSpin_)
        {
            TurboSpinText.text = LanguageMan.instance.RequestForText("L_101"); ;
        }
        else if (IsNormalMode)
        {
            TurboSpinText.text = LanguageMan.instance.RequestForText("L_102"); ;
        }
        mytween = TurboUI.GetComponent<CanvasGroup>().DOFade(1 , 1f)
            .OnComplete(() =>
            {
                Invoke(nameof(DisableTurboUI) , 2f);
            });
    }

    public void DisableTurbospin ()
    {
        ActivateTurboUI();
        TurboUI.GetComponent<CanvasGroup>().alpha = 0;
        if (IsTurboSpin_)
        {
            TurboSpinText.text = "Turbo Spin Disabled";
        }
        else if (IsSuperTurboSpin_)
        {
            TurboSpinText.text = "Super Turbo Spin Disabled";
        }
        TurboUI.GetComponent<CanvasGroup>().DOFade(1 , .5f)
          .OnComplete(() =>
          {
              Invoke(nameof(DisableTurboUI) , 2f);
          });
    }

    public void ActivateTurboUI ()
    {
        TurboUI.SetActive(true);
        IsNormalMode = TurboSpin.IsNormalSpin;
        IsTurboSpin_ = TurboSpin.IsTurboSpin;
        IsSuperTurboSpin_ = TurboSpin.IsSuperTurboSpin;
    }

    public void DisableTurboUI ()
    {
        TurboUI.GetComponent<CanvasGroup>().DOFade(0 , .5f)
         .OnComplete(() =>
         {
             TurboUI.SetActive(false);
         });
        
    }

    private void Update ()
    {
        if (CommandCentre.Instance)
        {
            if (IsNormalMode || IsTurboSpin_)
            {
                CommandCentre.Instance.GridManager_.moveDuration = 0.25f;
                CommandCentre.Instance.DemoManager_.DemoGridManager_.moveDuration = 0.25f;
                foreach(AutoSpinFx autoSpinFx in AutoSpinFx_)
                {
                    autoSpinFx.degreesPerSecond = 360;
                }
            }
            else if (IsSuperTurboSpin_)
            {
                CommandCentre.Instance.GridManager_.moveDuration = 0.1f;
                CommandCentre.Instance.DemoManager_.DemoGridManager_.moveDuration = 0.1f;

                foreach (AutoSpinFx autoSpinFx in AutoSpinFx_)
                {
                    autoSpinFx.degreesPerSecond = 1440;
                }
            }
        }
    }
}
