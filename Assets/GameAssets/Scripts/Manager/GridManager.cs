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
                    .SetDelay(( row * delayBetweenMoves ) + ( col *4 * delayBetweenMoves ))
                    .OnComplete(() =>
                    {
                        targetPos.GetComponent<CardPos>().TheOwner = card;
                        CalculateObjectsPlaced();
                    });
                //CommandCentre.Instance.SoundManager_.PlaySound("Card" , false , .75f);
                currentDeck.RemnoveCardFromDeck();
            }
        }
    }

    void returnToPool ()
    {
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
            if (tr)
            {
                poolManager.ReturnCard(tr.GetComponent<CardPos>().TheOwner);
                if (tr.GetComponent<CardPos>().GetComponentInChildren<Card>())
                {
                    poolManager.ReturnCard(tr.GetComponent<CardPos>().GetComponentInChildren<Card>().gameObject);
                }
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
