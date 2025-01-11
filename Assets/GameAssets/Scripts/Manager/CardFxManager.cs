using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardfxColumn
{
    public List<GameObject> cardFxPos = new List<GameObject>();
}

public class CardFxManager : MonoBehaviour
{
    public GameObject cardFxMask;
    public GameObject CardFxHolder;
    public List<CardfxColumn> CardFx = new List<CardfxColumn>();
    public List<CardfxColumn> CardFxMask = new List<CardfxColumn>();

    [ContextMenu("Activate")]
    public void Activate ()
    {
        //Debug.Log("activateMasks");
        CardFxHolder.SetActive(true);
    }

    public void Deactivate ()
    {
        CardFxHolder.SetActive(false);
    }
    [ContextMenu("Activate Mask")]
    public void ActivateCardMask ()
    {
        cardFxMask.SetActive(true);
    }
    [ContextMenu("Deactivate Mask")]
    public void DeactivateCardMask ()
    {
        cardFxMask.SetActive(false);
    }
   

    public void ReturnToPool ()
    {
        foreach (CardfxColumn c in CardFx)
        {
           foreach(GameObject obj in c.cardFxPos)
            {
                if (obj != null)
                {
                    Cardfx thefx = obj.GetComponentInChildren<Cardfx>();
                    if (thefx)
                    {
                        CommandCentre.Instance.PoolManager_.ReturnFx(thefx.gameObject);
                    }
                }
            }
        }
    }

    public void ReturnScatterCardsToPool ()
    {
        foreach (CardfxColumn c in CardFx)
        {
            foreach (GameObject obj in c.cardFxPos)
            {
                if (obj != null)
                {
                    ScatterCardFx thefx = obj.GetComponentInChildren<ScatterCardFx>();
                    if (thefx)
                    {
                        CommandCentre.Instance.PoolManager_.ReturnFx(thefx.gameObject);
                    }
                }
            }
        }
    }

    [ContextMenu("activate card Masks")]
    void test ()
    {
        ActivateCardMask();
        ActivateAllCardFxMask();
    }

    public void ActivateAllCardFxMask ()
    {
        for (int i = 0 ; i < 5 ; i++)
        {
            for (int j = 4 - 1 ; j >= 0 ; j--)
            {
                CardFxMask [j].cardFxPos [i].SetActive(true);
            }
        }
    }


    public void ActivateCardFxMask(int whichrow, int whichcol )
    {
        for (int i = 0;i<5 ;i++)
        {
            for(int j = 4-1;j>=0 ;j--)
            {
                if(CardFxMask [whichrow].cardFxPos [whichcol] == CardFxMask [j].cardFxPos [i])
                {
                    CardFxMask [whichrow].cardFxPos [whichcol].SetActive(true);
                }
            }
        }
    }

    [ContextMenu("Deactivate card Masks")]
    public void DeactivateCardFxMask ()
    {
        DeactivateCardMask ();
        for (int i = 0 ; i < 5 ; i++)
        {
            for (int j = 4 - 1 ; j >= 0 ; j--)
            {
                CardFxMask [j].cardFxPos [i].SetActive(false);
            }
        }
    }
}
