using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingMenu_Content : MonoBehaviour
{
    public GameObject LeftBtn;
    public GameObject RightBtn;
    public ScrollRect scrollRect;
    public RectTransform scrollRectTransform;
    public GameObject[] Content;
    public int TargetIndex = 0;
    public RectTransform targetChild;// The child you want to scroll to
    [SerializeField]bool isAutoScrolling = false;
    [SerializeField]bool MovedLeft;
    [SerializeField]bool AutoScrollMovedLeft;
    [SerializeField]bool isUserInteraction = false;
    [SerializeField]float timer;
    float waitTime = 5f;

    private Vector2 startDragPosition;
    private Vector2 currentDragPosition;
    [SerializeField] private bool isDragging = false;
    [SerializeField] private bool isDraggingRight = false;
    [SerializeField] private bool dragStartedInScrollRect = false;

    [Header("Drag Settings")]
    public float minimumDragDistance = 50f; // Minimum pixels to register as a drag
    Tween scrollTween;
    private void OnEnable ()
    {
        
    }

    private void Update ()
    {
        Drag();

        if (!isDragging)
        {
            Autoscroll();
        }
    }


    public void Autoscroll ()
    {
        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            if (AutoScrollMovedLeft)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }
            timer = 0f;
        }
    }

    public void Drag ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStartedInScrollRect = RectTransformUtility.RectangleContainsScreenPoint(scrollRectTransform , Input.mousePosition);

            if (dragStartedInScrollRect)
            {
                startDragPosition = Input.mousePosition;
                isDragging = false;
            }
        }

        if (dragStartedInScrollRect && Input.GetMouseButton(0))
        {
            currentDragPosition = Input.mousePosition;
            Vector2 dragDelta = currentDragPosition - startDragPosition;

            if (!isDragging && Mathf.Abs(dragDelta.x) >= minimumDragDistance)
            {
                isDragging = true;
            }

            if (isDragging)
            {
                DragLogic(currentDragPosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                FinalizeDrag();
            }

            isDragging = false;
            dragStartedInScrollRect = false;
        }
    }

    private void DragLogic ( Vector2 currentMousePosition )
    {
        currentDragPosition = currentMousePosition;
        Vector2 dragDelta = currentDragPosition - startDragPosition;

        if (Mathf.Abs(dragDelta.x) > minimumDragDistance)
        {
            if (dragDelta.x > 0)
            {
                isDraggingRight = true;
            }
            else
            {
                isDraggingRight = false;
            }

            // Reset drag start so next drag is measured from current position
            startDragPosition = currentDragPosition;
        }
    }

    private void FinalizeDrag ()
    {
        // Optional: add final drag direction resolution or cleanup here
        if (isDraggingRight)
        {
            MoveLeft();
        }
        else
        {
            MoveRight();
        }
    }
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

        scrollTween = DOTween.To(() => scrollRect.horizontalNormalizedPosition ,
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
            if (TargetIndex == 0)
            {
                AutoScrollMovedLeft = true;
            }
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
            if (TargetIndex == Content.Length - 1)
            {
                AutoScrollMovedLeft = false;
            }
            ScrollToTarget();
        }
    }

}