using System.Collections;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    public Combo [] TheCombos;
    public int whichCombo;

    [ContextMenu("Activate Combo")]
    public void ShowCombo ()
    {
        ActivateCombo(whichCombo);
    }
    private void Start ()
    {
        if (whichCombo == 0)
        {
            TheCombos [whichCombo].gameObject.SetActive(true);
        }
    }

    public void ActivateCombo ( int which )
    {
        refresh();
        for (int i = 0 ; i < TheCombos.Length ; i++)
        {
            if (i == which)
            {
                TheCombos [which].gameObject.SetActive(true);
            }
        }
        whichCombo++;
        if (whichCombo >= 5)
        {
            whichCombo = 5; // Cap the value at 5
            TheCombos [TheCombos.Length - 1].gameObject.SetActive(true); // Adjust the index to avoid out-of-bounds
        }
        
    }

    public void refresh ()
    {
        foreach(Combo combo in TheCombos)
        {
            combo.gameObject.SetActive (false);
        }
    }

    [ContextMenu("Reset Combo")]
    public void ResetCombos()
    {
        for (int i = 0 ; i < TheCombos.Length ; i++)
        {
            if (i == 0)
            {
                TheCombos [i].gameObject.SetActive(true);
            }
            else
            {
                TheCombos [i].gameObject.SetActive(false);
            }
        }
        whichCombo = 0;
    }

    [ContextMenu("show free gamecombo ui")]
    public void ShowUi ()
    {
        for (int i = 0 ; i < TheCombos.Length+1 ; i++) // Iterate up to TheCombos.Length - 1
        {
            TheCombos [0].gameObject.SetActive(true);
            int currentIndex = i;
            StartCoroutine(ActivateComboUI(currentIndex));
        }
    }

    public IEnumerator ActivateComboUI ( int which )
    {
        yield return new WaitForSeconds(0.3f * ( which - 1 )); // Wait for 0.3 seconds multiplied by the current index minus 1

        // Check if which is not 0 before accessing TheCombos[which - 1]
        if (which > 1)
        {
            TheCombos [which - 1].gameObject.SetActive(false); // Deactivate the previous UI element
        }

        if (which <= TheCombos.Length - 1)
        {

            TheCombos [which].gameObject.SetActive(true); // Activate the current UI element
        }

        if (which == TheCombos.Length - 1) // If this is the last UI element
        {
            TheCombos [0].gameObject.SetActive(true); // Reactivate the first UI element
        }
        else
        {
            TheCombos [TheCombos.Length - 1].gameObject.SetActive(false); // Deactivate the last UI element
        }
    }
}
