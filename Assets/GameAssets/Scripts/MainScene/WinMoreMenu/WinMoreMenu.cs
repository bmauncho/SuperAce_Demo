using UnityEngine;

public class WinMoreMenu : MonoBehaviour
{
    public GameObject DemoBtn;
    public GameObject Suggestion_1;

    public void DeactivateSuggestion_1 ()
    {
        Suggestion_1.SetActive (false);
    }

    public void ActivateSuggestion_1 ()
    {
        Suggestion_1.SetActive(true);
    }

    public void DeactivateDemoBtn ()
    {
        DemoBtn.SetActive(false);
    }

    public void ActivateDemoBtn ()
    {
        DemoBtn.SetActive (true);
    }
}
