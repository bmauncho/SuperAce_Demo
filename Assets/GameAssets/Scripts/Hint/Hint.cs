using DG.Tweening;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] RectTransform m_RectTransform;
    public int slideCounter = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate ()
    {
        this.gameObject.SetActive(true);
        ShowHint();
    }

    public void Deactivate ()
    {
        this.gameObject.SetActive(false);
    }

    [ContextMenu("ShowHint")]
    public void ShowHint ()
    {
        Debug.Log("Show Hint");
        m_RectTransform.DOAnchorPosY(0 , .75f)
            .OnComplete(() =>
        {
            SlideHint(2f);
        });
    }

    public void SlideHint (float Duration)
    {
        m_RectTransform.DOAnchorPosX(-500 , Duration)
            .SetDelay(2f)
            .OnComplete(() =>
        {
            refreshHint();
        });
    }

    public void refreshHint ()
    {
        slideCounter++;
        m_RectTransform.anchoredPosition = new Vector2(500 , 0);
        if(slideCounter > 1)
        {
            slideCounter = 0;
            Deactivate();
            resetHint();
            CommandCentre.Instance.HintManager_.whichHint++;
            CommandCentre.Instance.HintManager_.CanStartTimer = true;
            CommandCentre.Instance.HintManager_.timer = 0;
            
        }
        else
        {
            SlideHint(4f);
        }
    }
    public void resetHint ()
    {
        m_RectTransform.anchoredPosition = new Vector2(120 , 50);
    }
}
