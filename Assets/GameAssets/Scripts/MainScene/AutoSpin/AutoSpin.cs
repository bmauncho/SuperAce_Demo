using System.Collections;
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

    public AutoSpinFx AutospinFx_;

    public float timerDuration = 10f;  // Set the timer duration (10 seconds)
    private float timer;
    bool showfx = false;
    [Header("Cycle A")]
    public TMP_Text suggestionsA;
    public string [] suggestionsListA;
    [Header("Cycle B")]
    public TMP_Text suggestionsB;
    public string [] suggestionsListB;

    public float delayBeforeShow = 2f;
    public float showDuration = 3f;
    public float delayBetweenSuggestions = 2f;
    private void Start ()
    {
        AutoSpinToggle = GetComponentInChildren<Toggle>();
        timer = timerDuration;
        if (suggestionsA != null && suggestionsListA.Length > 0 &&
                 suggestionsB != null && suggestionsListB.Length > 0)
        {
            suggestionsA.gameObject.SetActive(false);
            suggestionsB.gameObject.SetActive(false);

            StartCoroutine(CycleAthenB());
        }
    }
    private IEnumerator CycleAthenB ()
    {
        while (true)
        {
            // Run Cycle A once through its list
            yield return StartCoroutine(RunSuggestionCycle(suggestionsA , suggestionsListA));

            // Run Cycle B once through its list
            yield return StartCoroutine(RunSuggestionCycle(suggestionsB , suggestionsListB));
        }
    }

    private IEnumerator RunSuggestionCycle ( TMP_Text textComp , string [] suggestions )
    {
        for (int i = 0 ; i < suggestions.Length ; i++)
        {
            yield return new WaitForSeconds(delayBeforeShow);

            textComp.text = suggestions [i];
            textComp.gameObject.SetActive(true);

            yield return new WaitForSeconds(showDuration);

            textComp.gameObject.SetActive(false);

            yield return new WaitForSeconds(delayBetweenSuggestions);
        }
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
        // Countdown timer
        timer -= Time.deltaTime;

        // When timer reaches zero
        if (timer <= 0f)
        {
            if (!showfx)
            {
                showfx = true;
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
        AutospinFx_.Activate();
        yield return new WaitForSeconds(10);
        AutospinFx_.Deactivate();
        timer = timerDuration;
        showfx = false;
    }

    private void TriggerAlternateAction ()
    {
       // Debug.Log("Alternate Action Triggered");
        // Add your custom logic here
        CommandCentre.Instance.SettingsManager_.SettingsController_.ActivateAutoSpinSetting();
    }
}


