using DG.Tweening;
using UnityEngine;

public class LoadingMenu_Content : MonoBehaviour
{
    public GameObject LeftBtn;
    public GameObject RightBtn;
    public RectTransform ScrollContent;
    Vector2 TargetPos;
    float RightPos = 224f;
    float LeftPos = -224f;
    bool MovedLeft;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckPos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CheckPos ()
    {
        if (MovedLeft)
        {
            DeactivateRightBtn();
            ActivateLeftBtn();

        }
        else
        {
            ActivateRightBtn();
            DeactivateLeftBtn();
        }
    }
    public void ScrollTheContent (float target)
    {
        TargetPos = new Vector2(target,ScrollContent.anchoredPosition.y);
        ScrollContent.DOAnchorPos(TargetPos , .1f)
            .OnComplete(() =>
            {
               CheckPos ();
            });
    }

    public void Deactivate ()
    {
        this.gameObject.SetActive(false);
    }

    public void CanBounce ()
    {
        ScrollContent.DOPunchAnchorPos(new Vector2(.2f , 0) , .25f , 0 , .15f , false);
    }

    public void ActivateLeftBtn ()
    {
        LeftBtn.SetActive(true);
    }

    public void DeactivateLeftBtn ()
    {
        LeftBtn.SetActive(false);
    }

    public void ActivateRightBtn ()
    {
        RightBtn.SetActive(true);
    }
    public void DeactivateRightBtn ()
    {
        RightBtn.SetActive(false);
    }

    public void MoveLeft ()
    {
        MovedLeft = false;
        ScrollTheContent(RightPos);
    }
    public void MoveRight ()
    {
        MovedLeft = true;
        ScrollTheContent(LeftPos);
    }
}
