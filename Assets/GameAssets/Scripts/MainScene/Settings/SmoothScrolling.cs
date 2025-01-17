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

    void Start ()
    {
        // Get the ScrollRect component attached to this GameObject
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 1;
    }

    void Update ()
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

        if(isScrolling)
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
}
