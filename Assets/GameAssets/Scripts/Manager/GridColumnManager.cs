using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine.UIElements;
[System.Serializable]
public class  GridPosColumns
{
    public List<GameObject> CardsPos = new List<GameObject>();
}
public class GridColumnManager : MonoBehaviour
{
    WinLoseManager winLoseManager;
    PoolManager poolManager;
    private MultiDeckManager multiDeckManager;
    public bool IsDoneCheckingWin;
    public bool isWinChecked = false;
    public bool [] refillColumnCompleted;
    public bool [] columnsToRefill;
    public bool isRepositioning;
    public int totalObjectsToPlace;
   public int objectsPlaced;

    public GameObject CardPosHolders;
    public List<GameObject> CardList = new List<GameObject>();
    public List<GridPosColumns> Columns = new List<GridPosColumns>();
    private void Start ()
    {
        poolManager = CommandCentre.Instance.PoolManager_;
        winLoseManager = CommandCentre.Instance.WinLoseManager_;
        multiDeckManager = CommandCentre.Instance.MultiDeckManager_;
        refillColumnCompleted = new bool [5];
        GetCardPositions();
    }

    void GetCardPositions ()
    {
        foreach (Transform tr in CardPosHolders.transform)
        {
            CardList.Add(tr.gameObject);
        }
    }

    int TotalNumberofEmptyCardPos ()
    {
        List<GameObject> cardsPos = new List<GameObject>();
        for(int i = 0 ; i < Columns.Count ;i++)
        {
            for(int j = 0 ;j< Columns [i].CardsPos.Count; j++)
            {
                if(!Columns [i].CardsPos [j].GetComponent<CardPos>().TheOwner)
                {
                    cardsPos.Add(Columns [i].CardsPos [j]);
                }
            }
        }

        return cardsPos.Count;
    }

    public void CheckAndFillColumns (int No_Of_Columns)
    {
        refreshAllRefillColumnsCompleted ();
        isWinChecked = false;
        IsDoneCheckingWin = false;
        totalObjectsToPlace = TotalNumberofEmptyCardPos();
        objectsPlaced = 0;
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
        // Get the list of card positions in the column using the Columns list
        List<GameObject> cardsInColumn = Columns [colIndex].CardsPos;
        bool repositioning = false;

        for (int i = 0 ; i < cardsInColumn.Count ; i++)
        {
            if (!cardsInColumn [i].GetComponent<CardPos>().TheOwner)
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
            StartCoroutine(RefillColumn(colIndex));
        }
        else
        {
            yield break;
        }
    }

    public IEnumerator RefillColumn ( int colIndex )
    {
        isRepositioning = true;
        // Get the responsible deck for this column
        Deck responsibleDeck = multiDeckManager.GetDeck(colIndex);
        responsibleDeck.ResetDeck();
        //Debug.Log($"Refilling column {colIndex} using deck {responsibleDeck.name}");

        List<GameObject> newCards = new List<GameObject>();
        List<GameObject> cardPosInColumn = Columns [colIndex].CardsPos;  // Directly access the column from the Columns list

        float delayIncrement = 0.05f; // Delay between cards, adjust as needed
        int rowCount = 4; // Number of rows
        int positionIndex = 0; // Keeps track of the current position in the column
        for (int row = 0 ; row < rowCount ; row++)
        {
            GameObject currentCardPos = cardPosInColumn [positionIndex];
            if (!currentCardPos.GetComponent<CardPos>().TheOwner)
            {
                GameObject newCard = responsibleDeck.DrawCard();

                if (newCard == null)
                {
                    Debug.LogError("Deck is empty, unable to continue refilling.");
                    break;
                }
                CardPos CardPos = currentCardPos.GetComponent<CardPos>();
                if (!CardPos.TheOwner)
                {
                    CardPos.TheOwner = newCard;
                    newCard.transform.rotation = Quaternion.Euler(0 , 180f , 0);
                    newCard.transform.SetParent(CardPos.transform);
                    Vector3 targetPosition = Vector3.zero;
                    float delay = ( colIndex * rowCount + row ) * delayIncrement; // Calculate delay based on column and row

                    Sequence cardSequence = DOTween.Sequence();
                    cardSequence.Append(newCard.transform.DOLocalMove(targetPosition , CommandCentre.Instance.GridManager_.moveDuration)
                        .SetEase(Ease.OutQuad)
                        .SetDelay(delay)
                        .OnComplete(() =>
                        {
                            CalculateObjectsPlaced();
                            newCard.transform.localPosition = Vector3.zero; // Ensure final position is correct
                            ActivateNewCard(newCard);
                        }));

                    //yield return new WaitForSeconds(CommandCentre.Instance.GridManager_.moveDuration);
                    CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(colIndex , newCard);
                }
            }
            positionIndex++; // Move to the next position in the column
        }
        // Mark refill complete for this column
        MarkRefillComplete(colIndex);
        // If all refills are complete, check win conditions
        if (AreAllRefillColumnsCompleted())
        {
            CheckWin();
            winLoseManager.enableSpin = true;
            poolManager.ReturnAllInactiveCardsToPool();
        }
        yield return null;
    }

    void CheckWin ()
    {
        CommandCentre.Instance.WinLoseManager_.PopulateGridChecker(CommandCentre.Instance.GridManager_.CardsParent.transform);
        isWinChecked = true;
        IsDoneCheckingWin = true;
    }

    void CalculateObjectsPlaced ()
    {
        objectsPlaced++;
        if (IsGridRepositioningComplete())
        {
            winLoseManager.enableSpin = true;
        }
    }

    public bool IsGridRepositioningComplete ()
    {
        return objectsPlaced >= totalObjectsToPlace;
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
