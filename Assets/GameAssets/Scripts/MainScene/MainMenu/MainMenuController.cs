using System.Collections;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Variables")]
    public bool CanSpin = false;
    public bool isBtnPressed = false;

    public bool IsFreeGame = false;
    public bool IsFirstTimeDone = false;
    [Space(10)]
    [Header("Menus")]
    public GameObject GameplayMenu;
    public GameObject StartGameMenu;
    public GameObject BetingMenu;
    public GameObject WinMoreMenu;


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
        Invoke(nameof(SetGrid) , .5f);
    }

    void SetGrid ()
    {
        CommandCentre.Instance.GridManager_.IsFirstTime = false;
        CommandCentre.Instance.GridManager_.CheckGrid();
    }

    void Update ()
    {
        if (CommandCentre.Instance)
        {
            CanSpin = CommandCentre.Instance.GridManager_.IsGridCreationComplete();
            if (CanSpin)
            {
                EnableGameplayMenu();
                if (!IsFirstTimeDone)
                {
                    Invoke(nameof(EnableWinMoreMenu) , .5f);
                    IsFirstTimeDone = true;
                }
            }
        }
    }

    public void StartGame ()
    {
        DisableStartGameMenu();
        Invoke(nameof(ManualStart) , 0.25f);
    }

    public void ManualStart ()
    {
        CommandCentre.Instance.GridManager_.ManualStart();
        StartCoroutine(CheckIfGridReady());
    }

    IEnumerator CheckIfGridReady ()
    {
        while (!CanSpin)
        {
            CanSpin = CommandCentre.Instance.GridManager_.IsGridCreationComplete();
            if (CanSpin)
            {
                EnableGameplayMenu();
                Invoke(nameof(EnableWinMoreMenu) , .5f);
                yield break;
            }
            yield return null;
        }
    }

   

    public void Spin ()
    {
        Debug.Log("Spin");
        Debug.Log(isBtnPressed);
        if (!isBtnPressed)
        {
            if (CanSpin /*&& CommandCentre.Instance.WinLoseManager_.enableSpin*/)
            {
                StartCoroutine(SpinReel());
            }
            isBtnPressed = true;
        }
        CommandCentre.Instance.HintManager_.CanShowHints = true;    
    }

    IEnumerator SpinReel ()
    {
        Debug.Log("Spinning");
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
        CommandCentre.Instance.CashManager_.ResetWinings();
        CommandCentre.Instance.WinLoseManager_.enableSpin = false;
        CommandCentre.Instance.GridManager_.ResetGrid();
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
        BetingMenu.SetActive(false);
    }

    public void Bet ()
    {
        GamePlayMenuController gpmc = GameplayMenu.GetComponent<GamePlayMenuController>();
        if (gpmc.betsBtn.BetsButton.isOn)
        {
            OpenBets();
        }
        else
        {
            CloseBets();
        }
    }
}
