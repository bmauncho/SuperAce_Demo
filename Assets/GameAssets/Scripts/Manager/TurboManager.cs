using DG.Tweening;
using TMPro;
using UnityEngine;

public class TurboManager : MonoBehaviour
{
    public bool TurboSpin_ = false;
    public GameObject TurboUI;
    public TMP_Text TurboSpinText;


    public void EnableTurbospin ()
    {
        ActivateTurboUI();
        TurboUI.GetComponent<CanvasGroup>().alpha = 0;
        TurboSpin_ = true;
        TurboSpinText.text = "Turbo Spin Enabled";
        TurboUI.GetComponent<CanvasGroup>().DOFade(1, 1f)
            .OnComplete(() =>
            {
                Invoke(nameof(DisableTurboUI) , 2f);
            });
    }

    public void DisableTurbospin ()
    {
        ActivateTurboUI();
        TurboUI.GetComponent<CanvasGroup>().alpha = 0;
        TurboSpin_ = false;
        TurboSpinText.text = "Turbo Spin Disabled";
        TurboUI.GetComponent<CanvasGroup>().DOFade(1 , .5f)
          .OnComplete(() =>
          {
              Invoke(nameof(DisableTurboUI) , 2f);
          });
    }

    public void ActivateTurboUI ()
    {

        TurboUI.SetActive(true);
    }

    public void DisableTurboUI ()
    {
        TurboUI.GetComponent<CanvasGroup>().DOFade(0 , .5f)
         .OnComplete(() =>
         {
             TurboUI.SetActive(false);
         });
        
    }
}
