using TMPro;
using UnityEngine;

public class FreeGameWinUI : MonoBehaviour
{
    public float targetAmount; // Change to 10 or any target number
    public float currentAmount; // Change to 10 or any target number
    public TMP_Text CurrentWinAmount; // TextMeshPro component to show the value
    public float Duration = 2f; // Duration of the number counting animation
    private float startTime;
    public bool canUpdateWinnings = false; // Control when to start updating
    public GameObject TheWinAmountText;
    private float elapsedTime;  // To track the elapsed time

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        Refresh();
        startTime = Time.time;
        targetAmount = GetTotalAmount();
    }

    private void OnEnable ()
    {
        Refresh();
        startTime = Time.time;
        targetAmount = GetTotalAmount();
    }

    private void Update ()
    {
        if(canUpdateWinnings)
        {
            TheWinAmountText.SetActive(true);
            UpdateFreeGameTotalWinnings();
        }
        else
        {
            TheWinAmountText.SetActive(false);
        }
    }

    public void UpdateFreeGameTotalWinnings ()
    {
        // Increment elapsed time based on real time passed
        elapsedTime += Time.deltaTime;
        // Calculate the current value using Mathf.Lerp
        float currentValue = Mathf.Lerp(0f , targetAmount , elapsedTime / Duration);
        // Update the text with the formatted value
        CurrentWinAmount.text = currentValue.ToString("N2");

        // Reset or cap elapsed time once the duration is reached
        if (elapsedTime >= Duration)
        {
            elapsedTime = Duration;  // Ensure it doesn't go beyond the duration
            CurrentWinAmount.text = targetAmount.ToString("N2");  // Set to the final target value
        }
    }


    float GetTotalAmount ()
    {
        float theAmount = 0f;
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            theAmount = CommandCentre.Instance.CashManager_.CurrentWinings;

        }
        else
        {
            theAmount = CommandCentre.Instance.CashManager_.CurrentWinings;
        }
        return theAmount;
    }

    void Refresh ()
    {
        targetAmount = 0;
        currentAmount = 0;
    }
}
