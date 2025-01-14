using UnityEngine;
using UnityEngine.UI;

public class SmoothScrolling : MonoBehaviour
{
    [Header("Scrolling Settings")]
    [Tooltip("Speed of the scrolling")]
    public float scrollSpeed = 0.1f;

    [Tooltip("Smoothness of the scrolling")]
    public float smoothTime = 0.1f;

    private ScrollRect scrollRect;
    private float targetScrollPosition;
    private float currentVelocity;
    public bool isHandHeldDevice = false;
    [SerializeField]private bool isScrolling = false;
    [SerializeField]private bool isDragging = false;
    Vector3 previousMousePosition;

    void Start ()
    {
        scrollRect = GetComponent<ScrollRect>();

        // Initialize the target position based on the current scroll position
        targetScrollPosition = scrollRect.verticalNormalizedPosition;
    }

    void Update ()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        Vector3 currentMousePosition = Input.mousePosition;
        

        if (Input.GetMouseButton(0) && scrollInput == 0)
        {
            isDragging = true;
            isScrolling = false;
            isHandHeldDevice = true;
        }
        else if(scrollInput != 0f) // When mouse button is released
        {
            isDragging = false;
            isScrolling = true;
            isHandHeldDevice = false;
        }
        else
        {
            previousMousePosition = currentMousePosition;
        }

        // If not dragging, handle scrolling via mouse wheel
        if (!isDragging)
        {
            targetScrollPosition += scrollInput * scrollSpeed;
            targetScrollPosition = Mathf.Clamp01(targetScrollPosition); // Clamp to 0-1 range
            scrollRect.enabled = false; // Disable ScrollRect for smooth scrolling

        }
        else
        {
            Vector3 mouseDelta = currentMousePosition - previousMousePosition;
            previousMousePosition = currentMousePosition;
            float normalizedMouseDeltaY = mouseDelta.y / Screen.height;

            // Update scroll position based on the drag amount
            targetScrollPosition -= normalizedMouseDeltaY * scrollSpeed;
            targetScrollPosition = Mathf.Clamp01(targetScrollPosition);
        }
        // Smooth scrolling
        scrollRect.verticalNormalizedPosition = Mathf.SmoothDamp(
            scrollRect.verticalNormalizedPosition ,
            targetScrollPosition ,
            ref currentVelocity ,
            smoothTime
        );

        scrollRect.verticalScrollbar.value = Mathf.SmoothDamp(
            scrollRect.verticalScrollbar.value ,
            targetScrollPosition ,
            ref currentVelocity ,
            smoothTime
            );
    }

}
