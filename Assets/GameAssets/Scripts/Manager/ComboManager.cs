using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public int ComboCounter = 0;
    public TMP_Text ComboText;
    public GameObject ComboVisual;
    public ComboUI ComboUI_;
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
            ComboUI_.FreeGameCombo.ShowCombo();
            ComboCounter += 2;
        }
        else
        {
            ComboUI_.NormalCombo.ShowCombo();
            ComboCounter++;
        }
        
    }

    public void ResetComboCounter ()
    {
        DeactivateComboUI ();
        ComboCounter = 0;
        if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            ComboUI_.FreeGameCombo.ResetCombos ();
        }
        else
        {
            ComboUI_.NormalCombo.ResetCombos();
        }
    }

    public void SetCombo ()
    {

    }
}
