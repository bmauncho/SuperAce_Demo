using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (CommandCentre.Instance)
        {
            if(!CommandCentre.Instance.GridManager_.IsGridCreationComplete())
            {
                DisableButtonInteractvity();
            }
            else
            {
                EnableButtonInteractvity();
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
