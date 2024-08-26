using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Deck : MonoBehaviour
{
    PoolManager poolManager;
    [Header("Variables")]
    public bool CanHaveGoldenCard = false;
    public int cardsPerDeck = 10;
    public int CardSortPosCounter;
    public int DefaultCardSortCount = 5;
    public Vector3 cardOffset = new Vector3(0 , -0.2f , 0);
    public Vector3 OriginalPos;
    [Space(10)]
    [Header("Lists")]
    public List<GameObject> DeckCards = new List<GameObject>();
    //public List<Transform> DeckCardsPos = new List<Transform>();

    private void Start ()
    {
        poolManager = CommandCentre.Instance.PoolManager_;
    }

    private void Update ()
    {
        refilldeck();
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
        if (DeckCards.Count <= 0) { CardSortPosCounter = DefaultCardSortCount; }

        int currentCardCount = DeckCards.Count;
        int cardsNeeded = cardsPerDeck - currentCardCount;
        if (currentCardCount > cardsPerDeck)
        {
            for (int i = 0 ; i < currentCardCount - cardsPerDeck ; i++)
            {
                Transform card = transform.GetComponentInChildren<Card>().transform;
                poolManager.ReturnCard(card.gameObject);
                DeckCards.Remove(card.gameObject);
                CardSortPosCounter--;
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
                    UpdateCardSortPositions(card);
                    card.transform.SetParent(transform);
                    card.transform.localPosition = Vector3.zero;
                    //card.transform.localPosition = new Vector3 (0,7,0);
                    card.transform.localRotation = Quaternion.Euler(0 , 0 , 0);
                    card.SetActive(true); 
                    card.GetComponent<Card>().SetCardSortPos(CardSortPosCounter);
                    CardSortPosCounter++;
                }
            }
        }
    }
    //IEnumerator ShuffleDeck ()
    //{

    //}
    public GameObject DrawCard ()
    {
        Transform card = null;
        if (DeckCards.Count > 0)
        {
            
            if (checkifCardsAreInTheDeckObj())
            {
                Debug.Log($"Checkif cards are in the deck:{checkifCardsAreInTheDeckObj()}");
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
            card.transform.localRotation = Quaternion.Euler(0 , 0 , 0);
            CommandCentre.Instance.CardManager_.GetAndAssignSprites(card);
            CardSortPosCounter--;
            return card.gameObject;
        }
        
        return null;
    }

    public void RefreshCardPos ()
    {
        
    }
    public void UpdateCardSortPositions ( GameObject newCard )
    {
        Card card = DeckCards [DeckCards.Count - 1].GetComponent<Card>();
        int newSortPos = card.CardSortPos + 1;
        for (int i = 0 ; i < DeckCards.Count ; i++)
        {
            if(card != null)
            {
                newCard.GetComponent<Card>().SetCardSortPos(newSortPos);
            }
        }
    }

    public bool CheckifDeckHasCards ()
    {
        List<Transform> cards = new List<Transform>();
        if(cards.Count > 0) {cards.Clear();}
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

    public void RemnoveCardFromDeck ()
    {
        DeckCards.RemoveAt(DeckCards.Count-1);
        CardSortPosCounter--;
        if(CardSortPosCounter <= 5) { CardSortPosCounter = DefaultCardSortCount; }
    }

    void refilldeck ()
    {
        switch (CheckifDeckHasCards())
        { 
            case true:
                //Debug.Log($"Does it have cards:{CheckifDeckHasCards()}");
                if (HasEnoughCards())
                {
                   // Debug.Log($"Does it have enough cards:{CheckifDeckHasCards()}");
                    break;
                }
                else
                {
                    RefillDeckFromPool();
                }
                break;
            case false:
                RefillDeckFromPool();
                break;
        }
    }
}
