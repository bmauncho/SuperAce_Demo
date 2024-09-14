using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoSpin : MonoBehaviour
{
    public bool IsAutoSpin = false;
    public bool IsPressingBtn = false;
    public GameObject AutospinHolder;
    public TMP_Text AutoSpinText;
    public Toggle AutoSpinToggle;

    public float holdDuration = 2.0f; // Duration in seconds for holding the toggle
    public float holdTime = 0f;
    public bool isHolding = false;
    public bool isPointerDown = false;
    private void Start ()
    {
       AutoSpinToggle = GetComponentInChildren<Toggle>();
    }
    public void IsAutoSpinPressed ()
    {
        if (AutoSpinToggle.isOn)
        {
            CommandCentre.Instance.AutoSpinManager_.EnableAutoSpin();
        }
        else
        {
            CommandCentre.Instance.AutoSpinManager_.DisableAutoSpin();
        }
        IsAutoSpin = AutoSpinToggle.isOn;
    }

    private void Update ()
    {
        if (CommandCentre.Instance)
        {
            if (CommandCentre.Instance.AutoSpinManager_.IsAutoSpin)
            {
                AutospinHolder.SetActive(true);
                AutoSpinText.text = CommandCentre.Instance.AutoSpinManager_.AutoSpinIndex_.ToString();
            }
            else
            {
                AutospinHolder.SetActive(false);
            }
        }
        // If the toggle is being held down
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
    }

  
    private void TriggerAlternateAction ()
    {
        Debug.Log("Alternate Action Triggered");
        // Add your custom logic here
        CommandCentre.Instance.SettingsManager_.SettingsController_.ActivateAutoSpinSetting();
    }
}


