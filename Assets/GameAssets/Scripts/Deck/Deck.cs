using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using DG.Tweening;
using System;

public class Deck : MonoBehaviour
{
    PoolManager poolManager;
    [Header("Variables")]
    public bool CanHaveGoldenCard = false;
    public bool deckRefilled = false; // Flag to track if the deck has been refilled
    private bool isMaintainingDeck = false;
    public int cardsPerDeck = 10;
    public int tempDeckSize = 5;
    public Vector3 cardOffset = new Vector3(0 , -0.2f , 0);
    public Vector3 OriginalPos;
    [Space(10)]
    [Header("Lists")]
    public List<GameObject> DeckCards = new List<GameObject>();
    public List<GameObject> TempDeckCards = new List<GameObject>(); // Temporary deck list
    public List<Vector3> cardPositions = new List<Vector3>();
    bool IsPositionsaved = false;

    private void Start ()
    {
        poolManager = CommandCentre.Instance.PoolManager_;
    }

    private void Update ()
    {
        if (CommandCentre.Instance)
        {
            if (poolManager.Poolinitialized && !deckRefilled)
            {
                RefillTempDeckFromPool();
            }
        }
        MaintainCorrectAmountOfCardsInDeck();
        RepositionCards();
    }

    public void ClearDeck ()
    {
        
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Card>())
            {
                CommandCentre.Instance.PoolManager_.ReturnCard(child.gameObject);
            }
        }
        DeckCards.Clear();
    }

    public void RefillTempDeckFromPool ()
    {
        if (TempDeckCards.Count < tempDeckSize)
        {
            foreach(GameObject card in TempDeckCards)
            {
                if (card)
                {
                    poolManager.ReturnCard(card);
                }
            }
            TempDeckCards.Clear(); // Clear the temporary deck

            for (int i = 0 ; i < tempDeckSize ; i++)
            {
                GameObject newCard = poolManager.GetCard();
                if (newCard != null)
                {
                    newCard.transform.localPosition = new Vector3(0 , 7 , 0);
                    TempDeckCards.Add(newCard);
                    newCard.transform.SetParent(transform);
                }
            }
            //Debug.Log("Refill complete.");
            UpdateDeckFromTempDeck();
        }
    }

    public void UpdateDeckFromTempDeck ()
    {
        if (!deckRefilled)
        {
            //Debug.Log("Refilling Deck - TempDeckCards count: " + TempDeckCards.Count);

            if (TempDeckCards.Count == 0)
            {
                Debug.LogWarning("TempDeckCards is empty before update!");
                return;
            }

            Sequence deckSequence = DOTween.Sequence();
            for (int i = 0 ; i < tempDeckSize ; i++)
            {
                GameObject newCard = TempDeckCards [i];
                if (newCard != null)
                {
                    DeckCards.Add(newCard);
                    Vector3 targetOffset = Vector3.zero + new Vector3(0 , cardOffset.y * i , cardOffset.z * i);
                    deckSequence.Append(newCard.transform.DOLocalMove(targetOffset , 0.25f).SetEase(Ease.OutQuad));

                    if (!IsPositionsaved)
                    {
                        cardPositions.Add(targetOffset);
                    }
                }
                else
                {
                    Debug.LogWarning("Card at index " + i + " is null in TempDeckCards.");
                }
            }

            deckSequence.OnComplete(() =>
            {
                TempDeckCards.Clear();
               // Debug.Log("Deck refilled successfully, TempDeckCards cleared.");
            });

            deckRefilled = true;
            IsPositionsaved = true;
        }
        else
        {
            //Debug.Log("Deck is already refilled.");
        }
    }


    public void ResetDeck ()
    {
        deckRefilled = false; // Reset the flag when you need to refill the deck again
    }

    public GameObject DrawCard ()
    {
        if (DeckCards.Count <= 0)
        {
            ResetDeck();
        }

        if (DeckCards.Count > 0)
        {
            GameObject drawnCard = DeckCards [0];
            DeckCards.RemoveAt(0);

            // Set the card and background
            if (CanHaveGoldenCard)
            {
                // Randomize between golden and normal card
                CommandCentre.Instance.CardManager_.RandomizeDealing(drawnCard.transform);
                //CommandCentre.Instance.CardManager_.RandomizeDealing_golden(drawnCard.transform);
            }
            else
            {
                CommandCentre.Instance.CardManager_.RandomizeDealing(drawnCard.transform);
                //CommandCentre.Instance.CardManager_.RandomizeDealing_Scatter(drawnCard.transform);
            }

            drawnCard.transform.SetParent(null);
            drawnCard.SetActive(true);
            drawnCard.transform.localRotation = Quaternion.Euler(0 , 0 , 0);
            //CommandCentre.Instance.CardManager_.GetAndAssignSprites(drawnCard.transform);
            return drawnCard;
        }

        return null;
    }

    public GameObject DrawSpecificCard (int col)
    {
        if (DeckCards.Count <= 0)
        {
            ResetDeck();
        }

        if (DeckCards.Count > 0)
        {
            GameObject drawnCard = DeckCards [0];
            DeckCards.RemoveAt(0);
            CommandCentre.Instance.CardManager_.DealSpecificCardType(drawnCard.transform , col);
            drawnCard.transform.SetParent(null);
            drawnCard.SetActive(true);
            drawnCard.transform.localRotation = Quaternion.Euler(0 , 0 , 0);
            CommandCentre.Instance.CardManager_.GetAndAssignSprites(drawnCard.transform);
            return drawnCard;
        }
        return null;
    }

    public void RepositionCards ()
    {
        if (DeckCards.Count == 0 || DeckCards.Count > cardsPerDeck) return;

        for (int i = 0 ; i < DeckCards.Count ; i++)
        {
            if (i < cardPositions.Count)
            {
                Vector3 targetPosition = cardPositions [i];
                DeckCards [i].transform.localPosition = targetPosition;
            }
        }
    }
    public void MaintainCorrectAmountOfCardsInDeck ()
    {
        if (isMaintainingDeck || DeckCards.Count <= cardsPerDeck) return;

        isMaintainingDeck = true;

        List<GameObject> excessCards = new List<GameObject>();

        // Set a safety limit for the loop
        int safetyCounter = 10; // or any appropriate limit

        while (DeckCards.Count > cardsPerDeck && safetyCounter > 0)
        {
            GameObject excessCard = DeckCards [DeckCards.Count - 1];
            DeckCards.RemoveAt(DeckCards.Count - 1);
            excessCards.Add(excessCard);

            safetyCounter--; // Decrease safety counter
        }

        if (safetyCounter == 0)
        {
            Debug.LogWarning("MaintainCorrectAmountOfCardsInDeck exited due to safety limit.");
        }

        StartCoroutine(ReturnExcessCards(excessCards));

        isMaintainingDeck = false;
    }

    private IEnumerator ReturnExcessCards ( List<GameObject> excessCards )
    {
        foreach (GameObject card in excessCards)
        {
            poolManager.ReturnCard(card);
            yield return null; // Return control to the main thread
        }
    }

    public bool CheckIfDeckHasCards ()
    {
        foreach (Transform card in transform)
        {
            if (card.GetComponent<Card>() != null)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasEnoughCards ()
    {
        return DeckCards.Count >= cardsPerDeck;
    }

    bool CheckIfCardsAreInTheDeckObj ()
    {
        // This function's logic is essentially the same as CheckIfDeckHasCards.
        return CheckIfDeckHasCards();
    }

    public void CheckAndResetDeck ()
    {
        if (!HasEnoughCards())
        {
            Debug.Log("Doesnt have enough Cards");
            if (!CheckIfDeckHasCards())
            {
                Debug.Log("Has no cards");
                ResetDeck();

            }
            else
            {
                Debug.Log("have cards but not enough Cards");
                ResetDeck();
            }
        }
    }

}
