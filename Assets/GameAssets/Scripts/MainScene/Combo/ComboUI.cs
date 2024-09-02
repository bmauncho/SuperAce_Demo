using UnityEngine;

public class ComboUI : MonoBehaviour
{
    public ComboController NormalCombo;
    public ComboController FreeGameCombo;


    public void ActivateNormalCombo ()
    {
        NormalCombo.gameObject.SetActive (true);
    }

    public void DeactivateNormalCombo ()
    {
        NormalCombo .gameObject.SetActive (false);
    }

    public void ActivateFreeGameCombo ()
    {
        FreeGameCombo.gameObject.SetActive (true);
    }

    public void DeactivateFreeGameCombo ()
    {
        FreeGameCombo.gameObject.SetActive (false);
    }
}
