using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoGridManager : MonoBehaviour
{
    MultiDeckManager multiDeckManager;
    PoolManager poolManager;
    CardManager cardManager;
    public DemoWinLoseManager winLoseManager;
    public DemoSequence demoSequence;

    [Header("Data")]
    public bool isFirstPlay = true;
    public bool isRefreshDone = true;
    public bool isRefilling = false;

    [Header("Data")]
    public GameObject cardPositionsHolder;

    [Header("Variables")]
    public float moveDuration = 0.5f;
    public float delayBetweenMoves = 0.1f;
    public int totalDemoObjectsToPlace = 0;
    public int demoObjectsPlaced;
    Vector3 originalPosition;

    [Header("Lists")]
    public List<cardPositions> colData = new List<cardPositions>(5);
    bool firstDemocheckpoint =false;
    private void Start ()
    {
        poolManager = CommandCentre.Instance.PoolManager_;
        multiDeckManager = CommandCentre.Instance.MultiDeckManager_;
        cardManager = CommandCentre.Instance.CardManager_;
        originalPosition = cardPositionsHolder.transform.localPosition;
    }

    [ContextMenu("Refresh Demo Grid")]
    public void refreshDemoGrid ()
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
            foreach (var obj in colData)
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

        foreach (var obj in colData)
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
        if (tempPos.Count < totalDemoObjectsToPlace)
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
        demoObjectsPlaced = 0;
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

                    cardManager.setUpDemoCards(newCard.GetComponent<Card>() , col , row);
                }
                currentDeck.ResetDeck();
                Transform targetPos = colData [col].cardPositionInRow [row].transform;
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

                    cardManager.setUpDemoCards(newCard.GetComponent<Card>() , col , row);
                }
                currentDeck.ResetDeck();
                Transform targetPos = colData [col].cardPositionInRow [row].transform;

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
        isRefilling = true;
        Deck [] decks = multiDeckManager.decks;
        demoObjectsPlaced = totalDemoObjectsToPlace - objectshidden;
        float delayIncrement = 0.1f; // Delay between cards, adjust as needed
        int rowCount = 4; // Number of rows
        int columnCount = decks.Length; // Number of columns

        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = 0 ; row < rowCount ; row++)
            {

                GameObject cardPosHolder = colData [col].cardPositionInRow [row];
                CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
                GameObject card = cardPos.TheOwner;
                if (!card)
                {
                    Deck currentDeck = decks [col];
                    GameObject newCard = currentDeck.DrawCard();

                    cardManager.setUpCard(newCard.GetComponent<Card>() , col , row);

                    currentDeck.ResetDeck();
                    Transform targetPos = colData [col].cardPositionInRow [row].transform;
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

    public void refillTurbo ( int objectshidden )
    {
        isRefilling = true;
        Deck [] decks = multiDeckManager.decks;
        demoObjectsPlaced = totalDemoObjectsToPlace - objectshidden;
        int rowCount = 4; // Number of rows
        int columnCount = decks.Length; // Number of columns
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = 0 ; row < rowCount ; row++)
            {

                GameObject cardPosHolder = colData [row].cardPositionInRow [col];
                CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
                GameObject card = cardPos.TheOwner;
                if (!card)
                {
                    Deck currentDeck = decks [col];
                    GameObject newCard = currentDeck.DrawCard();
                    cardManager.setUpCard(newCard.GetComponent<Card>() , col , row);
                    currentDeck.ResetDeck();
                    Transform targetPos = colData [col].cardPositionInRow [row].transform;

                    newCard.transform.SetParent(targetPos);
                    newCard.transform.rotation = Quaternion.Euler(0f , 180f , 0f);

                    Sequence cardSequence = DOTween.Sequence();
                    cardSequence.Append(newCard.transform.DOLocalMove(Vector3.zero , moveDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            if (newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                            {
                                CommandCentre.Instance.SoundManager_.PlaySound("ScatterDrop" , false , CommandCentre.Instance.SoundManager_.maxSound);
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
        demoObjectsPlaced++;
        if (isDemoGridFilled())
        {
            demoSequence.SetUpCards();
            Debug.Log("Grid is filled");
            StartCoroutine(CheckAndContinue());
        }
    }


    IEnumerator CheckAndContinue ()
    {
        yield return new WaitUntil(()=>demoSequence.isSetUpCard);
        Debug.Log($"can refill : {winLoseManager.CanRefill()}");
        if (winLoseManager.CanRefill())
        {
            winLoseManager.DemoWinSequence();
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
                if (!firstDemocheckpoint)
                {
                    firstDemocheckpoint = true;
                    CommandCentre.Instance.DemoManager_.ActivateDemoFeature();
                }
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

        //check win
    }


    public bool isDemoGridFilled ()
    {
        return demoObjectsPlaced >= totalDemoObjectsToPlace;
    }
}
