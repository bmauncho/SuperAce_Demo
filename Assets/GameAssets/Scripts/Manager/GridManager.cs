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
        Debug.Log(tempPos.Count);
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
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = 0 ; row < rowCount ; row++)
            {
                Deck currentDeck = decks [col];
                GameObject newCard = currentDeck.DrawCard();
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

        isFirstPlay = false;
    }

    void TurboFillGrid ( int columnCount , int rowCount , Deck [] decks )
    {
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = 0 ; row < rowCount ; row++)
            {
                Deck currentDeck = decks [col];
                GameObject newCard = currentDeck.DrawCard();
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

                newCard.transform.SetParent(targetPos);
                newCard.transform.rotation = Quaternion.Euler(0f , 180f , 0f);

                Sequence cardSequence = DOTween.Sequence();
                cardSequence.Append(newCard.transform.DOLocalMove(Vector3.zero , moveDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {

                        newCard.transform.localPosition = Vector3.zero;
                        targetPos.GetComponent<CardPos>().TheOwner = newCard;
                        CalculateObjectsPlaced();
                    }));
            }
        }
        isFirstPlay = false;
    }


    public void refillGrid ( int objectshidden )
    {
        Deck [] decks = multiDeckManager.decks;
        objectsPlaced = totalObjectsToPlace - objectshidden;
        float delayIncrement = 0.1f; // Delay between cards, adjust as needed
        int rowCount = 4; // Number of rows
        int columnCount = decks.Length; // Number of columns

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
        Deck [] decks = multiDeckManager.decks;
        objectsPlaced = totalObjectsToPlace - objectshidden;
        int rowCount = 4; // Number of rows
        int columnCount = decks.Length; // Number of columns
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
            Debug.Log("Grid is filled");
            StartCoroutine(CheckAndContinue());
        }
    }

    IEnumerator CheckAndContinue ()
    {
        if (CommandCentre.Instance.WinLoseManager_.IsWin())
        {
            CommandCentre.Instance.WinLoseManager_.winSequence();
        }
        else
        {
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
