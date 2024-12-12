using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NormalGamePlay : MonoBehaviour
{
    public GameObject currentWinnings;
    public float BounceAmt;
    public GameObject [] gamePlayButtons;

    public void Bounce ()
    {
        currentWinnings.transform.DOScale(BounceAmt , .25f).OnComplete(() =>
        {
            Debounce();
        });
    }

    public void Debounce ()
    {
        currentWinnings.transform.DOScale(1 , .25f);
    }

    public void setButtonsInteractivity (bool isActive)
    {
        for(int i = 0; i < gamePlayButtons.Length; i++)
        {
            if (gamePlayButtons [i].GetComponent<Button>())  
            {
                gamePlayButtons [i].GetComponent<Button>().interactable = isActive;
               
            }
            else if(gamePlayButtons [i].GetComponent<Toggle>())
            {
                gamePlayButtons [i].GetComponent<Toggle>().interactable = isActive;
            }
        }
    }
}
