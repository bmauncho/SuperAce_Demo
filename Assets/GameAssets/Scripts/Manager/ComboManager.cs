using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ComboAmount
{
    public List<int> NormalAmount = new List<int>();
    public List<int> FreeGameAmount = new List<int>();
}


public class ComboManager : MonoBehaviour
{
    public int ComboCounter = 0;
    public TMP_Text ComboText;
    public GameObject ComboVisual;
    public ComboUI ComboUI_;

    public ComboAmount ComboAmount; 

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
            ComboCounter= ComboCounter+2;
            if (ComboCounter >= 10) { ComboCounter = 10; }
        }
        else
        {
            ComboUI_.NormalCombo.ShowCombo();
            ComboCounter++;
            if (ComboCounter > 3) { ComboCounter = 5; }
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

    public int GetCombo()
    {
        if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            for (int i = 0 ; i < ComboAmount.FreeGameAmount.Count ; i++)
            {
                if (i == ComboCounter)
                {
                    return ComboAmount.FreeGameAmount [i];
                }
            }
        }
        else
        {

            for (int i = 0 ; i < ComboAmount.NormalAmount.Count ; i++)
            {
                if (i == ComboCounter)
                {
                    return ComboAmount.NormalAmount [i];
                }
            }
        }
        return 1;
    }
}
