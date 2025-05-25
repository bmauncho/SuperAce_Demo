using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SmoothScrolling : MonoBehaviour
{
    [Tooltip("Smoothness of the scrolling")]
    public float smoothTime = 0.1f;
    private float currentVelocity = 0f; // For smooth scrolling
    private float targetScrollPosition;
    private ScrollRect scrollRect;
    public RectTransform slidingArea;
    public RectTransform handle;

    private Vector2 velocity;

    public bool isScrolling =false;

    public RectTransform scrollRectTransform;
    private Vector2 startDragPosition;
    private Vector2 currentDragPosition;
    [SerializeField] private bool isDragging = false;
    [SerializeField] private bool isDraggingUp = false;
    [SerializeField] private bool dragStartedInScrollRect = false;

    [Header("Drag Settings")]
    public float minimumDragDistance = 50f; // Minimum pixels to register as a drag
    Tween scrollTween;
    Vector2 dragDist;

    void Start ()
    {
        // Get the ScrollRect component attached to this GameObject
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 1;
    }

    void Update ()
    {
        scroll();
        Drag();

    }

    public void scroll ()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            isScrolling = true;
            scrollRect.enabled = false;
            // Adjust the target scroll position based on input
            targetScrollPosition += scrollInput * smoothTime;

            // Clamp the target position between 0 and 1
            targetScrollPosition = Mathf.Clamp01(targetScrollPosition);
        }

        if (Input.GetMouseButtonDown(0))
        {
            isScrolling = false;
            scrollRect.enabled = true;
        }

        if (isScrolling)
        {
            scrollRect.verticalNormalizedPosition = Mathf.SmoothDamp(
            scrollRect.verticalNormalizedPosition ,
            targetScrollPosition ,
            ref currentVelocity ,
            smoothTime);
        }
        // Get the height of the SlidingArea
        float slidingAreaHeight = slidingArea.rect.height;

        // Calculate the target Y position based on verticalNormalizedPosition
        float targetY = Mathf.Lerp(-slidingAreaHeight / 2 , slidingAreaHeight / 2 , scrollRect.verticalNormalizedPosition);

        // Smoothly move the Handle's position towards the target position
        Vector2 targetPosition = new Vector2(handle.anchoredPosition.x , targetY);
        handle.anchoredPosition = Vector2.SmoothDamp(handle.anchoredPosition , targetPosition , ref velocity , smoothTime);
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
            dragDist = dragDelta;
            if (!isDragging && Mathf.Abs(dragDelta.y) >= minimumDragDistance)
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
                FinalizeDrag(dragDist);
            }

            isDragging = false;
            dragStartedInScrollRect = false;
        }
    }

    private void DragLogic ( Vector2 currentMousePosition )
    {
        Vector2 dragDelta = currentMousePosition - startDragPosition;

        if (Mathf.Abs(dragDelta.y) > minimumDragDistance)
        {
            isDraggingUp = dragDelta.y > 0;
            startDragPosition = currentMousePosition;
        }
    }


    private void FinalizeDrag ( Vector2 dragDelta )
    {
        float scrollDelta = dragDelta.y / slidingArea.rect.height;

        float currentNormalizedPosition = scrollRect.verticalNormalizedPosition;
        float targetPosition = Mathf.Clamp01(currentNormalizedPosition - scrollDelta);

        DOTween.To(
            () => scrollRect.verticalNormalizedPosition ,
            x => scrollRect.verticalNormalizedPosition = x ,
            targetPosition ,
            0.3f // duration in seconds
        ).SetEase(Ease.OutCubic); // optional: smooth easing
    }
}
