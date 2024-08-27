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
    [Header("Refrences")]

    MultiDeckManager multiDeckManager;
    PoolManager poolManager;
    public GameObject CardsParent;

    [Space(10)]
    [Header("Variables")]
    public float moveDuration = 0.5f;
    public float delayBetweenMoves = 0.1f;
    public int totalObjectsToPlace;
    private int objectsPlaced;
    public bool IsResetDone;
    public bool IsReturnToPoolDone = false;
    public bool IsFirstTime = true;
    
    [Header("Lists")]
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
        foreach (GridColumns column in Columns)
        {
            column.Cards.Clear();
        }

        // Start the grid refresh after clearing columns
        MoveGrid(new Vector3(0 , -43f , 0));
    }

    [ContextMenu("Create Grid")]
    public void CreateGrid ()
    {
        Deck [] decks = multiDeckManager.decks;
        totalObjectsToPlace = decks.Length * 4;
        objectsPlaced = 0;

        List<Transform> tempPos = new List<Transform>();

        // Collect available positions
        foreach (Transform tr in CardsParent.transform)
        {
            if (tr.GetComponent<CardPos>()?.TheOwner == null)
            {
                tempPos.Add(tr);
            }
        }

        if (tempPos.Count < totalObjectsToPlace)
        {
            Debug.LogError("Not enough available positions to place all the cards.");
            return;
        }

        float delayIncrement = 0.1f; // Delay between cards, adjust as needed
        int rowCount = 4; // Number of rows
        int columnCount = decks.Length; // Number of columns

        int positionIndex = 0; // Keeps track of the current position in tempPos
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = 0 ; row < rowCount ; row++)
            {
                Deck currentDeck = decks [col];
                if (currentDeck == null || currentDeck.DeckCards.Count == 0) continue;

                GameObject card = currentDeck.DrawCard();
                currentDeck.ResetDeck();
                if (card == null) continue;

                Columns [col].Cards.Add(card);

                Transform targetPos = tempPos [positionIndex];
                positionIndex++;

                card.transform.SetParent(targetPos);
                card.transform.rotation = Quaternion.Euler(0 , 180f , 0);

                // Create a sequence for each card's movement
                Sequence cardSequence = DOTween.Sequence();
                cardSequence.Append(card.transform.DOLocalMove(Vector3.zero , moveDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        // Set local position to zero explicitly after the animation
                        card.transform.localPosition = Vector3.zero;
                        targetPos.GetComponent<CardPos>().TheOwner = card;
                        CalculateObjectsPlaced();
                    }));
                cardSequence.PrependInterval(( col * rowCount + row ) * delayIncrement); // Delay based on column and row position
            }
        }

    }

    void returnToPool ()
    {
        List<Transform> tempPos = new List<Transform>();

        foreach (Transform tr in CardsParent.transform)
        {
            var cardPos = tr.GetComponent<CardPos>();
            if (cardPos && ( cardPos.TheOwner || tr.GetComponentInChildren<Card>() ))
            {
                tempPos.Add(tr);
            }
        }

        foreach (Transform tr in tempPos)
        {
            if (tr)
            {
                var cardPos = tr.GetComponent<CardPos>();
                if (cardPos)
                {
                    var card = cardPos.TheOwner;
                    if (card)
                    {
                        poolManager.ReturnCard(card);
                    }
                    else
                    {
                        Debug.LogWarning($"CardPos found but TheOwner is null: {tr.name}");
                    }
                    var cardInChildren = tr.GetComponentInChildren<Card>();
                    if (cardInChildren)
                    {
                        poolManager.ReturnCard(cardInChildren.gameObject);
                    }
                    cardPos.TheOwner = null;
                }
                else
                {
                    Debug.LogWarning($"Transform does not have CardPos: {tr.name}");
                }
            }
        }

        IsReturnToPoolDone = true;
        Debug.Log("Return to pool process completed.");
    }


    IEnumerator Refresh ()
    {
        MoveGrid(new Vector3(0 , -43f , 0));
        yield return null;
    }

    void CalculateObjectsPlaced ()
    {
        objectsPlaced++;
        if (IsFirstTime)
        {
            return;
        }
        CheckGrid();
    }

    public void CheckGrid ()
    {
        if (IsGridCreationComplete())
        {
            multiDeckManager.refillAllDecks();
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
        Tween myTween = CardsParent.transform.DOLocalMove(CardsParent.transform.localPosition + Dir , .25f);
        yield return myTween.WaitForCompletion();
        returnToPool();
        CardsParent.transform.localPosition = OriginalPos;
        IsResetDone = true;
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

    public void RefreshCurrentColumnCards (int colIndex,GameObject newCard)
    {
        for (int j = 0 ; j < Columns [colIndex].Cards.Count ; j++)
        {
            if (!Columns [colIndex].Cards [j].activeSelf)
            {
                newCard.GetComponent<Card>().CardSortPos = 
                    Columns [colIndex].Cards [j].GetComponent<Card>().CardSortPos;
                Columns [colIndex].Cards [j] = newCard;
                
            }
        }
    }
}
