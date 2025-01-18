using System.Collections;
using TMPro;
using UnityEngine;

public class BalanceController : MonoBehaviour
{
    public int CurrentBalance = 0;
    public TMP_Text Balance;

    public float holdDuration = 2.0f; // Duration in seconds for holding the toggle
    public float holdTime = 0f;
    public bool isHolding = false;
    public bool isPointerDown = false;

    public float timerDuration = 10f;  // Set the timer duration (10 seconds)
    private float timer;
    bool ActionPerformed = false;
    bool IsDecreaseButton = false;
    bool IsIncreaseButton = false;

    private void Start ()
    {
        timer = timerDuration;
    }
    public void IncreaseBalance ()
    {
       
        CurrentBalance+=10;

        Balance.text = $"{CurrentBalance}";
    }

    public void DecreaseBalance ()
    {
        IsDecreaseButton =true;
        IsIncreaseButton = false;
        CurrentBalance-=10;
        if (CurrentBalance <= 1) { CurrentBalance = 1; }
        Balance.text = $"{CurrentBalance}";
    }

    private void Update ()
    {
        if (isPointerDown)
        {
            // Increment hold time
            holdTime += Time.deltaTime;

            // If the hold time exceeds the specified duration, trigger the alternate action
            if (holdTime >= holdDuration)
            {
                TriggerAlternateAction();
                // Optionally reset the holdTime and isHolding to allow repeated actions
                holdTime = 0f;
                isHolding = false;
            }
        }
        else
        {
            // Reset hold time if the toggle is released or turned off
            holdTime = 0f;
            isPointerDown = false;
        }
        // Countdown timer
        timer -= Time.deltaTime;

        // When timer reaches zero
        if (timer <= 0f)
        {
            if (!ActionPerformed)
            {
                ActionPerformed = true;
                PerformAction();
            }

        }
    }

    void PerformAction ()
    {
        // Add your logic here
        StartCoroutine(PerformTask());
    }

    public IEnumerator PerformTask ()
    {
        yield return new WaitForSeconds(10);
        timer = timerDuration;
        ActionPerformed = false;
    }

    private void TriggerAlternateAction ()
    {
       // Debug.Log("Alternate Action Triggered");
        // Add your custom logic here
        if (IsIncreaseButton)
        {
            IncreaseBalance();
        }
        else if(IsDecreaseButton)
        {
            DecreaseBalance();
        }
    }

    public void TriggerIncreaseBtn ()
    {
        IsIncreaseButton = true;
        IsDecreaseButton = false;
    }

    public void TriggerDecreaseBtn ()
    {
        IsDecreaseButton = true;
        IsIncreaseButton =false;
    }

    public void Refresh ()
    {
        IsIncreaseButton = false;
        IsDecreaseButton = false;
    }
}
