using DG.Tweening;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    public bool IsDemo = false;
    public bool CanShowDemoFeature;
    public bool IsDemoFeatureActive; 
    public GameObject DemoFeature;
    public GameObject DemoUi;

    [Header("References")]
    [Space(10)]
    public MainScene_LoadingMenu MainScene_LoadingMenu_;
    public DemoSequence DemoSequence_;
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

        CommandCentre.Instance.BetManager_.refreshBetSlip();

        Invoke(nameof(DemoSpin) , 3f);
    }

    public void DemoSpin ()
    {
        CommandCentre.Instance.MainMenuController_.Spin();
        //Debug.Log("DemoSpin");
        if(IsDemoFeatureActive)
        {
            DeactivateDemoFeature();
        }
    }

    public void StartDemoFromWinMoreMenu ()
    {
        IsDemo = true;
        CommandCentre.Instance .MainMenuController_.DisableWinMoreMenu();
        CommandCentre.Instance .MainMenuController_.EnableGameplayMenu();
        CommandCentre.Instance.BetManager_.refreshBetSlip();
        ActivateDemoUI ();
        Invoke(nameof(DemoSpin) , 1f);
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

    public void ActivateDemoFeature ()
    {
        DemoFeature.SetActive(true);
        IsDemoFeatureActive = true;
    }

    public void DeactivateDemoFeature ()
    {
        DemoFeature.SetActive(false) ;
        IsDemoFeatureActive= false;
    }

    public void RealMode ()
    {
        DeactivateDemoFeature();
        CommandCentre.Instance.MainMenuController_.EnableWinMoreMenu();
        CommandCentre.Instance.FreeGameManager_.winMoreMenu_.DeactivateDemoBtn();
        CommandCentre.Instance.FreeGameManager_.winMoreMenu_.DeactivateSuggestion_1();
        CommandCentre.Instance.DemoManager_.IsDemo = false;
        CommandCentre.Instance.MainMenuController_.GameplayMenu.GetComponent<GamePlayMenuController>().ShowNormalGamePlayMenu();
        CommandCentre.Instance.MainMenuController_.GameplayMenu.GetComponent<GamePlayMenuController>().HideDemoGamePlayMenu();

    }
}
