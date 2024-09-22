using UnityEngine;

public class GamePlayMenuController : MonoBehaviour
{
    public BetButton []betsBtn;
    public GameObject NormalGamePlay;
    public GameObject DemoGamePlay;
   

    public void ShowNormalGamePlayMenu ()
    {
        NormalGamePlay.SetActive (true);
    }

    public void HideNormalGamePlayMenu ()
    {
        NormalGamePlay.SetActive(false);
    }

    public void ShowDemoGamePlayMenu ()
    {
        DemoGamePlay.SetActive(true);
    }

    public void HideDemoGamePlayMenu ()
    {
        DemoGamePlay.SetActive(false);
    }
}
