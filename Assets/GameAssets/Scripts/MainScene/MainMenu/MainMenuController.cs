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
        CommandCentre.Instance.GridManager_.populateGrid();
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
        Debug.Log(1);
        if (!isBtnPressed)
        {
            if (CommandCentre.Instance.CashManager_.CashAmount <= 0
                || CommandCentre.Instance.CashManager_.CashAmount < CommandCentre.Instance.BetManager_.BetAmount)
            {
                InsufficientAmount.SetActive(true);
                return;
            }
            Debug.Log(2);
            Debug.Log($"Cans spin is : {CanSpin}");
            if (CanSpin)
            {
                Debug.Log(3);
                StartCoroutine(FetchDataAndSpin());
            }
            Debug.Log(2.1);
            isBtnPressed = true;
        }
        Debug.Log(2.2);
        CommandCentre.Instance.HintManager_.CanShowHints = true;
        Debug.Log(2.3);
    }

    private IEnumerator FetchDataAndSpin ()
    {

        bool datafetched = CommandCentre.Instance.APIManager_.isDataFetched;
        CommandCentre.Instance.APIManager_.FetchInfo();

        // Wait until data is fetched, without freezing the game
        while (!datafetched)
        {
            datafetched = CommandCentre.Instance.APIManager_.isDataFetched;
            yield return null; // Wait for the next frame
        }

        // Once data is fetched, start the spinning process
        StartCoroutine(SpinReel());
    }

    IEnumerator SpinReel ()
    {
        //Debug.Log("Spinning");
        CommandCentre.Instance.GridManager_.refreshGrid();
        if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            CommandCentre.Instance.FreeGameManager_.DecreaseFreespins();
            
        }
        else
        {
            CommandCentre.Instance.CashManager_.DecreaseCash(CommandCentre.Instance.BetManager_.BetAmount);
        }
        DecreaseAutoSpins();
        CommandCentre.Instance.ComboManager_.ResetComboCounter();
        if (!CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            CommandCentre.Instance.CashManager_.ResetWinings();
        }
        CommandCentre.Instance.APIManager_.isDataFetched =false;
         yield break;
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
