using DG.Tweening;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    public GameObject CurrentWinnings;
    public GameObject TotalWinnings;
    public GameObject CurrentWiningsTextHolder;
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
}
