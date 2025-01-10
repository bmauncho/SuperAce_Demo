using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cardPositions
{
    public List<GameObject> cardPositionInRow = new List<GameObject>(4);
}

public class GridManager : MonoBehaviour
{
    MultiDeckManager multiDeckManager;
    PoolManager poolManager;
    CardManager cardManager;

    [Header("Data")]
    public bool isFirstPlay = true;
    public bool isRefreshDone = true;
    public bool isRefilling = false;

    [Header("Data")]
    public GameObject cardPositionsHolder;

    [Header("Variables")]
    public float moveDuration = 0.5f;
    public float delayBetweenMoves = 0.1f;
    public int totalObjectsToPlace = 0;
    public int objectsPlaced;
    Vector3 originalPosition;

    [Header("Lists")]
    public List<cardPositions> rowData = new List<cardPositions>(5);

    private void Start ()
    {
        poolManager = CommandCentre.Instance.PoolManager_;
        multiDeckManager = CommandCentre.Instance.MultiDeckManager_;
        cardManager = CommandCentre.Instance.CardManager_;
        originalPosition = cardPositionsHolder.transform.localPosition;
    }

    [ContextMenu("Refresh Grid")]
    public void refreshGrid ()
    {
        isRefreshDone = false;
        StartCoroutine(refresh());
    }

    IEnumerator refresh ()
    {
        Vector3 direction = new Vector3(0 , -45f , 0);

        Tween myTween = cardPositionsHolder.transform.DOLocalMove(cardPositionsHolder.transform.localPosition + direction , .25f);
        yield return myTween.WaitForCompletion(true);
        returnCardsToPool();
        //Debug.Log(originalPosition);
        cardPositionsHolder.transform.localPosition = originalPosition;
        isRefreshDone = true;
        if (isRefreshDone)
        {
            populateGrid();
        }
    }

    void returnCardsToPool ()
    {
        if (!isFirstPlay)
        {
            foreach (var obj in rowData)
            {
                foreach (var _obj in obj.cardPositionInRow)
                {
                    var cardPos = _obj.GetComponent<CardPos>();
                    if (cardPos)
                    {
                        var card = cardPos.TheOwner;
                        if (card)
                        {
                            poolManager.ReturnCard(card);
                            cardPos.TheOwner = null;
                        }
                        else
                        {
                            Debug.LogWarning($"CardPos found :{cardPos.name} but TheOwner is null: {card.name}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Transform does not have CardPos");
                    }
                }
            }
        }
    }

    public bool isGridSpaceAvailable ()
    {
        Deck [] decks = multiDeckManager.decks;

        List<Transform> tempPos = new List<Transform>();

        foreach (var obj in rowData)
        {
            foreach (var _obj in obj.cardPositionInRow)
            {
                if (_obj.GetComponent<CardPos>().TheOwner == null)
                {
                    tempPos.Add(_obj.transform);
                }
            }
        }
       // Debug.Log(tempPos.Count);
        if (tempPos.Count < totalObjectsToPlace)
        {
            Debug.LogError("Not enough available positions to place all the cards.");
            return false;
        }

        return true;

    }

    [ContextMenu("Populate Grid")]
    public void populateGrid ()
    {
        Deck [] decks = multiDeckManager.decks;
        objectsPlaced = 0;
        float delayIncrement = 0.1f; // Delay between cards, adjust as needed
        int rowCount = 4; // Number of rows
        int columnCount = decks.Length; // Number of columns
        if (isGridSpaceAvailable())
        {

            if (CommandCentre.Instance.TurboManager_.TurboSpin_)
            {
                TurboFillGrid(columnCount , rowCount , decks);
            }
            else
            {
                NormalFillGrid(columnCount , rowCount , decks , delayIncrement);
            }

        }

    }

    void NormalFillGrid ( int columnCount , int rowCount , Deck [] decks , float delayIncrement )
    {
        if (decks == null || decks.Length == 0)
        {
            Debug.LogError("Decks array is null or empty.");
            return;
        }
        CommandCentre.Instance.SoundManager_.PlaySound("cards" , false , 1);
        for (int col = 0 ; col < columnCount ; col++)
        {
            if (col >= decks.Length || decks [col] == null)
            {
                Debug.LogError($"Deck at column {col} is null or out of bounds.");
                continue;
            }

            Deck currentDeck = decks [col];
            for (int row = 0 ; row < rowCount ; row++)
            {
                if (rowData == null || row >= rowData.Count || rowData [row] == null || rowData [row].cardPositionInRow == null || col >= rowData [row].cardPositionInRow.Count)
                {
                    Debug.LogError($"Row data or card position for row {row}, col {col} is invalid.");
                    continue;
                }

                GameObject newCard = currentDeck.DrawCard();
                if (newCard == null)
                {
                    currentDeck.ResetDeck();
                    newCard = currentDeck.DrawCard();
                    Debug.LogError($"Failed to draw card from deck {col}.");
                    continue;
                }

                if (isFirstPlay)
                {
                    cardManager.SetUpStartCards(newCard.GetComponent<Card>() , col , row);
                }
                else
                {
                    cardManager.setUpCard(newCard.GetComponent<Card>() , col , row);
                }

                currentDeck.ResetDeck();
                Transform targetPos = rowData [row].cardPositionInRow [col].transform;
                if (targetPos == null)
                {
                    Debug.LogError($"Target position for row {row}, col {col} is null.");
                    continue;
                }

                newCard.transform.SetParent(targetPos);
                newCard.transform.rotation = Quaternion.Euler(0 , 180f , 0);
                float delay = ( col * rowCount + row ) * delayIncrement;

                Sequence cardSequence = DOTween.Sequence();
                cardSequence.Append(newCard.transform.DOLocalMove(Vector3.zero , moveDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        newCard.transform.localPosition = Vector3.zero;
                        CardPos cardPosComponent = targetPos.GetComponent<CardPos>();
                        if (cardPosComponent != null)
                        {
                            cardPosComponent.TheOwner = newCard;
                        }
                        else
                        {
                            Debug.LogError($"CardPos component is missing on target position at row {row}, col {col}.");
                        }

                        CalculateObjectsPlaced();
                    }));
                cardSequence.PrependInterval(delay);
            }
        }
    }


    public void TurboFillGrid ( int columnCount , int rowCount , Deck [] decks )
    {
        // Ensure all inputs are valid
        if (decks == null || decks.Length < columnCount)
        {
            Debug.LogError("Decks array is null or does not match the column count.");
            return;
        }

        if (rowData == null || rowData.Count < rowCount)
        {
            Debug.LogError("RowData is null or does not match the row count.");
            return;
        }

        if (cardManager == null)
        {
            Debug.LogError("CardManager is not assigned.");
            return;
        }
        CommandCentre.Instance.SoundManager_.PlaySound("cards" , false , 1);
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = 0 ; row < rowCount ; row++)
            {
                // Validate rowData and its cardPositionInRow
                if (rowData [row] == null || rowData [row].cardPositionInRow == null || rowData [row].cardPositionInRow.Count <= col)
                {
                    Debug.LogError($"Invalid rowData or cardPositionInRow at row {row}, column {col}.");
                    continue;
                }

                Deck currentDeck = decks [col];
                if (currentDeck == null)
                {
                    Debug.LogError($"Deck at column {col} is null.");
                    continue;
                }

                GameObject newCard = currentDeck.DrawCard();
                if (newCard == null)
                {
                    Debug.LogError($"DrawCard returned null for deck at column {col}.");
                    continue;
                }

                // Setup the card using the cardManager
                Card cardComponent = newCard.GetComponent<Card>();
                if (cardComponent == null)
                {
                    Debug.LogError($"New card at column {col}, row {row} does not have a Card component.");
                    continue;
                }

                if (isFirstPlay)
                {
                    cardManager.SetUpStartCards(cardComponent , col , row);
                }
                else
                {
                    cardManager.setUpCard(cardComponent , col , row);
                }

                // Reset the deck for subsequent draws
                currentDeck.ResetDeck();

                // Get the target position
                Transform targetPos = rowData [row].cardPositionInRow [col]?.transform;
                if (targetPos == null)
                {
                    Debug.LogError($"Target position at row {row}, column {col} is null.");
                    continue;
                }

                // Set the card's parent and initial rotation
                newCard.transform.SetParent(targetPos);
                newCard.transform.rotation = Quaternion.Euler(0f , 180f , 0f);

                // Animate the card to the target position
                Sequence cardSequence = DOTween.Sequence();
                cardSequence.Append(newCard.transform.DOLocalMove(Vector3.zero , moveDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        newCard.transform.localPosition = Vector3.zero;
                        CardPos cardPos = targetPos.GetComponent<CardPos>();
                        if (cardPos != null)
                        {
                            cardPos.TheOwner = newCard;
                        }
                        else
                        {
                            Debug.LogError($"Target position at row {row}, column {col} does not have a CardPos component.");
                        }
                        CalculateObjectsPlaced();
                    }));
            }
        }

    }


    public void refillGrid ( int objectshidden )
    {
        isRefilling = true;
        Deck [] decks = multiDeckManager.decks;
        objectsPlaced = totalObjectsToPlace - objectshidden;
        float delayIncrement = 0.1f; // Delay between cards, adjust as needed
        int rowCount = 4; // Number of rows
        int columnCount = decks.Length; // Number of columns
        CommandCentre.Instance.SoundManager_.PlaySound("cards" , false , 1);
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = 0 ; row < rowCount ; row++)
            {

                GameObject cardPosHolder = rowData [row].cardPositionInRow [col];
                CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
                GameObject card = cardPos.TheOwner;
                if (!card)
                {
                    Deck currentDeck = decks [col];
                    GameObject newCard = currentDeck.DrawCard();

                    cardManager.setUpCard(newCard.GetComponent<Card>() , col , row);

                    currentDeck.ResetDeck();
                    Transform targetPos = rowData [row].cardPositionInRow [col].transform;
                    newCard.transform.SetParent(targetPos);
                    newCard.transform.rotation = Quaternion.Euler(0 , 180f , 0);
                    float delay = ( col * rowCount + row ) * delayIncrement;
                    Sequence cardSequence = DOTween.Sequence();
                    cardSequence.Append(newCard.transform.DOLocalMove(Vector3.zero , moveDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {

                            newCard.transform.localPosition = Vector3.zero;
                            targetPos.GetComponent<CardPos>().TheOwner = newCard;
                            CalculateObjectsPlaced();
                        }));
                    cardSequence.PrependInterval(delay);
                }
            }
        }

    }

    public void refillTurbo (int objectshidden)
    {
        isRefilling = true;
        Deck [] decks = multiDeckManager.decks;
        objectsPlaced = totalObjectsToPlace - objectshidden;
        int rowCount = 4; // Number of rows
        int columnCount = decks.Length; // Number of columns
        CommandCentre.Instance.SoundManager_.PlaySound("cards" , false , 1);
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = 0 ; row < rowCount ; row++)
            {

                GameObject cardPosHolder = rowData [row].cardPositionInRow [col];
                CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
                GameObject card = cardPos.TheOwner;
                if (!card)
                {
                    Deck currentDeck = decks [col];
                    GameObject newCard = currentDeck.DrawCard();
                    cardManager.setUpCard(newCard.GetComponent<Card>() , col , row);
                    currentDeck.ResetDeck();
                    Transform targetPos = rowData [row].cardPositionInRow [col].transform;

                    newCard.transform.SetParent(targetPos);
                    newCard.transform.rotation = Quaternion.Euler(0f , 180f , 0f);

                    Sequence cardSequence = DOTween.Sequence();
                    cardSequence.Append(newCard.transform.DOLocalMove(Vector3.zero , moveDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            if(newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                            {
                                CommandCentre.Instance.SoundManager_.PlaySound("ScatterDrop" , false , 1);
                            }
                            newCard.transform.localPosition = Vector3.zero;
                            targetPos.GetComponent<CardPos>().TheOwner = newCard;
                            CalculateObjectsPlaced();
                        }));
                }
            }
        }
    }

    void CalculateObjectsPlaced ()
    {
        objectsPlaced++;

        if (isGridFilled())
        {
            if (isFirstPlay)
            {
                isFirstPlay = false;
                CommandCentre.Instance.MainMenuController_.EnableWinMoreMenu();
            }
           //Debug.Log("Grid is filled");
            StartCoroutine(CheckAndContinue());
        }
    }

    IEnumerator CheckAndContinue ()
    {
        if (CommandCentre.Instance.APIManager_.betPlacingAPI_.IsUpdated)
        {
            CommandCentre.Instance.CashManager_.CashAmount = CommandCentre.Instance.APIManager_.betPlacingAPI_.response.new_wallet_balance;
            CommandCentre.Instance.CashManager_.updateThecashUi();
        }

        if (isRefilling)
        {
            isRefilling = false;
            CommandCentre.Instance.CashManager_.CashAmount = CommandCentre.Instance.APIManager_.betUpdaterAPI_.updateBetResponse_.new_wallet_balance;
            CommandCentre.Instance.CashManager_.updateThecashUi();
            yield return new WaitForSeconds(.25f);
            if (!CommandCentre.Instance.APIManager_.refillCardsAPI_.isError)
            {
                CommandCentre.Instance.APIManager_.GameDataAPI_.recheckWin();
            }
            CommandCentre.Instance.APIManager_.refillCardsAPI_.isError =false;
        }

        if (CommandCentre.Instance.WinLoseManager_.IsWin())
        {
            CommandCentre.Instance.CashManager_.updateThecashUi();
            CommandCentre.Instance.APIManager_.refillCardsAPI_.FetchData();
            CommandCentre.Instance.APIManager_.UpdateBet();
            yield return new WaitUntil(() => CommandCentre.Instance.APIManager_.refillCardsAPI_.refillDataFetched);
            CommandCentre.Instance.WinLoseManager_.winSequence();
        }
        else
        {
            int combo = CommandCentre.Instance.ComboManager_.GetCombo();
            if (combo >= 3)
            {
                //show total win
                CommandCentre.Instance.PayOutManager_.ShowTotalWinings();
                yield return new WaitForSeconds(5f);
                CommandCentre.Instance.PayOutManager_.HideTotalWinnings();
            }

            yield return StartCoroutine(Autospin());
        }
    }


    IEnumerator Autospin ()
    {
        // Wait until spinning is allowed
        yield return new WaitUntil(() => CommandCentre.Instance.MainMenuController_.CanSpin);

        // Check if auto-spin is enabled and perform the spin
        if (CommandCentre.Instance.AutoSpinManager_.IsAutoSpin)
        {
            Debug.Log("Can auto spin");
            CommandCentre.Instance.MainMenuController_.Spin();
        }
    }


    public bool isGridFilled ()
    {
        return objectsPlaced >= totalObjectsToPlace;
    }
}
