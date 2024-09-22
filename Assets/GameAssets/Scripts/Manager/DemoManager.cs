using DG.Tweening;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    public bool IsDemo = false;
    public GameObject DemoUi;

    [Header("References")]
    [Space(10)]
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
        ActivateDemoUI();

        Invoke(nameof(DemoSpin) , 3f);
    }

    public void DemoSpin ()
    {
        CommandCentre.Instance.MainMenuController_.Spin();
        Debug.Log("DemoSpin");
    }

    public void StartDemoFromWinMoreMenu ()
    {
        IsDemo = true;
        CommandCentre.Instance .MainMenuController_.DisableWinMoreMenu();
        CommandCentre.Instance .MainMenuController_.EnableGameplayMenu();
        CommandCentre.Instance.BetManager_.refreshBetSlip();
        ActivateDemoUI ();
        Invoke(nameof(DemoSpin) , 3f);
    }

    public void ActivateDemoUI ()
    {
        DemoUi.SetActive(true);
        DemoUi.GetComponent<CanvasGroup>().DOFade(1 , .5f)
            .OnComplete(() =>
            {
                Invoke(nameof(DeactivateDemoUI) , 1f);
            });
    }

    public void DeactivateDemoUI ()
    {
        DemoUi.GetComponent<CanvasGroup>().DOFade(0 , .5f)
            .OnComplete(() =>
            {
                DemoUi.SetActive(false);
            });
    }
}