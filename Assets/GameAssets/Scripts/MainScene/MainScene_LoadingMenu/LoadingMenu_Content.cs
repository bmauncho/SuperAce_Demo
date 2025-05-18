using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingMenu_Content : MonoBehaviour
{
    public GameObject LeftBtn;
    public GameObject RightBtn;
    public ScrollRect scrollRect;
    [SerializeField]bool MovedLeft;
    public GameObject[] Content;
    public int TargetIndex = 0;
    public RectTransform targetChild;          // The child you want to scroll to

    public void ScrollToTarget ()
    {
        Canvas.ForceUpdateCanvases();

        RectTransform content = scrollRect.content;
        float contentWidth = content.rect.width;
        float viewportWidth = scrollRect.viewport.rect.width;

        // Get the child's anchored position and adjust for pivot
        float targetPosX = targetChild.anchoredPosition.x;
        float pivotOffset = targetChild.rect.width * ( 0.5f - targetChild.pivot.x ); // compensate for pivot
        float correctedPosX = targetPosX + pivotOffset;

        // Center the target and apply offset (e.g., scroll 54 units earlier)
        float centerOffset = correctedPosX - ( viewportWidth / 2f );

        float scrollableWidth = contentWidth - viewportWidth;
        float normalizedX = centerOffset / scrollableWidth;
        normalizedX = Mathf.Clamp01(normalizedX);

        DOTween.To(() => scrollRect.horizontalNormalizedPosition ,
                   x => scrollRect.horizontalNormalizedPosition = x ,
                   normalizedX ,
                   0.3f);
    }


    public void Deactivate ()
    {
        this.gameObject.SetActive(false);
    }
    public void MoveLeft ()
    {
        if (TargetIndex > 0)
        {
            TargetIndex--;
            targetChild = Content [TargetIndex].GetComponent<RectTransform>();
            MovedLeft = true;
            ScrollToTarget();
        }
    }

    public void MoveRight ()
    {
        if (TargetIndex < Content.Length - 1)
        {
            TargetIndex++;
            targetChild = Content [TargetIndex].GetComponent<RectTransform>();
            MovedLeft = false;
            ScrollToTarget();
        }
    }

}
