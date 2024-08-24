using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public Image Loadingbar;
    public float load_time;
    public TMP_Text LoadingText;
    public TMP_Text ContinueText;
    public RectTransform ContinueBtn;


    void Start ()
    {
        loadUp();
    }

    void loadUp ()
    {
        Tween tween = DOTween.To(() => Loadingbar.fillAmount , x => Loadingbar.fillAmount = x , 1f , load_time)
            .OnUpdate(() => LoadingText.text = $"Loading...{Mathf.FloorToInt(loadingPercentage())}%")
             .OnComplete(() =>
             {
                 ContinueBtn.gameObject.SetActive( true );
                 ContinueBtn.DOSizeDelta(new Vector2(150 , 40) , .25f);
                 Invoke(nameof(ActivateContinueText) , .15f);
                 LoadingText.gameObject.SetActive( false );
                 Invoke(nameof(Deactivate) , .5f);
             });
    }

    public float loadingPercentage ()
    {
        return Loadingbar.fillAmount * 100f;
    }

    void ActivateContinueText ()
    {
        ContinueText.gameObject.SetActive(true);
    }

    public void Deactivate ()
    {
        this.gameObject.SetActive( false );
    }
}
