using DG.Tweening;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    public GameObject CurrentWinnings;
    public GameObject TotalWinnings;
    public GameObject CurrentWiningsTextHolder;
    public GameObject FreeGameWinUi;
    public bool IsShowingTotalWinings;

    public void ActivateCurrentWinings ()
    {
        CurrentWinnings.GetComponent<CanvasGroup>().alpha = 1.0f;
        CurrentWinnings.SetActive (true);
        CurrentWiningsTextHolder.transform.DOPunchScale(new Vector3(.1f , .1f , .1f) , .5f , 8 , 1)
            .OnComplete(() => {
              CurrentWiningsTextHolder.transform.localScale = Vector3.one;
          });
    }

    public void DeactivateCurrentWinings ()
    {
        CurrentWinnings.GetComponent<CanvasGroup>().DOFade(0 , .5f).OnComplete(() =>
        {
            CurrentWinnings.SetActive(false);
        });
    }

    public void ActivateTotalWinnings ()
    {
        IsShowingTotalWinings = true;
        TotalWinnings.GetComponent<CanvasGroup>().alpha = 1.0f;
        TotalWinnings.SetActive (true);
    }

    public void DeactivateTotalWinnings ()
    {
        TotalWinnings.GetComponent<CanvasGroup>().DOFade(0 , .5f).OnComplete(() =>
        {
            IsShowingTotalWinings= false;
            TotalWinnings.SetActive(false);
        });
    }

    public void ShowFreeGameWinUi_win ()
    {
        FreeGameWinUi.SetActive(true);
        FreeGameWinUi.GetComponent<CanvasGroup>().DOFade(1 , .5f)
            .OnComplete(() =>
            {
                Invoke(nameof(ActivteTheText) , 2f);
                Invoke(nameof(HideFreeGameUi_win) , 8f);
            });
    }

    void ActivteTheText ()
    {
        FreeGameWinUi.GetComponent<FreeGameWinUI>().canUpdateWinnings = true;
    }

    public void HideFreeGameUi_win ()
    {
        FreeGameWinUi.GetComponent<CanvasGroup>().DOFade(0 , .5f)
            .OnComplete(() =>
            {
                FreeGameWinUi.GetComponent<FreeGameWinUI>().canUpdateWinnings = false;
                FreeGameWinUi.SetActive (false);
                if (CommandCentre.Instance.DemoManager_.IsDemo)
                {
                    CommandCentre.Instance.MainMenuController_.EnableWinMoreMenu();
                    CommandCentre.Instance.FreeGameManager_.winMoreMenu_.DeactivateDemoBtn();
                    CommandCentre.Instance.FreeGameManager_.winMoreMenu_.DeactivateSuggestion_1();
                    CommandCentre.Instance.DemoManager_.IsDemo = false;
                    CommandCentre.Instance.MainMenuController_.GameplayMenu.GetComponent<GamePlayMenuController>().ShowNormalGamePlayMenu();
                    CommandCentre.Instance.MainMenuController_.GameplayMenu.GetComponent<GamePlayMenuController>().HideDemoGamePlayMenu();
                }
            });
    }
}
