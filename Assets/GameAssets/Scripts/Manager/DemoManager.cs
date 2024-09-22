using UnityEngine;

public class DemoManager : MonoBehaviour
{
    public bool IsDemo = false;
    public MainScene_LoadingMenu MainScene_LoadingMenu_;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDemo ()
    {
        IsDemo = true;
        MainScene_LoadingMenu_.ContinueToMainGame();
        CommandCentre.Instance.MainMenuController_.StartGame();
    }

    public void StartDemoFromWinMoreMenu ()
    {
        IsDemo = true;
        CommandCentre.Instance .MainMenuController_.DisableWinMoreMenu();
        CommandCentre.Instance .MainMenuController_.EnableGameplayMenu();
        CommandCentre.Instance.BetManager_.refreshBetSlip();
    }
}
