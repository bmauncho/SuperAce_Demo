using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using System.Linq;
[System.Serializable]
public class  GridPosColumns
{
    public List<GameObject> CardsPos = new List<GameObject>();
}
public class GridColumnManager : MonoBehaviour
{
    private MultiDeckManager multiDeckManager;
    public bool IsDoneCheckingWin;
    public bool isWinChecked = false;
    public bool [] refillColumnCompleted;
    public bool [] columnsToRefill;
    public bool isRepositioning;

    public GameObject CardPosHolders;
    public List<GameObject> CardList = new List<GameObject>();
    public List<GridPosColumns> Columns = new List<GridPosColumns>();
    private void Start ()
    {
        multiDeckManager = CommandCentre.Instance.MultiDeckManager_;
        refillColumnCompleted = new bool [5];
        GetCardPositions();
    }


    void GetCardPositions ()
    {
        foreach(Transform tr in CardPosHolders.transform)
        {
            CardList.Add(tr.gameObject);
        }
    }
    public void CheckAndFillColumns (int No_Of_Columns)
    {
        refreshAllRefillColumnsCompleted ();
        isWinChecked = false;
        IsDoneCheckingWin = false;
        // Get the number of columns and rows
        int columns = No_Of_Columns;
        InitializeRefillTracking(No_Of_Columns);
        // Iterate through each column
        StartCoroutine(ProcessColumnsSequentially(columns));
    }
    private IEnumerator ProcessColumnsSequentially ( int columns )
    {
        for (int col = 0 ; col <= columns-1; col++)
        {
            // Process each column one after the other
            yield return StartCoroutine(HandleDisabledCardInColumn(col));
        }

        // After all columns are processed, check win conditions
        if (!isWinChecked)
        {
            isWinChecked = true;
            IsDoneCheckingWin = true;
        }
    }


    IEnumerator HandleDisabledCardInColumn ( int colIndex )
    {
        List<GameObject> cardsInColumn = GetCardsInColumn(colIndex);
        bool repositioning = false;
        for (int i = 0 ; i < cardsInColumn.Count ; i++)
        {
            if (!cardsInColumn [i].gameObject.activeSelf)
            {
                // Mark that the column needs a refill
                MarkColumnForRefill(colIndex);
                repositioning = true;
                break;
            }
        }
        refillColumnCompleted [colIndex] = false;
        if (repositioning)
        {
            CommandCentre.Instance.WinLoseManager_.enableSpin = false;
            StartCoroutine(RefillColumn(colIndex, cardsInColumn));
        }
        else
        {
            yield break;
        }

    }

    public List<GameObject> GetCardsInColumn ( int colIndex )
    {
        List<GameObject> cards = new List<GameObject>();
        cards = new List<GameObject>(CommandCentre.Instance.WinLoseManager_.columns [colIndex].Cards);
        if (cards == null || cards.Count == 0)
        {
            Debug.LogError($"No cards found in column {colIndex}.");
        }
        else
        {
            Debug.Log($"Found {cards.Count} cards in column {colIndex}.");
        }

        return cards;
    }

    public IEnumerator RefillColumn ( int colIndex , List<GameObject> cardsInColumn )
    {
        Deck responsibleDeck = multiDeckManager.GetDeck(colIndex);
        if (responsibleDeck == null)
        {
            Debug.LogError("Responsible deck is null. Ensure the MultiDeckManager is properly initialized.");
            yield break;
        }

        WinLoseManager winLoseManager = CommandCentre.Instance.WinLoseManager_;
        if (winLoseManager == null)
        {
            Debug.LogError("WinLoseManager is null. Ensure it's properly initialized.");
            yield break;
        }

        PoolManager poolManager = CommandCentre.Instance.PoolManager_;
        if (poolManager == null)
        {
            Debug.LogError("PoolManager is null. Ensure it's properly initialized.");
            yield break;
        }

        if (cardsInColumn == null || cardsInColumn.Count == 0)
        {
            Debug.LogError("No cards found in the column.");
            yield break;
        }

        List<GameObject> newCards = new List<GameObject>();
        //responsibleDeck.ResetDeck();

        for (int j = 0 ; j < cardsInColumn.Count ; j++)
        {
            if (cardsInColumn [j] == null)
            {
                Debug.LogError("Card in column is null.");
                continue;
            }

            if (!cardsInColumn [j].activeSelf)
            {
                GameObject newCard = responsibleDeck.DrawCard();
                responsibleDeck.ResetDeck();
                if (newCard == null)
                {
                    Debug.LogError("Deck is empty, unable to continue refilling.");
                    break;
                }

                Vector3 targetPosition = cardsInColumn [j].transform.localPosition;
                newCard.transform.rotation = Quaternion.Euler(0 , 180f , 0);
                newCard.transform.SetParent(cardsInColumn [j].transform.parent);
                CardPos cardPos = null;
                for (int i = 0 ;i<CardList.Count ;i++)
                {
                    if (CardList [i].GetComponent<CardPos>().TheOwner == cardsInColumn [j])
                    {
                        cardPos = CardList [i].GetComponent<CardPos>();
                    }
                }
                Debug.Log(cardPos);
                //CardPos cardPos = cardsInColumn [j].transform.GetComponentInParent<CardPos>();
                if (cardPos == null)
                {
                    Debug.LogError("CardPos component is missing in the parent.");
                    continue;
                }

                cardPos.TheOwner = newCard;
                newCards.Add(newCard);

                newCard.transform.DOLocalMove(targetPosition , CommandCentre.Instance.GridManager_.moveDuration)
                    .SetEase(Ease.OutQuad)
                    .SetDelay(colIndex * 4 * 0.1f)
                    .OnComplete(() =>
                    {
                        newCard.transform.localPosition = Vector3.zero;
                        ActivateNewCard(newCard);
                    });

                yield return new WaitForSeconds(CommandCentre.Instance.GridManager_.moveDuration);
                CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(colIndex , newCard);
            }
        }

        yield return new WaitForSeconds(CommandCentre.Instance.GridManager_.moveDuration + colIndex * 4 * 0.1f);
        MarkRefillComplete(colIndex);
        if (AreAllRefillColumnsCompleted())
        {
            CheckWin();
            winLoseManager.enableSpin = true;
            poolManager.ReturnAllInactiveCardsToPool();
        }
    }


    void CheckWin ()
    {
        CommandCentre.Instance.WinLoseManager_.PopulateGridChecker(CommandCentre.Instance.GridManager_.CardsParent.transform);
        Debug.Log("Win condition checked.");
        isWinChecked = true;
        IsDoneCheckingWin = true;
    }

    void ActivateNewCard(GameObject card )
    {
        card.SetActive(true);
    }

    void InitializeRefillTracking ( int numberOfColumns )
    {
        refillColumnCompleted = new bool [numberOfColumns];
        columnsToRefill = new bool [numberOfColumns];

        for (int i = 0 ; i < numberOfColumns ; i++)
        {
            // Assuming initially columns have cards, so marking false (not empty)
            columnsToRefill [i] = false;
            refillColumnCompleted [i] = false;
        }
    }

    public void MarkColumnForRefill ( int colIndex )
    {
        // Mark the column as needing a refill
        columnsToRefill [colIndex] = true;
    }

    public void MarkRefillComplete ( int colIndex )
    {
        // Mark the column as having completed the refill process
        refillColumnCompleted [colIndex] = true;
    }

    public bool AreAllRefillColumnsCompleted ()
    {
        // Check if all columns that were marked for refill are completed
        for (int i = 0 ; i < columnsToRefill.Length ; i++)
        {
            if (columnsToRefill [i] && !refillColumnCompleted [i])
            {
                return false; // If any marked column isn't done refilling, return false
            }
        }

        return true; // All marked columns are done refilling
    }

    public void refreshAllRefillColumnsCompleted ()
    {
        for (int i = 0;i< refillColumnCompleted.Length ; i++)
        {
            refillColumnCompleted [i] = false;
        }
    }

}
