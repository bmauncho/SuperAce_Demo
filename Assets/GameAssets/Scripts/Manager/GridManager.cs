using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridColumns
{
    public List<GameObject> Cards = new List<GameObject>();
}

public class GridManager : MonoBehaviour
{
    MultiDeckManager multiDeckManager;
    PoolManager poolManager;
    public GameObject CardsParent;
    public float moveDuration = 0.5f;
    public float delayBetweenMoves = 0.1f;
    public int totalObjectsToPlace;
    private int objectsPlaced;
    public bool IsResetDone;
    public bool IsReturnToPoolDone = false;
    public List<GridColumns> Columns = new List<GridColumns>();

    private void Start ()
    {
        poolManager = CommandCentre.Instance.PoolManager_;
        multiDeckManager = CommandCentre.Instance.MultiDeckManager_;
    }

    public void ManualStart ()
    {
        CreateGrid();
    }

    [ContextMenu("Reset Grid")]
    public void ResetGrid ()
    {
        IsResetDone = false; // Indicate reset is in progress
        IsReturnToPoolDone = false;

        // Clear all columns
        for (int i = 0 ; i < Columns.Count ; i++)
        {
            if (Columns [i].Cards.Count >= 0)
            {
                Columns [i].Cards.Clear();
            }
        }

        // Start the grid refresh
        StartCoroutine(Refresh());

        // Refill decks from pool
        foreach (Deck deck in multiDeckManager.decks)
        {
            deck.RefillDeckFromPool();
        }
    }

    [ContextMenu("Create Grid")]
    public void CreateGrid ()
    {
        Debug.Log("create grid");

        Deck [] decks = multiDeckManager.decks;
        totalObjectsToPlace = decks.Length * 4;
        objectsPlaced = 0;

        List<Transform> tempPos = new List<Transform>();
        if (tempPos.Count > 0) { tempPos.Clear(); }

        foreach (Transform tr in CardsParent.transform)
        {
            if (!tr.GetComponent<CardPos>().TheOwner)
            {
                tempPos.Add(tr);
            }
        }

        int deckCount = decks.Length;

        if (tempPos.Count < totalObjectsToPlace)
        {
            Debug.LogError("Not enough available positions to place all the cards.");
            return;
        }

        for (int col = 0 ; col < deckCount ; col++)
        {
            Deck currentDeck = decks [col];

            for (int row = 0 ; row < 4 ; row++)
            {
                if (currentDeck == null || currentDeck.DeckCards.Count == 0)
                {
                    Debug.LogWarning("Deck is empty or null. Skipping.");
                    break;
                }

                GameObject card = currentDeck.DrawCard();
                if (card == null)
                {
                    Debug.LogWarning("Card is null. Skipping.");
                    break;
                }

                Columns [col].Cards.Add(card);

                Transform targetPos = tempPos [( col * 4 ) + row];
                card.transform.SetParent(targetPos);
                card.transform.rotation = Quaternion.Euler(0 , 180f , 0);
                card.transform.DOMove(targetPos.position , moveDuration)
                    .SetEase(Ease.OutQuad)
                    .SetDelay(( row * delayBetweenMoves ) + ( col * 4 * delayBetweenMoves ))
                    .OnComplete(() =>
                    {
                        targetPos.GetComponent<CardPos>().TheOwner = card;
                        CalculateObjectsPlaced();
                    });
                CommandCentre.Instance.SoundManager_.PlaySound("Card" , false , .75f);
                currentDeck.DeckCards.Remove(card);
            }
        }
    }

    void returnToPool ()
    {
        Debug.Log("Returning cards to pool");

        List<Transform> tempPos = new List<Transform>();
        if (tempPos.Count > 0) { tempPos.Clear(); }

        foreach (Transform tr in CardsParent.transform)
        {
            if (tr.GetComponentInChildren<Card>()||tr.GetComponent<CardPos>().TheOwner)
            {
                tempPos.Add(tr);
            }
        }

        foreach (Transform tr in tempPos)
        {
            if (tr.GetComponent<CardPos>().TheOwner)
            {
                poolManager.ReturnCard(tr.GetComponent<CardPos>().TheOwner);
                tr.GetComponent<CardPos>().TheOwner = null;
            }
        }

        IsReturnToPoolDone = true;
    }

    IEnumerator Refresh ()
    {
        MoveGrid(new Vector3(0 , -43f , 0));
        yield return null;
    }

    void CalculateObjectsPlaced ()
    {
        objectsPlaced++;
        if (IsGridCreationComplete())
        {
            CommandCentre.Instance.WinLoseManager_.PopulateGridChecker(CardsParent.transform);
        }
    }

    public void MoveGrid ( Vector3 direction )
    {
        Vector3 originalPosition = CardsParent.transform.localPosition;
        StartCoroutine(GridMovement(direction , originalPosition));
    }

    IEnumerator GridMovement ( Vector3 Dir , Vector3 OriginalPos )
    {
        Tween myTween = CardsParent.transform.DOLocalMove(CardsParent.transform.localPosition + Dir , 1f);
        yield return myTween.WaitForCompletion();
        returnToPool();
        CardsParent.transform.localPosition = OriginalPos;
        IsResetDone = true;
        yield return new WaitForSeconds(.5f);
        CreateGrid();
    }

    public bool IsGridCreationComplete ()
    {
        return objectsPlaced >= totalObjectsToPlace;
    }

    private int DetermineColumnIndex ( Transform cardTransform )
    {
        for (int i = 0 ; i < Columns.Count ; i++)
        {
            GridColumns column = Columns [i];

            foreach (GameObject card in column.Cards)
            {
                if (card.transform == cardTransform)
                {
                    return i;
                }
            }
        }

        Debug.LogWarning("Card not found in any column.");
        return -1;
    }
}