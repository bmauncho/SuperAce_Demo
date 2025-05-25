using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    bool init = false;
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
        if (!init)
        {
            if (GameManager.Instance)
            {
                if (GameManager.Instance.IsDemo())
                {
                    // set cards on grid

                    StartDemoOnstart();
                    init = true;
                }
                else
                {
                    //Debug.Log("Not in demo mode");
                    IsDemo = false;
                    CommandCentre.Instance.MainMenuController_.IsDemo = false;
                    CommandCentre.Instance.MainMenuController_.StartGameMenu.SetActive(true);
                    init = true;
                }
            }
            
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

    void StartDemoOnstart ()
    {
        IsDemo = true;
        CommandCentre.Instance.MainMenuController_.IsDemo = true;
        CommandCentre.Instance.MainMenuController_.StartGameMenu.SetActive(false);
        CommandCentre.Instance.MainMenuController_.GameplayMenu.SetActive(true);
        CommandCentre.Instance.MainMenuController_.GameplayMenu.GetComponent<GamePlayMenuController>().ShowDemoGamePlayMenu();
        CommandCentre.Instance.MainMenuController_.GameplayMenu.GetComponent<GamePlayMenuController>().HideNormalGamePlayMenu();
        CommandCentre.Instance.MainMenuController_.CanSpin = true;
        DemoGridManager_.demoObjectsPlaced = 20;
        DemoGridManager_.isFirstPlay = false;
        CommandCentre.Instance.BetManager_.refreshBetSlip();
        ActivateDemoUI();
        Invoke(nameof(DemoSpin) , 1f);
    }


    public void StartDemo ()
    {
        IsDemo = true;
        //CommandCentre.Instance.CashManager_.CashAmount = 2000;
        //CommandCentre.Instance.CashManager_.updateThecashUi();
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
        CommandCentre.Instance.FreeGameManager_.winMoreMenu_.DeactivateDemoBtn();
        CommandCentre.Instance.FreeGameManager_.winMoreMenu_.DeactivateSuggestion_1();
        CommandCentre.Instance.DemoManager_.IsDemo = false;
        CommandCentre.Instance.MainMenuController_.IsDemo = false;
        //Debug.Log($"Is Demo {CommandCentre.Instance.DemoManager_.IsDemo}");
        CommandCentre.Instance.MainMenuController_.GameplayMenu.GetComponent<GamePlayMenuController>().HideDemoGamePlayMenu();
        CommandCentre.Instance.MainMenuController_.GameplayMenu.GetComponent<GamePlayMenuController>().ShowNormalGamePlayMenu();
        CommandCentre.Instance.MainMenuController_.EnableWinMoreMenu();
        CommandCentre.Instance.MainMenuController_.EnableGameplayMenu();
        CommandCentre.Instance.MainMenuController_.CanSpin = true;
        CommandCentre.Instance.BetManager_.refreshBetSlip();
        Debug.Log("Change to normal game");
        ResetDemo();
        DemoGridManager_.isFirstPlay = true;
        CommandCentre.Instance.PayOutManager_.resetCurrentWinings();
        CommandCentre.Instance.ComboManager_.ResetComboCounter();
        CommandCentre.Instance.MainMenuController_.StartGameMenu.SetActive(true);
        winIndex = 0;
    }

    public void ResetDemo ()
    {
        List<cardPositions> colData = DemoGridManager_.GetGrid();

        foreach (var obj in colData)
        {
            foreach (var _obj in obj.cardPositionInRow)
            {
                var cardPos = _obj.GetComponent<CardPos>();
                if (cardPos)
                {
                    var card = cardPos.TheOwner;
                    if (card)
                    {
                        CommandCentre.Instance.PoolManager_.ReturnCard(card);
                        cardPos.TheOwner = null;
                    }
                    else
                    {
                        Debug.LogWarning($"CardPos found :{cardPos.name} but TheOwner is null: {card.name}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Transform does not have CardPos");
                }
            }
        }
    }

}
