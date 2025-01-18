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
    [SerializeField]bool firstDemoFreeSpin = true;
    [SerializeField]bool secondDemoSpin = false;
    [SerializeField]bool secondCheckPoint = false;
    int refilcount = 0;
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
       // Debug.Log(tempPos.Count);
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
        CommandCentre.Instance.SoundManager_.PlaySound("cards" , false);
        for (int col = 0 ; col < columnCount ; col++)
        {
            if (col == 2)
            {
                CommandCentre.Instance.SoundManager_.PlaySound("cards" , false);
            }
            Deck currentDeck = decks [col];
            for (int row = rowCount -1; row >=0 ; row--)
            {
               
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
                Transform targetPos = colData [row].cardPositionInRow [col].transform;
                newCard.transform.SetParent(targetPos);
                newCard.transform.rotation = Quaternion.Euler(0 , 180f , 0);
                float delay = ( col * rowCount + ( rowCount - 1 - row ) ) * delayIncrement;
                Sequence cardSequence = DOTween.Sequence();
                cardSequence.Append(newCard.transform.DOLocalMove(Vector3.zero , moveDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {

                        newCard.transform.localPosition = Vector3.zero;
                        targetPos.GetComponent<CardPos>().TheOwner = newCard;
                        CalculateObjectsPlaced();
                        if (newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                        {
                            CommandCentre.Instance.SoundManager_.PlaySound("scatter_2");
                        }

                    }));
                cardSequence.PrependInterval(delay);
            }
        }

        isFirstPlay = false;
    }

    void TurboFillGrid ( int columnCount , int rowCount , Deck [] decks )
    {
         CommandCentre.Instance.SoundManager_.PlaySound("cards" , false);
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = rowCount - 1 ; row >= 0 ; row--)
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
                Transform targetPos = colData [row].cardPositionInRow [col].transform;

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
                        if(newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                        {
                            CommandCentre.Instance.SoundManager_.PlaySound("scatter_2");
                        }
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
            for (int row = rowCount - 1 ; row >= 0 ; row--)
            {

                GameObject cardPosHolder = colData [row].cardPositionInRow [col];
                CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
                GameObject card = cardPos.TheOwner;
                if (!card)
                {
                    Deck currentDeck = decks [col];
                    GameObject newCard = currentDeck.DrawCard();

                    cardManager.setUpDemoCards(newCard.GetComponent<Card>() , col , row);

                    currentDeck.ResetDeck();
                    Transform targetPos = colData [row].cardPositionInRow [col].transform; ;
                    newCard.transform.SetParent(targetPos);
                    newCard.transform.rotation = Quaternion.Euler(0 , 180f , 0);
                    float delay = ( col * rowCount + ( rowCount - 1 - row ) ) * delayIncrement;
                    Sequence cardSequence = DOTween.Sequence();
                    cardSequence.Append(newCard.transform.DOLocalMove(Vector3.zero , moveDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {

                            newCard.transform.localPosition = Vector3.zero;
                            targetPos.GetComponent<CardPos>().TheOwner = newCard;
                            CalculateObjectsPlaced();
                            if (newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                            {
                                CommandCentre.Instance.SoundManager_.PlaySound("scatter_2");
                            }

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
            for (int row = rowCount - 1 ; row >= 0 ; row--)
            {

                GameObject cardPosHolder = colData [row].cardPositionInRow [col];
                CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
                GameObject card = cardPos.TheOwner;
                if (!card)
                {

                    Deck currentDeck = decks [col];
                    GameObject newCard = currentDeck.DrawCard();
                    cardManager.setUpDemoCards(newCard.GetComponent<Card>() , col , row);
                    currentDeck.ResetDeck();
                    Transform targetPos = colData [row].cardPositionInRow [col].transform;

                    newCard.transform.SetParent(targetPos);
                    newCard.transform.rotation = Quaternion.Euler(0f , 180f , 0f);

                    Sequence cardSequence = DOTween.Sequence();
                    cardSequence.Append(newCard.transform.DOLocalMove(Vector3.zero , moveDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            if (newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                            {
                                CommandCentre.Instance.SoundManager_.PlaySound("ScatterDrop" , false );
                            }
                            newCard.transform.localPosition = Vector3.zero;
                            targetPos.GetComponent<CardPos>().TheOwner = newCard;
                            CalculateObjectsPlaced();
                        }));
                }
            }
        }
        
    }

    int freeGamerefills = 0;

    void CalculateObjectsPlaced ()
    {
        demoObjectsPlaced++;

        if (demoObjectsPlaced == 9)
        {
            if (!CommandCentre.Instance.TurboManager_.TurboSpin_ && !isRefilling)
            {
                CommandCentre.Instance.SoundManager_.PlaySound("cards" , false);
            }
        }
        if (isDemoGridFilled())
        {
            isRefilling = false;
           
            if (!CommandCentre.Instance.DemoManager_.isScatterSpin)
            {
                if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
                {
                    if (!firstDemoFreeSpin)
                    {
                        if (!secondDemoSpin)
                        {
                            Debug.Log("free game - start");
                            if (demoSequence.spinCount <= 7)
                            {
                                demoSequence.SetUpFreeCards();
                            }
                            else if (demoSequence.spinCount == 8)
                            {
                                refilcount++;
                                if (refilcount >= 2)
                                {
                                    demoSequence.SetUpFreeCards();
                                    refilcount = 0;
                                }
                            }
                            else if (demoSequence.spinCount == 9)
                            {
                                demoSequence.SetUpFreeCards();
                            }
                            else if (demoSequence.spinCount == 10)
                            {
                                demoSequence.SetUpFreeCards();
                            }
                            else if (demoSequence.spinCount == 11)
                            {
                                demoSequence.SetUpFreeCards();
                            }
                            else if(demoSequence.spinCount == 12)
                            {
                                if (refilcount >0)
                                {
                                    demoSequence.SetUpFreeCards();
                                    refilcount = 0;
                                }
                                refilcount++;
                            }
                            else if(demoSequence.spinCount == 13)
                            {
                                demoSequence.SetUpFreeCards();
                            }
                            else if(demoSequence.spinCount == 16)
                            {
                                if (refilcount >= 2)
                                {
                                    demoSequence.SetUpFreeCards();
                                    refilcount = 0;
                                }
                                refilcount++;
                            }else if (demoSequence.spinCount == 17)
                            {
                                if (refilcount > 0)
                                {
                                    demoSequence.SetUpFreeCards();
                                    refilcount = 0;
                                }
                                refilcount++;
                            }
                            else
                            {
                                demoSequence.SetUpFreeCards();
                            }
                        }
                    }
                    firstDemoFreeSpin = false;
                    freeGamerefills++;
                    //Debug.Log($"which refill : {freeGamerefills}");

                    if(freeGamerefills >= 2)
                    {
                        secondDemoSpin = false;
                    }
                    else
                    {
                        secondDemoSpin = true;
                    }
                }
                else
                {
                    demoSequence.SetUpCards();
                }
                
            }
           // Debug.Log("Grid is filled");
            StartCoroutine(CheckAndContinue());
        }
    }


    IEnumerator CheckAndContinue ()
    {
        yield return new WaitUntil(()=>demoSequence.isSetUpCard);
        //Debug.Log($"can refill : {winLoseManager.CanRefill()}");
        if (winLoseManager.CanRefill())
        {
            winLoseManager.DemoWinSequence();
            CommandCentre.Instance.DemoManager_.winIndex++;
        }
        else if (!winLoseManager.CanRefill() && CommandCentre.Instance.DemoManager_.isScatterSpin)
        {
            winLoseManager.DemoWinSequence();
        }
        else
        {
            if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
            {
                if (secondDemoSpin)
                {
                    //setup spin 2 
                    demoSequence.setUpSecondFreeCards();
                }

                if (demoSequence.spinCount == 8)
                {
                    demoSequence.setUpThirdFreeCards();
                    secondCheckPoint = true;
                }
                else if (demoSequence.spinCount == 9)
                {
                    demoSequence.setUpForthFreeCards();
                }
                else if (demoSequence.spinCount == 10)
                {
                    demoSequence.setUpFifthFreeCards();
                }
                else if (demoSequence.spinCount == 11)
                {
                    demoSequence.setUpSixthFreeCards();
                }
                else if(demoSequence.spinCount == 12)
                {
                    demoSequence.setUpSeventhFreeCards();
                }
                else if(demoSequence.spinCount == 15)
                {
                    demoSequence.setUpEighthFreeCards();
                }
                else if(demoSequence.spinCount == 16)
                {
                    demoSequence.setUpNinethFreeCards();
                }

                if(CommandCentre.Instance.FreeGameManager_.FreeSpinCounter == 0)
                {
                    CommandCentre.Instance.FreeGameManager_.DeactivateFreeGame();
                    if (CommandCentre.Instance.DemoManager_.IsDemo)
                    {
                        CommandCentre.Instance.DemoManager_.IsDemo =false;

                    }
                }

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
                        CommandCentre.Instance.DemoManager_.isScatterSpin = true;
                        CommandCentre.Instance.DemoManager_.DemoSequence_.setUpscatterCards();
                        CommandCentre.Instance.DemoManager_.ActivateDemoFeature();
                    }
                }
            }
            CommandCentre.Instance.MainMenuController_.CanSpin = true;
            
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

            if (CommandCentre.Instance.AutoSpinManager_.AutoSpinIndex_ < 1)
            {
                CommandCentre.Instance.AutoSpinManager_.DisableAutoSpin();
                CommandCentre.Instance.AutoSpinManager_.IsAutoSpin = false;
            }
            else
            {
                //Debug.Log("Can auto spin");
                CommandCentre.Instance.MainMenuController_.Spin();
            }
        }

        //check win
    }


    public bool isDemoGridFilled ()
    {
        return demoObjectsPlaced >= totalDemoObjectsToPlace;
    }
}
