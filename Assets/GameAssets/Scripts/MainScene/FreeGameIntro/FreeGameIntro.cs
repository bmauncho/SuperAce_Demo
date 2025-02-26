using DG.Tweening;
using UnityEngine;

public class FreeGameIntro : MonoBehaviour
{
    public void Activate ()
    {
        this.gameObject.SetActive (true);
        GetComponent<CanvasGroup>().alpha = 1.0f;
    }

    void FadeOut ()
    {
        GetComponent<CanvasGroup>().DOFade(0 , .5f);
        transform.DOScale(1.3f , 0.5f).OnComplete(() =>
        {
            transform.DOScale(1 , 0.25f).OnComplete(() =>
            {
                CommandCentre.Instance.FreeGameManager_.showComboUi(true);
                DOVirtual.DelayedCall(0.3f , () => // Adjust delay time as needed
                {
                    this.gameObject.SetActive(false);
                });
            });
        });

    }

    public void Deactivate ()
    {
        FadeOut();
    }
}
