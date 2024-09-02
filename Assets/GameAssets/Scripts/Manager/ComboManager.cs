using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public int ComboCounter = 0;
    public TMP_Text ComboText;
    public GameObject ComboVisual;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ComboText.text = $"Combo {ComboCounter}";
    }

    public void ActivateComboUI ()
    {
        ComboVisual.SetActive(true);
    }

    public void DeactivateComboUI ()
    {
       ComboVisual.SetActive(false);
    }

    public void IncreaseComboCounter ()
    {
        ActivateComboUI ();

        if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            ComboCounter += 2;
        }
        else
        {
            ComboCounter++;
        }
        
    }

    public void ResetComboCounter ()
    {
        DeactivateComboUI ();
        ComboCounter = 0;
    }

    public void SetCombo ()
    {

    }
}
