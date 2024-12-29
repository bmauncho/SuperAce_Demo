using System.Collections;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Variables")]
    public bool CanSpin = false;
    public bool isBtnPressed = false;

    public bool IsFreeGame = false;
    public bool IsFirstTimeDone = false;
    public bool IsDemo = false;
    [Space(10)]
    [Header("Menus")]
    public GameObject GameplayMenu;
    public GameObject StartGameMenu;
    public GameObject BetingMenu;
    public GameObject WinMoreMenu;
    public GameObject InsufficientAmount;

    [Space(10)]
    [Header("References")]
    public StartFx Startfx_;


    void Start ()
    {
        // Initialization logic if needed
    }

    public void EnableStartGameMenu ()
    {
        StartGameMenu.SetActive(true);
    }

    public void DisableStartGameMenu ()
    {
        StartGameMenu.SetActive(false);
    }

    public void EnableGameplayMenu ()
    {
        GameplayMenu.SetActive(true);
        GamePlayMenuController GPMC = GameplayMenu.GetComponent<GamePlayMenuController>();
        if (IsDemo)
        {
            GPMC.ShowDemoGamePlayMenu();
            GPMC.HideNormalGamePlayMenu();
        }
        else
        {
            GPMC.HideDemoGamePlayMenu();
            GPMC.ShowNormalGamePlayMenu();
        }
    }

    public void DisableGameplayMenu ()
    {
        GameplayMenu.SetActive(false);
    }

    public void EnableWinMoreMenu ()
    {
        WinMoreMenu.SetActive(true);
    }

    public void DisableWinMoreMenu ()
    {
        WinMoreMenu.SetActive(false);
    }

    void Update ()
    {
        
    }

    public void StartGame ()
    {
        DisableStartGameMenu();
        Invoke(nameof(ManualStart) , 0.25f);
    }

    public void ManualStart ()
    {
        Startfx_.Activate();
        StartCoroutine(CheckIfGridReady());
        if (!CommandCentre.Instance.DemoManager_.IsDemo)
        {
            CommandCentre.Instance.GridManager_.populateGrid();
        }
        else
        {
            CommandCentre.Instance.DemoManager_.DemoGridManager_.populateGrid();
        }
        
    }

    IEnumerator CheckIfGridReady ()
    {
        while (!CanSpin)
        {
            CanSpin = CommandCentre.Instance.GridManager_.isGridFilled();
            if (CanSpin)
            {
                EnableGameplayMenu();
                yield break;
            }
            yield return null;
        }
    }

    public void Spin ()
    {
        if (!isBtnPressed)
        {
            if (CommandCentre.Instance.CashManager_.CashAmount <= 0
                || CommandCentre.Instance.CashManager_.CashAmount < CommandCentre.Instance.BetManager_.BetAmount)
            {
                InsufficientAmount.SetActive(true);
                return;
            }
            if (CanSpin)
            {
                StartCoroutine(FetchDataAndSpin());
            }
            isBtnPressed = true;
        }

        CommandCentre.Instance.HintManager_.CanShowHints = true;
    }

    private IEnumerator FetchDataAndSpin ()
    {
        Debug.Log(CommandCentre.Instance.DemoManager_.IsDemo);
        if (!CommandCentre.Instance.DemoManager_.IsDemo)
        {
            bool datafetched = CommandCentre.Instance.APIManager_.GameDataAPI_.isDataFetched;
            CommandCentre.Instance.APIManager_.GameDataAPI_.FetchInfo();

            // Wait until data is fetched, without freezing the game
            while (!datafetched)
            {
                datafetched = CommandCentre.Instance.APIManager_.GameDataAPI_.isDataFetched;
                yield return null; // Wait for the next frame
            }
            // Once data is fetched, start the spinning process
            StartCoroutine(SpinReel(false));
        }
        else
        {
            // Once data is fetched, start the spinning process
            StartCoroutine(SpinReel(true));
        }

    
    }

    IEnumerator SpinReel (bool isDemo)
    {
        //Debug.Log("Spinning");
        
        if (!isDemo)
        {
            CommandCentre.Instance.GridManager_.refreshGrid();
            normlSpin();
        }
        else
        {
           CommandCentre.Instance.DemoManager_.DemoGridManager_.refreshDemoGrid();
            // show demo
            DemoSpin();
        }
         yield break;
    }

    void normlSpin ()
    {

        if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            CommandCentre.Instance.FreeGameManager_.DecreaseFreespins();

        }

        DecreaseAutoSpins();
        CommandCentre.Instance.ComboManager_.ResetComboCounter();
        if (!CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            CommandCentre.Instance.CashManager_.ResetWinings();
        }
        CommandCentre.Instance.APIManager_.GameDataAPI_.isDataFetched = false;
        //CommandCentre.Instance.APIManager_.PlaceBet();
    }

    void DemoSpin ()
    {
        if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            CommandCentre.Instance.FreeGameManager_.DecreaseFreespins();

        }
        DecreaseAutoSpins();
        CommandCentre.Instance.ComboManager_.ResetComboCounter();
        if (!CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            CommandCentre.Instance.CashManager_.ResetWinings();
        }
    }


    void DecreaseAutoSpins ()
    {
        if (CommandCentre.Instance.AutoSpinManager_.IsAutoSpin)
        {
            CommandCentre.Instance.AutoSpinManager_.DecreaseAutoSpins();
        }
    }

    public void OpenBets ()
    {
        BetingMenu.SetActive(true);
    }

    public void CloseBets ()
    {
        GamePlayMenuController gpmc = GameplayMenu.GetComponent<GamePlayMenuController>();
        if (IsDemo)
        {
            gpmc.betsBtn [1].BetsButton.isOn = false;   
        }
        else
        {
            gpmc.betsBtn [0].BetsButton.isOn = false;
        }
        BetingMenu.SetActive(false);
    }

    public void Bet ()
    {
        GamePlayMenuController gpmc = GameplayMenu.GetComponent<GamePlayMenuController>();
        if (IsDemo)
        {
            if (gpmc.betsBtn [1].BetsButton.isOn)
            {
                OpenBets();
            }
            else
            {
                CloseBets();
            }
        }
        else
        {
            if (gpmc.betsBtn [0].BetsButton.isOn)
            {
                OpenBets();
            }
            else
            {
                CloseBets();
            }
        }
       
    }
}
