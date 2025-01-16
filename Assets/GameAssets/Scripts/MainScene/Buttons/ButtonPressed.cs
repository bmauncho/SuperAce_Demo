using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    CommandCentre commandCentre;
    public bool isSpinButton = false;
    public bool IsToggle = false;
    public bool IsContinuebutton = false;
    private bool isButtonClicked = false; // Add this at the class level
    private void Start()
    {
        commandCentre=CommandCentre.Instance;
    }
    // Update is called once per frame
    void Update()
    {
        if (commandCentre)
        {
            if (isSpinButton)
            {
                if (commandCentre.DemoManager_.IsDemo)
                {
                    if (commandCentre.MainMenuController_.CanSpin)
                    {
                        EnableButtonInteractvity();
                        CommandCentre.Instance.MainMenuController_.isBtnPressed = false;
                    }
                    else
                    {
                        DisableButtonInteractvity ();
                        CommandCentre.Instance.MainMenuController_.isBtnPressed = true;
                    }
                }
                else
                {
                    if (commandCentre.MainMenuController_.CanSpin)
                    {
                        EnableButtonInteractvity();
                        CommandCentre.Instance.MainMenuController_.isBtnPressed = false;
                    }
                    else
                    {
                        DisableButtonInteractvity();
                        CommandCentre.Instance.MainMenuController_.isBtnPressed = true;
                    }

                    //if (commandCentre.GridManager_.isGridFilled() && !CommandCentre.Instance.WinLoseManager_.IsWin())
                    //{
                    //    EnableButtonInteractvity();
                    //    commandCentre.MainMenuController_.CanSpin = true;
                    //    CommandCentre.Instance.MainMenuController_.isBtnPressed = false;
                    //}
                    //else
                    //{
                    //    DisableButtonInteractvity();
                    //    commandCentre.MainMenuController_.CanSpin = false;
                    //    CommandCentre.Instance.MainMenuController_.isBtnPressed = true;
                    //}
                }
            }
            else if(IsToggle)
            {
                EnableButtonInteractvity();
                GamePlayMenuController GPMC = commandCentre.MainMenuController_.GameplayMenu.GetComponent<GamePlayMenuController>();
                if (commandCentre.MainMenuController_.CanSpin)
                {
                    GPMC.NormalGamePlay.GetComponent<NormalGamePlay>().setButtonsInteractivity(true);
                }
                else
                {
                    GPMC.NormalGamePlay.GetComponent<NormalGamePlay>().setButtonsInteractivity(false);
                }

            }
            else
            {
                EnableButtonInteractvity();
            }
        }
    }

    void EnableButtonInteractvity ()
    {
        if (IsToggle)
        {
            GetComponent<Toggle>().interactable = true;
        }
        else
        {
            if (isSpinButton)
            {
                isButtonClicked = false;
            }
            GetComponent<Button>().interactable = true;
        }
    }

    public void DisableButtonInteractvity ()
    {
        if (IsToggle)
        {
            GetComponent<Toggle>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }
    public void BounceBtn ()
    {
        if (this.gameObject.activeSelf)
        {
            DisableButtonInteractvity();
            if (isSpinButton)
            {
                if(isButtonClicked) return;
                if (!isButtonClicked)
                {
                    isButtonClicked = true;
                }
            }
            //Bounce();
            if (CommandCentre.Instance)
            {
                CommandCentre.Instance.SoundManager_.PlaySound("BtnClick" , false);
            }
        }
    }


    void Bounce ()
    {
        transform.DOScale(1.1f , .15f).SetUpdate(true).OnComplete(Debounce); ;
    }

    void Debounce ()
    {
        transform.DOScale(1f , .15f).SetUpdate(true);
        if (!isSpinButton)
        {
            EnableButtonInteractvity();
        }
    }
}
