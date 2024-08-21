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
    private bool [] columnsToRefill;       // Array to track which columns were initially empty

    private void Start ()
    {
        multiDeckManager = CommandCentre.Instance.MultiDeckManager_;
        refillColumnCompleted = new bool [5];
    }

    public void CheckAndFillColumns (int No_Of_Columns)
    {
        isWinChecked = false;
        IsDoneCheckingWin = false;
        Debug.Log("Get columns");
        // Get the number of columns and rows
        int columns = No_Of_Columns;

        // Iterate through each column
        for (int col = 0 ; col <= columns-1 ; col++)
        {
            StartCoroutine(HandleDisabledCardInColumn(col));
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
                repositioning = true;
                break;
            }
        }
        refillColumnCompleted [colIndex] = false;
        Debug.Log($"Checking if needs repositioning: {repositioning}");
        if (repositioning)
        {
            Debug.Log($"repositioning");
            CommandCentre.Instance.WinLoseManager_.enableSpin = false;
            StartCoroutine(RefillColumn(colIndex, cardsInColumn));
        }
        else
        {
            Debug.Log($"repositioning done");
            yield return new WaitForSeconds(2);
            if (!isWinChecked)
            {
                isWinChecked = true;
                IsDoneCheckingWin = true;
            }
            CommandCentre.Instance.WinLoseManager_.enableSpin = true;
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
        Debug.Log($"refilling");
        yield return new WaitForSeconds(colIndex * 0.2f); // Small delay before starting

        Deck responsibleDeck = multiDeckManager.GetDeck(colIndex);
        Vector3 newCardPos;

        for (int i = 0 ; i < cardsInColumn.Count ; i++)
        {
            if (!cardsInColumn [i].gameObject.activeSelf)
            {
                newCardPos = cardsInColumn [i].transform.localPosition;

                // Draw a new card from the responsible deck
                GameObject newCard = responsibleDeck.DrawCard();

                // Check if the deck is empty
                if (newCard == null)
                {
                    Debug.LogError("Deck is empty, unable to refill.");
                    yield break;
                }

                // Place the new card in the correct position
                newCard.transform.SetParent(cardsInColumn [i].transform.parent);
                newCard.transform.rotation=Quaternion.Euler(0,180f,0);
                // Animate the card's movement to the new position
                Tween myTween = newCard.transform.DOLocalMove(newCardPos , 0.35f)
                    .SetEase(Ease.OutQuad).SetDelay(colIndex*2 * 0.1f);
                yield return myTween.WaitForCompletion();

                // Activate and set the new card as the owner of the position
                ActivateNewCard(newCard);
                CommandCentre.Instance.PoolManager_.ReturnCard(cardsInColumn [i].gameObject);

                // Update the responsible deck and column list
                responsibleDeck.DeckCards.Remove(cardsInColumn [i].gameObject);
                responsibleDeck.RefillDeckFromPool();

                CommandCentre.Instance.WinLoseManager_.columns [colIndex].Cards.Remove(cardsInColumn [i].gameObject);
                CommandCentre.Instance.WinLoseManager_.columns [colIndex].Cards.Add(newCard);
            }
        }

        // Mark that refilling for this column is complete
        refillColumnCompleted [colIndex] = true;

        // If all columns are done refilling, check for win condition
        if (refillColumnCompleted.All(x => x))
        {
            CheckWin();
        }
    }

    void CheckWin ()
    {
        CommandCentre.Instance.WinLoseManager_.SpinEnd();
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

    private bool AreAllRefillColumnsCompleted ()
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
