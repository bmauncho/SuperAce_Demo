using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    public bool isSpinButton = false;
    // Update is called once per frame
    void Update()
    {
        if (CommandCentre.Instance)
        {
            if (isSpinButton)
            {
                if (CommandCentre.Instance.WinLoseManager_.enableSpin)
                {
                    //delay 
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

    void EnableButtonInteractvity ()
    {
        CommandCentre.Instance.MainMenuController_.isBtnPressed = false;
        GetComponent<Button>().interactable = true;
    }
    public void DisableButtonInteractvity ()
    {
        GetComponent<Button>().interactable = false;
    }

    public void BounceBtn ()
    {
        if (this.gameObject.activeSelf)
        {
            Bounce();
            if (CommandCentre.Instance)
            {
                CommandCentre.Instance.SoundManager_.PlaySound("ButtonPress" , false , .75f);
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
    }
}
