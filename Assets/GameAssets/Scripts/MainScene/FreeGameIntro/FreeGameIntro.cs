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
        transform.DOScale(1.3f , .5f).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

    public void Deactivate ()
    {
        FadeOut();
    }
}
