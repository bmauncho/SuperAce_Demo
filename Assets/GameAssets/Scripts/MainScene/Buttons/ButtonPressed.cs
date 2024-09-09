using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    public bool isSpinButton = false;
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
                    if (CommandCentre.Instance.GridColumnManager_.IsGridRepositioningComplete())
                    {
                        if (CommandCentre.Instance.GridColumnManager_.CardsToShake().Count > 0)
                        {
                            if(CommandCentre.Instance.GridColumnManager_.IsObjectShakedComplete()
                                && CommandCentre.Instance.GridColumnManager_.IsObjectsJumpComplete())
                            {
                                EnableButtonInteractvity();
                            }
                        }
                        else if(CommandCentre.Instance.GridColumnManager_.CardsToShake().Count <= 0)
                        {
                            EnableButtonInteractvity();
                        }
                    }
                    else
                    {
                        DisableButtonInteractvity ();
                    }
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
            DisableButtonInteractvity() ;
            if (CommandCentre.Instance)
            {
                //CommandCentre.Instance.SoundManager_.PlaySound("ButtonPress" , false , .75f);
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
