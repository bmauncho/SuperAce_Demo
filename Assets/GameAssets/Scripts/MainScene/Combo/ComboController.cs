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

    public void ActivateCombo (int which)
    {
        refresh ();
        for(int i = 0; i < TheCombos.Length; i++)
        {
            if (i== which)
            {
                TheCombos [which].gameObject.SetActive (true);
            }
        }
        whichCombo++;
        if (whichCombo > TheCombos.Length)
        {
            whichCombo = 0;
        }
    }

    public void refresh ()
    {
        foreach (Combo combo in TheCombos)
        {
            combo.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Reset Combo")]
    public void ResetCombos()
    {
        refresh();
        whichCombo = 0;
    }
}
