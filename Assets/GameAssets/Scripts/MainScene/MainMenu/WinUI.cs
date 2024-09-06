using DG.Tweening;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    public GameObject CurrentWinnings;
    public GameObject TotalWinnings;

    public void ActivateCurrentWinings ()
    {
        CurrentWinnings.GetComponent<CanvasGroup>().alpha = 1.0f;
        CurrentWinnings.SetActive (true);
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
        TotalWinnings.GetComponent<CanvasGroup>().alpha = 1.0f;
        TotalWinnings.SetActive (true);
    }

    public void DeactivateTotalWinnings ()
    {
        TotalWinnings.GetComponent<CanvasGroup>().DOFade(0 , .5f).OnComplete(() =>
        {
            TotalWinnings.SetActive(false);
        });
    }
}
