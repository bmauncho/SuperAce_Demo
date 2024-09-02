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

    public int totalObjectsToShake;
    public int objectsShaked;

    public GameObject CardPosHolders;
    public List<GameObject> CardList = new List<GameObject>();
    public List<GridPosColumns> Columns = new List<GridPosColumns>();
    public bool [] bigJokerCardsProcessed = new bool [5];

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

    private IEnumerator ProcessColumnsSequentially ( int columns )
    {
        for (int col = 0 ; col <= columns - 1 ; col++)
        {
            yield return StartCoroutine(HandleDisabledCardInColumn(col));
        }

        // Wait until all cards are repositioned and rotations are complete
        while (!IsGridRepositioningComplete() || !IsGridGoldenCardsRotationDone())
        {
            yield return null;
        }

        if (CheckForCardsToShake())
        {
            yield return StartCoroutine(ShakeCards());
        }
        yield return new WaitForSeconds(.5f);
        // After shaking animation or if no cards to shake, check win conditions
        CheckWin();
    }


    IEnumerator HandleDisabledCardInColumn (int colIndex)
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
        // If all refills are complete, check win conditions
        if (AreAllRefillColumnsCompleted())
        {
            StartCoroutine(ShakeCards());
            bool IsDone = false;
            while (!IsObjectShakedComplete())
            {
                if (IsObjectShakedComplete())
                {
                    IsDone = true;
                    break;
                }
                yield return null;
            }

            if (IsDone)
            {
                yield return new WaitForSeconds(0.5f); // delay by 0.5 seconds
                CheckWin();
                Debug.Log("Proceed to checkWin");
            }
        }
    }

    private void CheckforRotatedCards ()
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
                    Vector3 targetRotation = new Vector3(0 , 180 , 0);
                    float duration = 1.0f;
                    float delayIncrement = 0.05f;
                    float delay = ( columnIndex * 4 + cardIndex ) * delayIncrement;
                    CommandCentre.Instance.CardManager_.RandomizeDealing_Jocker(owner.transform);

                    // Start rotation coroutine for each card
                    StartCoroutine(RotateCardAndWait(owner.transform , targetRotation , duration , delay));

                    cardIndex++;
                }
            }
            columnIndex++;
        }
    }

    private IEnumerator RotateCardAndWait ( Transform target , Vector3 targetRotation , float duration , float delay )
    {
        // Wait for delay before starting rotation
        yield return new WaitForSeconds(delay);

        // Create a rotation tween and wait for it to complete
        Tween rotationTween = target.DORotate(targetRotation , duration).SetEase(Ease.OutQuad);
        yield return rotationTween.WaitForCompletion();

        // Ensure the rotation is set correctly at the end
        target.rotation = Quaternion.Euler(0 , 180 , 0);
        objectsRotated++;

        // Check if all rotations are done
        if (objectsRotated == totalObjectsToRotate)
        {
            StartCoroutine(DelayWinCheck(0.5f));
        }
    }

    // Coroutine to delay the win check by a given delay time
    private IEnumerator DelayWinCheck ( float delay )
    {
        yield return new WaitForSeconds(delay);
        EnableSpin(); // This is your win check logic
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
        bigJokerCardsProcessed = new bool[numberOfColumns];

        for (int i = 0; i < numberOfColumns; i++)
        {
            columnsToRefill[i] = false;
            refillColumnCompleted[i] = false;
            bigJokerCardsProcessed[i] = false;
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
 #region
    //unsed code
    //void CheckWinConditions ()
    //{
    //    // Wait for any ongoing animations to complete before checking the win condition
    //    if (IsGridRepositioningComplete() && IsGridGoldenCardsRotationDone())
    //    {
    //        bool hasCardsToShake = CheckForCardsToShake();

    //        if (hasCardsToShake)
    //        {
    //            // Shake the cards if any exist
    //            StartCoroutine(ShakeCards());
    //            bool IsDone = false;
    //            while (!IsObjectShakedComplete())
    //            {
    //                if (IsObjectShakedComplete())
    //                {
    //                    IsDone = true;
    //                }
    //                return;
    //            }

    //            Debug.Log("Proceed to checkWin");
    //            if (IsDone) { CheckWin(); }
    //        }
    //        else
    //        {
    //            // After shaking animation or if no cards to shake, check win conditions
    //            CheckWin();
    //        }
    //    }
    //}
    //end of unused code
#endregion
    private IEnumerator ShakeCards ()
    {
        yield return new WaitForSeconds(0.5f);

        totalObjectsToShake = CardsToShake().Count;
        objectsShaked = 0;
        HashSet<GameObject> cards = new HashSet<GameObject>(CardsToShake());

        foreach (GameObject card in cards)
        {
            // Wait until any active rotation tween is complete
            yield return StartCoroutine(WaitForTweenToComplete(card.transform));

            // Create a sequence for each card to handle the shake
            Sequence sequence = DOTween.Sequence();
            sequence.Append(card.transform.DOShakeRotation(0.25f , new Vector3(0 , 0 , 15) , 10 , 90 , true , ShakeRandomnessMode.Harmonic))
                .OnComplete(() =>
                {
                    objectsShaked++;
                    card.transform.rotation = Quaternion.Euler(0 , 180 , 0);
                    Debug.Log($"Card {card.name} finished shaking.");
                });
        }

        // Wait until all shake animations are complete
        yield return new WaitUntil(() => IsObjectShakedComplete());
        
        Debug.Log("Shaking complete");

        yield return StartCoroutine(CheckForBigJokerAndAnimate());
    }

    private IEnumerator WaitForTweenToComplete ( Transform target )
    {
        // Wait until there is no active tween on the card's transform
        while (DOTween.IsTweening(target , true))
        {
            yield return null; // Wait for the next frame
        }

        // Once the tween completes, ensure the final rotation is set correctly
        target.rotation = Quaternion.Euler(0 , 180 , 0);
        Debug.Log($"Card {target.name} rotation tween completed, rotation set to (0, 180, 0).");
    }

    List<GameObject> CardsToShake ()
    {
        List<GameObject> cards = new List<GameObject>();
        foreach (GridPosColumns column in Columns)
        {
            foreach (GameObject cardPos in column.CardsPos)
            {
                CardPos cardPosScript = cardPos.GetComponent<CardPos>();

                if (cardPosScript != null && cardPosScript.TheOwner != null)
                {
                    GameObject card = cardPosScript.TheOwner;
                    Card cardScript = card.GetComponent<Card>();

                    if (cardScript != null && cardScript.cardType == CardType.Big_Jocker)
                    {
                        cards.Add(card);
                    }

                }
            }
        }
        return cards;
    }

    public bool IsObjectShakedComplete ()
    {
        return objectsShaked >= totalObjectsToShake;
    }

    private bool CheckForCardsToShake ()
    {
        if (CardsToShake().Count > 0)
        {
            return true;
        }
        return false;
    }

    private IEnumerator CheckForBigJokerAndAnimate ()
    {
        Debug.Log(" CheckForBigJokerAndAnimate");
        // Wait for any ongoing tweens to complete
        while (DOTween.TotalPlayingTweens() > 0)
        {
            yield return null;
        }

        List<Tween> activeTweens = new List<Tween>();

        foreach (GridPosColumns column in Columns)
        {
            foreach (GameObject cardPos in column.CardsPos)
            {
                CardPos cardPosScript = cardPos.GetComponent<CardPos>();

                if (cardPosScript?.TheOwner != null)
                {
                    Card cardScript = cardPosScript.TheOwner.GetComponent<Card>();

                    if (cardScript != null && cardScript.cardType == CardType.Big_Jocker)
                    {
                        int columnIndex = Columns.IndexOf(column);
                        if (!bigJokerCardsProcessed [columnIndex])
                        {
                            bigJokerCardsProcessed [columnIndex] = true;
                            // Get two new cards from the pool manager
                            GameObject newCard1 = poolManager.GetCard();
                            GameObject newCard2 = poolManager.GetCard();
                            CommandCentre.Instance.CardManager_.DealBigJocker(newCard1.transform);
                            CommandCentre.Instance.CardManager_.DealBigJocker(newCard1.transform);
                            if (newCard1 != null && newCard2 != null)
                            {
                                Vector3 initialPosition = cardPosScript.TheOwner.transform.position;
                                Quaternion initialRotation = cardPosScript.TheOwner.transform.rotation;

                                // Select random positions for the new cards
                                int randomColumnIndex1 = Random.Range(0 , 5);
                                int randomPositionIndex1 = Random.Range(0 , Columns [randomColumnIndex1].CardsPos.Count);

                                int randomColumnIndex2, randomPositionIndex2;
                                do
                                {
                                    randomColumnIndex2 = Random.Range(0 , 5);
                                    randomPositionIndex2 = Random.Range(0 , Columns [randomColumnIndex2].CardsPos.Count);
                                } while (randomColumnIndex1 == randomColumnIndex2 && randomPositionIndex1 == randomPositionIndex2);

                                // Position and rotate the new cards at the "Big Joker" card's position
                                newCard1.transform.SetPositionAndRotation(initialPosition , initialRotation);
                                newCard2.transform.SetPositionAndRotation(initialPosition , initialRotation);

                                // DOTween jump animations to move the new cards to their random positions
                                activeTweens.Add(newCard1.transform.DOJump(
                                    Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].transform.position ,
                                    2.0f , 1 , 1.0f)
                                    .OnComplete(() =>
                                    {
                                        Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].transform.rotation = Quaternion.Euler(0 , 180 , 0);

                                    }));
                                Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].GetComponent<CardPos>().TheOwner.SetActive(false);
                                Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].GetComponent<CardPos>().TheOwner = newCard1;

                                activeTweens.Add(newCard2.transform.DOJump(
                                    Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].transform.position ,
                                    2.0f , 1 , 1.0f)
                                    .OnComplete(() =>
                                    {
                                        Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].transform.rotation = Quaternion.Euler(0 , 180 , 0);

                                    }));
                                Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].GetComponent<CardPos>().TheOwner.SetActive(false);
                                Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].GetComponent<CardPos>().TheOwner = newCard2;
                            }
                        }
                    }
                }
            }
        }

        // Wait for all animations to complete
        foreach (Tween tween in activeTweens)
        {
            yield return tween.WaitForCompletion();
        }
        yield return new WaitForSeconds (.5f);
    }
}
