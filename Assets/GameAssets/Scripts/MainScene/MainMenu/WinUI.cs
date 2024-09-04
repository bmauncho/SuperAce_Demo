using UnityEngine;

public class WinUI : MonoBehaviour
{
    public GameObject CurrentWinnings;
    public GameObject TotalWinnings;

    public void ActivateCurrentWinings ()
    {
        CurrentWinnings.SetActive (true);
    }

    public void DeactivateCurrentWinings ()
    {
        CurrentWinnings.SetActive (false);
    }

    public void ActivateTotalWinnings ()
    {
        TotalWinnings.SetActive (true);
    }

    public void DeactivateTotalWinnings ()
    {
        TotalWinnings.SetActive (false);
    }
}
