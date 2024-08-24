using DG.Tweening;
using UnityEngine;

public class MainScene_LoadingMenu : MonoBehaviour
{
    private void Awake ()
    {
        DontShowNeextTime DontShowNextTime_ = GetComponentInChildren<DontShowNeextTime>();
        if (DontShowNextTime_.CheckDontShow())
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public void ContinueToMainGame ()
    {
        GetComponent<CanvasGroup>().DOFade(0 , .5f);
        transform.DOScale(1.3f , .5f).OnComplete(() =>
        {
            Deactivate ();
        });
    }

    public void Deactivate ()
    {
        this.gameObject.SetActive(false);
    }

    public void Activate ()
    {
        this .gameObject.SetActive(true);   
    }
}
