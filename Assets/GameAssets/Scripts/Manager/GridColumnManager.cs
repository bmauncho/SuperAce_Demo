using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using System.Linq;

public class GridColumnManager : MonoBehaviour
{
    private MultiDeckManager multiDeckManager;
    public bool IsDoneCheckingWin;
    public bool isWinChecked = false;
    public bool [] refillColumnCompleted;
    public bool [] columnsToRefill;
    public bool isRepositioning;

    private void Start ()
    {
        multiDeckManager = CommandCentre.Instance.MultiDeckManager_;
        refillColumnCompleted = new bool [5];
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
        return cards;
    }

    public IEnumerator RefillColumn ( int colIndex , List<GameObject> cardsInColumn )
    {
        Deck responsibleDeck = multiDeckManager.GetDeck(colIndex);
        WinLoseManager winLoseManager = CommandCentre.Instance.WinLoseManager_;
        PoolManager poolManager = CommandCentre.Instance.PoolManager_;

        List<GameObject> newCards = new List<GameObject>();

        responsibleDeck.ResetDeck();

        for (int j = 0 ; j < cardsInColumn.Count ; j++)
        {
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
                CardPos cardPos = cardsInColumn [j].transform.GetComponentInParent<CardPos>();
                cardPos.TheOwner = newCard;

                newCards.Add(newCard);

                // Tween the card to its target position
                newCard.transform.DOLocalMove(targetPosition , CommandCentre.Instance.GridManager_.moveDuration)
                    .SetEase(Ease.OutQuad)
                    .SetDelay(colIndex * 4 * 0.1f)
                    .OnComplete(() =>
                    {
                        newCard.transform.localPosition = Vector3.zero;
                        ActivateNewCard(newCard);
                    });

                // Wait for the card movement to complete
                yield return new WaitForSeconds(CommandCentre.Instance.GridManager_.moveDuration);
                CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(colIndex , newCard);
            }
        }

        yield return new WaitForSeconds(CommandCentre.Instance.GridManager_.moveDuration + colIndex * 4 * 0.1f);

        MarkRefillComplete(colIndex);
        if (AreAllRefillColumnsCompleted())
        {
            CheckWin();
            CommandCentre.Instance.WinLoseManager_.enableSpin = true;
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
