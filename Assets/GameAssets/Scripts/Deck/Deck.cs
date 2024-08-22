using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    public int cardsPerDeck = 10;
    public Vector3 cardOffset = new Vector3(0 , -0.2f , 0);
    PoolManager poolManager;
    public List<GameObject> DeckCards = new List<GameObject>();
    public Vector3 OriginalPos;
    public bool CanHaveGoldenCard = false;  

    private void Start ()
    {
        poolManager = CommandCentre.Instance.PoolManager_;
    }

    private void Update ()
    {
        if (HasEnoughCards())
        {
            return;
        }
        else
        {
            RefillDeckFromPool();
        }
    }

    public void ClearDeck ()
    {
        if(DeckCards.Count > 0) {DeckCards.Clear(); }
        foreach(Transform child in transform)
        {
            if (child.GetComponent<Card>())
            {
                CommandCentre.Instance.PoolManager_.ReturnCard(child.gameObject);
                DeckCards.Remove(child.gameObject);
            }
        }
    }

    public void RefillDeckFromPool ()
    {
        int currentCardCount = DeckCards.Count;
        int cardsNeeded = cardsPerDeck - currentCardCount;
        if (currentCardCount > cardsPerDeck)
        {
            for (int i = 0 ; i < currentCardCount - cardsPerDeck ; i++)
            {
                Transform card = transform.GetComponentInChildren<Card>().transform;
                poolManager.ReturnCard(card.gameObject);
                DeckCards.Remove(card.gameObject);
            }
        }
        else if (currentCardCount < cardsPerDeck)
        {
            for (int i = 0 ; i < cardsNeeded ; i++)
            {
                GameObject card = poolManager.GetCard();
                if (card != null)
                {
                    card.GetComponent<Card>().SetCard(null);
                    DeckCards.Add(card);
                    card.transform.SetParent(transform);
                    card.transform.localPosition = Vector3.zero;
                    card.SetActive(true); 
                }
            }
        }
    }

    public GameObject DrawCard ()
    {
        if (DeckCards.Count > 0)
        {
            Transform card = null;
            if (checkifCardsAreInTheDeckObj())
            {
                RefillDeckFromPool();
                card = transform.GetComponentInChildren<Card>().transform;
            }
            else
            {
                ClearDeck();
                RefillDeckFromPool();
                card = transform.GetComponentInChildren<Card>().transform;
            }

            //set the card and background

            if (CanHaveGoldenCard)
            {
                //randomize between golden and normal card
                CommandCentre.Instance.CardManager_.RandomizeDealing_golden(card);
            }
            else
            {
                CommandCentre.Instance.CardManager_.RandomizeDealing_Scatter(card);
            }
            card.SetParent(null);
            card.gameObject.SetActive(true);
            CommandCentre.Instance.CardManager_.GetAndAssignSprites(card);
            return card.gameObject;
        }
        return null;
    }


    public bool CheckifDeckHasCards ()
    {
        List<Transform> cards = new List<Transform>();
        foreach (Transform card in transform)
        {
           cards.Add( card );
        }
        if (cards.Count > 0)
        {
            for (int i = 0 ; i < cards.Count ; i++)
            {
                if (cards [i].GetComponent<Card>())
                {
                    return true;
                }
            }
        }
        else
        {
            return false;
        }
        return false;
    }

    public bool HasEnoughCards ()
    {
        int currentCardCount = DeckCards.Count;
        int cardsNeeded = cardsPerDeck - currentCardCount;
        if (currentCardCount >= cardsPerDeck)
        {
            return true;
        }
        else 
        {
            return false ;
        }
    }

    bool checkifCardsAreInTheDeckObj ()
    {
        foreach(Transform tr in transform)
        {
            if(tr.GetComponent<Card>() != null)
            {
                return true;
            }
        }
        return false;
    }

    public void RemnoveCardFromDeck (GameObject Card)
    {
        DeckCards.Remove(Card);
    }


}
