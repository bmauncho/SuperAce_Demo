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
    private void Update ()
    {

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
        if (whichCombo > 5)
        {
            whichCombo = 5; // Cap the value at 5
        }
        TheCombos [TheCombos.Length - 1].gameObject.SetActive(true); // Adjust the index to avoid out-of-bounds
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
}
