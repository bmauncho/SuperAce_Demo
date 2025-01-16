using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    public bool IsDemo = false;
    public bool IsDemoFeatureActive; 
    public GameObject DemoFeature;
    public GameObject DemoUi;

    [Header("References")]
    [Space(10)]
    public MainScene_LoadingMenu MainScene_LoadingMenu_;
    public DemoSequence DemoSequence_;
    public DemoGridManager DemoGridManager_;
    public DemoWinLoseManager DemoWinLoseManager_;

    public bool isScatterSpin;
    [SerializeField]private string [] originalWinAmounts; // To store the original amounts
    public string [] winAmount;
    public int winIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeWinAmounts(winAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (CommandCentre.Instance)
        {

            UpdateWinAmounts(CommandCentre.Instance.BetManager_.BetAmount);
        }
    }

    

    public void InitializeWinAmounts ( string [] initialAmounts )
    {
        originalWinAmounts = (string [])initialAmounts.Clone();
        winAmount = (string [])initialAmounts.Clone();
    }

    public void UpdateWinAmounts ( float betAmount )
    {
        for (int i = 0 ; i < originalWinAmounts.Length ; i++)
        {
            if (float.TryParse(originalWinAmounts [i] , out float originalAmount))
            {
                winAmount [i] = ( originalAmount * betAmount / 10f ).ToString("F2");
            }
            else
            {
                Debug.LogWarning($"Invalid original win amount at index {i}: {originalWinAmounts [i]}");
            }
        }
    }




    public void StartDemo ()
    {
        IsDemo = true;
        CommandCentre.Instance.CashManager_.CashAmount = 2000;
        CommandCentre.Instance.CashManager_.updateThecashUi();
        MainScene_LoadingMenu_.ContinueToMainGame();
        CommandCentre.Instance.MainMenuController_.StartGame();
        ActivateDemoUI();

        CommandCentre.Instance.BetManager_.refreshBetSlip();

        Invoke(nameof(DemoSpin) , 3f);
    }

    public void DemoSpin ()
    {
        if(IsDemoFeatureActive)
        {
            DeactivateDemoFeature();
            
        }

        StartCoroutine(spin());
    }

    public IEnumerator spin ()
    {
        if (isScatterSpin)
        {
            yield return new WaitUntil(() => DemoSequence_.isScattersetUpDone);
        }
        CommandCentre.Instance.MainMenuController_.Spin();
    }
    public void StartDemoFromWinMoreMenu ()
    {
        IsDemo = true;
        CommandCentre.Instance.MainMenuController_.IsDemo =true;
        DemoGridManager_.demoObjectsPlaced = 20;
        DemoGridManager_.isFirstPlay = false;
        CommandCentre.Instance .MainMenuController_.DisableWinMoreMenu();
        CommandCentre.Instance .MainMenuController_.EnableGameplayMenu();
        CommandCentre.Instance.CashManager_.CashAmount = 2000;
        CommandCentre.Instance.CashManager_.updateThecashUi();
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
        CommandCentre.Instance.MainMenuController_.EnableGameplayMenu();
    }

    public void Demofill ()
    {

    }

    public void DemoRefill ()
    {

    }

}
