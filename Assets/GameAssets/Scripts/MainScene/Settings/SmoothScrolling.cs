using UnityEngine;
using UnityEngine.UI;

public class SmoothScrolling : MonoBehaviour
{
    [Tooltip("Smoothness of the scrolling")]
    public float smoothTime = 0.1f;

    private ScrollRect scrollRect;
    public RectTransform slidingArea;
    public RectTransform handle;

    private Vector2 velocity;

    void Start ()
    {
        // Get the ScrollRect component attached to this GameObject
        scrollRect = GetComponent<ScrollRect>();
    }

    void Update ()
    {
        // Get the height of the SlidingArea
        float slidingAreaHeight = slidingArea.rect.height;

        // Calculate the target Y position based on verticalNormalizedPosition
        float targetY = Mathf.Lerp(-slidingAreaHeight / 2 , slidingAreaHeight / 2 , scrollRect.verticalNormalizedPosition);

        // Smoothly move the Handle's position towards the target position
        Vector2 targetPosition = new Vector2(handle.anchoredPosition.x , targetY);
        handle.anchoredPosition = Vector2.SmoothDamp(handle.anchoredPosition , targetPosition , ref velocity , smoothTime);
    }
}
