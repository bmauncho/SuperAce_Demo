using TMPro;
using UnityEngine;

public class FreeGameWinUI : MonoBehaviour
{
    public float totalAmount;
    public TMP_Text CurrentWinAmount;
    public float increaseDuration = 1f; // Duration of the increase
    private float startTime;

    public bool canUpdateWinnings = false;
    public GameObject TheWinAmountText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        startTime = Time.time;
        totalAmount = GetTotalAmount();
    }

    private void OnEnable ()
    {
        startTime = Time.time;
        totalAmount = GetTotalAmount();
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
        float t = ( Time.time - startTime ) / increaseDuration;
        float currentValue = Mathf.Lerp(0f , totalAmount , t);
        CurrentWinAmount.text = currentValue.ToString("F2");
        
    }

    float GetTotalAmount ()
    {
        float theAmount = 0f;
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            theAmount = float.Parse(CommandCentre.Instance.CashManager_.WinCashAmountText [1].text.ToString());

        }
        else
        {
            theAmount = float.Parse(CommandCentre.Instance.CashManager_.WinCashAmountText [0].text.ToString());
        }
        return theAmount;
    }
}