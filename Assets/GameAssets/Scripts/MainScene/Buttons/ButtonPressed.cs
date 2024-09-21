using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    public bool isSpinButton = false;
    public bool IsToggle = false;
    public bool IsContinuebutton = false;
    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (CommandCentre.Instance)
        {
            if (isSpinButton)
            {
                if (CommandCentre.Instance.WinLoseManager_.enableSpin)
                {
                    EnableButtonInteractvity();
                    //delay 
                }
                else
                {
                    if (CommandCentre.Instance.MainMenuController_.isBtnPressed)
                    {
                        DisableButtonInteractvity();
                    }
                    else
                    {
                        DisableButtonInteractvity();
                    }
                    
                }
            }
            else
            {
                if (CommandCentre.Instance.WinLoseManager_.enableSpin)
                {
                    EnableButtonInteractvity();
                    //delay 
                }
                else
                {
                    if (IsContinuebutton)
                    {
                        EnableButtonInteractvity();
                    }
                    else
                    {
                        if (CommandCentre.Instance.MainMenuController_.isBtnPressed)
                        {
                            DisableButtonInteractvity();
                        }
                        else
                        {
                            DisableButtonInteractvity();
                        }
                    }

                }
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
        transform.DOScale(1.1f , 2f * Time.deltaTime).SetUpdate(true).OnComplete(Debounce); ;
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
