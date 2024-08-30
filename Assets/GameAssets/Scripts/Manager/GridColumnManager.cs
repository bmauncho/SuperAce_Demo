using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine.UIElements;

[System.Serializable]
public class GridPosColumns
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
    public bool[] refillColumnCompleted;
    public bool[] columnsToRefill;
    public bool isRepositioning;
    public int totalObjectsToPlace;
    public int objectsPlaced;
    public int totalObjectsToRotate;
    public int objectsRotated;

    public GameObject CardPosHolders;
    public List<GameObject> CardList = new List<GameObject>();
    public List<GridPosColumns> Columns = new List<GridPosColumns>();

    private void Start()
    {
        poolManager = CommandCentre.Instance.PoolManager_;
        winLoseManager = CommandCentre.Instance.WinLoseManager_;
        multiDeckManager = CommandCentre.Instance.MultiDeckManager_;
        refillColumnCompleted = new bool[5];
        GetCardPositions();
    }

    void GetCardPositions()
    {
        foreach (Transform tr in CardPosHolders.transform)
        {
            CardList.Add(tr.gameObject);
        }
    }

    int TotalNumberofEmptyCardPos()
    {
        List<GameObject> cardsPos = new List<GameObject>();
        for (int i = 0; i < Columns.Count; i++)
        {
            for (int j = 0; j < Columns[i].CardsPos.Count; j++)
            {
                if (!Columns[i].CardsPos[j].GetComponent<CardPos>().TheOwner)
                {
                    cardsPos.Add(Columns[i].CardsPos[j]);
                }
            }
        }
        return cardsPos.Count;
    }

    public void CheckAndFillColumns(int No_Of_Columns)
    {
        refreshAllRefillColumnsCompleted();
        isWinChecked = false;
        IsDoneCheckingWin = false;
        totalObjectsToPlace = TotalNumberofEmptyCardPos();
        objectsPlaced = 0;
        totalObjectsToRotate = CommandCentre.Instance.WinLoseManager_.goldenCards.Count;
        objectsRotated = 0;

        // Get the number of columns and rows
        int columns = No_Of_Columns;
        InitializeRefillTracking(No_Of_Columns);

        // Iterate through each column
        StartCoroutine(ProcessColumnsSequentially(columns));
    }

    private IEnumerator ProcessColumnsSequentially(int columns)
    {
        for (int col = 0; col <= columns - 1; col++)
        {
            yield return StartCoroutine(HandleDisabledCardInColumn(col));
        }

        // Check win condition only when both repositioning and rotations are complete
        while (!IsGridRepositioningComplete() || !IsGridGoldenCardsRotationDone())
        {
            yield return null; // Wait until all cards are repositioned and all rotations are complete
        }

        // After all columns are processed and conditions met, check win conditions
        CheckWin();
    }

    IEnumerator HandleDisabledCardInColumn(int colIndex)
    {
        List<GameObject> cardsInColumn = Columns[colIndex].CardsPos;
        bool repositioning = false;

        for (int i = 0; i < cardsInColumn.Count; i++)
        {
            if (!cardsInColumn[i].GetComponent<CardPos>().TheOwner)
            {
                MarkColumnForRefill(colIndex);
                repositioning = true;
                break;
            }
        }

        refillColumnCompleted[colIndex] = false;

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

    public IEnumerator RefillColumn(int colIndex)
    {
        isRepositioning = true;
        Deck responsibleDeck = multiDeckManager.GetDeck(colIndex);
        responsibleDeck.ResetDeck();

        List<GameObject> newCards = new List<GameObject>();
        List<GameObject> cardPosInColumn = Columns[colIndex].CardsPos;

        CheckforRotatedCards();

        float delayIncrement = 0.05f;
        int rowCount = 4;
        int positionIndex = 0;

        for (int row = 0; row < rowCount; row++)
        {
            GameObject currentCardPos = cardPosInColumn[positionIndex];
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
                    newCard.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                    newCard.transform.SetParent(CardPos.transform);
                    Vector3 targetPosition = Vector3.zero;
                    float delay = (colIndex * rowCount + row) * delayIncrement;

                    Sequence cardSequence = DOTween.Sequence();
                    cardSequence.Append(newCard.transform.DOLocalMove(targetPosition, CommandCentre.Instance.GridManager_.moveDuration)
                        .SetEase(Ease.OutQuad)
                        .SetDelay(delay)
                        .OnComplete(() =>
                        {
                            CalculateObjectsPlaced();
                            newCard.transform.localPosition = Vector3.zero;
                            ActivateNewCard(newCard);
                        }));

                    CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(colIndex);
                }
            }
            positionIndex++;
        }

        MarkRefillComplete(colIndex);

        // If all refills are complete, check win conditions
        if (AreAllRefillColumnsCompleted())
        {
            CheckWinConditions();
        }

        yield return null;
    }

    void CheckforRotatedCards()
    {
        List<GameObject> winningGoldenCards = new List<GameObject>(CommandCentre.Instance.WinLoseManager_.goldenCards);
        CommandCentre.Instance.WinLoseManager_.goldenCards.Clear();
        totalObjectsToRotate = winningGoldenCards.Count;
        objectsRotated = 0;

        int columnIndex = 0;
        foreach (var column in Columns)
        {
            int cardIndex = 0;
            foreach (var cardPos in column.CardsPos)
            {
                if (cardPos == null) continue;

                var owner = cardPos.GetComponent<CardPos>().TheOwner;
                if (owner != null && winningGoldenCards.Contains(owner) && owner.GetComponent<Card>().IsGoldenCard)
                {
                    Vector3 targetRotation = new Vector3(0, 180, 0);
                    float duration = 1.0f;
                    float delayIncrement = 0.05f;
                    float delay = (columnIndex * 4 + cardIndex) * delayIncrement;
                    CommandCentre.Instance.CardManager_.RandomizeDealing_Jocker(owner.transform);

                    Sequence rotSequence = DOTween.Sequence();
                    rotSequence.Append(owner.transform.DORotate(targetRotation, duration)
                        .SetEase(Ease.OutQuad)
                        .SetDelay(delay)
                        .OnComplete(() =>
                        {
                            objectsRotated++;
                            if (objectsRotated == totalObjectsToRotate)
                            {
                                EnableSpin();
                            }
                        }));

                    cardIndex++;
                }
            }
            columnIndex++;
        }
    }

    public bool IsGridGoldenCardsRotationDone()
    {
        return objectsRotated >= totalObjectsToRotate;
    }

    void CheckWin()
    {
        Debug.Log("win Check");
        CommandCentre.Instance.WinLoseManager_.PopulateGridChecker(CommandCentre.Instance.GridManager_.CardsParent.transform);
        isWinChecked = true;
        IsDoneCheckingWin = true;
    }

    void CalculateObjectsPlaced()
    {
        objectsPlaced++;
        if (IsGridRepositioningComplete())
        {
            EnableSpin();
        }
    }

    public void ResetIsGridRepositioning()
    {
        objectsPlaced = 0;
    }

    public void ResetRotation()
    {
        objectsRotated = 0;
    }

    void EnableSpin()
    {
        winLoseManager.enableSpin = true;
    }

    public bool IsGridRepositioningComplete()
    {
        return objectsPlaced >= totalObjectsToPlace;
    }

    void ActivateNewCard(GameObject card)
    {
        card.SetActive(true);
    }

    void InitializeRefillTracking(int numberOfColumns)
    {
        refillColumnCompleted = new bool[numberOfColumns];
        columnsToRefill = new bool[numberOfColumns];

        for (int i = 0; i < numberOfColumns; i++)
        {
            columnsToRefill[i] = false;
            refillColumnCompleted[i] = false;
        }
    }

    public void MarkColumnForRefill(int colIndex)
    {
        columnsToRefill[colIndex] = true;
    }

    public void MarkRefillComplete(int colIndex)
    {
        refillColumnCompleted[colIndex] = true;
    }

    public bool AreAllRefillColumnsCompleted()
    {
        for (int i = 0; i < columnsToRefill.Length; i++)
        {
            if (columnsToRefill[i] && !refillColumnCompleted[i])
            {
                return false;
            }
        }
        return true;
    }

    public void refreshAllRefillColumnsCompleted()
    {
        for (int i = 0; i < refillColumnCompleted.Length; i++)
        {
            refillColumnCompleted[i] = false;
        }
    }

    void CheckWinConditions()
    {
        // Wait for any ongoing animations to complete before checking the win condition
        if (IsGridRepositioningComplete() && IsGridGoldenCardsRotationDone())
        {
            CheckWin();
        }
    }
}
