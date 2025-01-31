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
    public int freeGameComboCounter = 0;
    public TMP_Text ComboText;
    public GameObject ComboVisual;
    public ComboUI ComboUI_;

    public ComboAmount ComboAmount;
    public int comboIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ComboText.text = $"{ComboCounter}";
    }

    public void ActivateComboUI ()
    {
        ComboVisual.SetActive(true);
        ComboVisual.GetComponentInChildren<Animator>().Rebind();
        ComboVisual.GetComponentInChildren<Animator>().Play("ComboAnim");
    }

    public void DeactivateComboUI ()
    {
       ComboVisual.SetActive(false);
    }
    [ContextMenu("IncreaseComboCounter")]
    public void IncreaseComboCounter ()
    {
        ActivateComboUI ();

        if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            ComboUI_.FreeGameCombo.ShowCombo();
            ComboCounter= ComboCounter+2;
            freeGameComboCounter++;
            if (ComboCounter > 6) { ComboCounter = 10; }
            if(freeGameComboCounter>3) { freeGameComboCounter = 5; }
            comboIndex++;
        }
        else
        {
            ComboUI_.NormalCombo.ShowCombo();
            ComboCounter++;
            if (ComboCounter > 3) { ComboCounter = 5; }
            comboIndex++;
        }
        
    }

    public void ResetComboCounter ()
    {
        DeactivateComboUI ();
        ComboCounter = 0;
        freeGameComboCounter = 0;
        comboIndex = 0;
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
                if (i == freeGameComboCounter)
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
