using System.Collections;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject GameplayMenu;
    public GameObject StartGameScreen;

    public bool CanSpin = false;
    public bool isBtnPressed = false;

    void Start ()
    {
        // Initialization logic if needed
    }

    void Update ()
    {
        if (CommandCentre.Instance)
        {
            CanSpin = CommandCentre.Instance.GridManager_.IsGridCreationComplete();
            if (CanSpin)
            {

                EnableGameplayMenu();
            }
        }
    }

    public void StartGame ()
    {
        DisableStartGameScreen();
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
                yield break;
            }
            yield return null;
        }
    }

    public void EnableStartGameScreen ()
    {
        StartGameScreen.SetActive(true);
    }

    public void DisableStartGameScreen ()
    {
        StartGameScreen.SetActive(false);
    }

    public void EnableGameplayMenu ()
    {
        GameplayMenu.SetActive(true);
    }

    public void DisableGameplayMenu ()
    {
        GameplayMenu.SetActive(false);
    }

    public void Spin ()
    {
        if (!isBtnPressed)
        {
            if (CanSpin && CommandCentre.Instance.WinLoseManager_.enableSpin)
            {
                StartCoroutine(SpinReel());
            }
            isBtnPressed = true;
        }
    }

    IEnumerator SpinReel ()
    {
        CommandCentre.Instance.WinLoseManager_.enableSpin = false;
        CommandCentre.Instance.GridManager_.ResetGrid();
        yield break;
    }
}
