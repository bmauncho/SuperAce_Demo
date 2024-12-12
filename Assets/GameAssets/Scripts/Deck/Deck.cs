using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    PoolManager poolManager;
    public bool isDeckFilled = false;
    public int cardsPerDeck = 10;
    public int tempDeckSize = 5;
    public Vector3 cardOffset = new Vector3(0 , -0.2f , 0);
    public List<GameObject> tempDeckCards = new List<GameObject>();
    public List<GameObject> DeckCards = new List<GameObject>();
    public List<Vector3> cardPositions = new List<Vector3>();
    bool IsPositionsaved = false;
    private bool isMaintainingDeck = false;

    private void Start ()
    {
        poolManager = CommandCentre.Instance.PoolManager_;
    }

    private void Update ()
    {
        if (CommandCentre.Instance)
        {
            if (!isDeckFilled && poolManager.Poolinitialized)
            {
                refillTempDeckFromPool();
            }
        }
        RepositionCards();
    }

    public void ResetDeck ()
    {
        isDeckFilled = false; // Reset the flag when you need to refill the deck again
    }

    public void refillTempDeckFromPool ()
    {
        // Return excess cards to the pool
        if (tempDeckCards.Count > tempDeckSize)
        {
            for (int i = tempDeckCards.Count - 1 ; i >= tempDeckSize ; i--)
            {
                if (tempDeckCards [i] != null)
                {
                    poolManager.ReturnCard(tempDeckCards [i]);
                }
                tempDeckCards.RemoveAt(i); // Remove the card from the temp deck
            }
        }

        // Add new cards to fill the temp deck
        while (tempDeckCards.Count < tempDeckSize)
        {
            GameObject newCard = poolManager.GetCard();
            if (newCard != null)
            {
                newCard.transform.localPosition = new Vector3(0 , 7 , 0);
                tempDeckCards.Add(newCard);
                newCard.transform.SetParent(transform);
            }
            else
            {
                Debug.LogWarning("Failed to get a new card from the pool!");
                break;
            }
        }

        // Proceed to fill the main deck
        fillDeck();
    }


    public void fillDeck ()
    {
        if (!isDeckFilled)
        {
            if (tempDeckCards.Count == 0)
            {
                Debug.LogWarning("TempDeckCards is empty before update!");
                return;
            }

            Sequence deckSequence = DOTween.Sequence();
            for (int i = 0 ; i < tempDeckCards.Count ; i++)
            {
                GameObject newCard = tempDeckCards [i];
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
                tempDeckCards.Clear();
            });

            isDeckFilled = true;
            IsPositionsaved = true;
        }
    }

    private IEnumerator ReturnExcessCards ( List<GameObject> excessCards )
    {
        foreach (GameObject card in excessCards)
        {
            poolManager.ReturnCard(card);
            yield return null; // Return control to the main thread
        }
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

    public GameObject DrawCard ()
    {
        if (DeckCards.Count <= 0)
        {
            ResetDeck();
        }

        if(DeckCards.Count > 0)
        {
            GameObject newCard = DeckCards [0];
            if (!newCard)
            {
                DrawCard();
            }
            DeckCards.RemoveAt (0);
            newCard.transform.SetParent(null);
            newCard.SetActive(true);
            newCard.transform.localRotation = Quaternion.Euler(0 , 180f , 0);
            return newCard;
        }
        return null;
    }
}
