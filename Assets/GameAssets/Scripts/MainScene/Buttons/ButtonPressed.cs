using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    CommandCentre commandCentre;
    public bool isSpinButton = false;
    public bool IsToggle = false;
    public bool IsContinuebutton = false;
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
                // if the win or card sequence is complete enable
                if (commandCentre.GridManager_.isGridFilled())
                {
                    EnableButtonInteractvity();
                    commandCentre.MainMenuController_.CanSpin = true;
                }
                else
                {
                    DisableButtonInteractvity ();
                    commandCentre.MainMenuController_.CanSpin = false;
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
            CommandCentre.Instance.MainMenuController_.isBtnPressed = false;
            GetComponent<Toggle>().interactable = true;
        }
        else
        {
            CommandCentre.Instance.MainMenuController_.isBtnPressed = false;
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
            Bounce();
            DisableButtonInteractvity() ;
            if (CommandCentre.Instance)
            {
                CommandCentre.Instance.SoundManager_.PlaySound("BtnClick" , false , .3f);
            }
        }
    }

    void Bounce ()
    {
        transform.DOScale(1.1f , 3 *Time.deltaTime).SetUpdate(true).OnComplete(Debounce); ;
    }

    void Debounce ()
    {
        transform.DOScale(1f , .25f).SetUpdate(true);
        if (!isSpinButton)
        {
            EnableButtonInteractvity();
        }
    }
}
