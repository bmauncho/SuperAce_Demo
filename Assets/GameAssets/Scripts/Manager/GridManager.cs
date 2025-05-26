using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField]private bool _isRefilling;
    public bool isRefilling
    {
        get => _isRefilling;
        set
        {
            _isRefilling = value;
           // Debug.Log($"isRefilling set to {value} by: {new System.Diagnostics.StackTrace()}");
        }
    }

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

    public GameObject ServerError;
    public ScatterUIFx scatterUIFx_;

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
                    if (cardPos != null)
                    {
                        var card = cardPos.TheOwner;
                        if (card != null)
                        {
                            poolManager.ReturnCard(card);
                            cardPos.TheOwner = null;
                        }
                        else
                        {
                            Debug.LogWarning($"CardPos found: {cardPos.name}, but TheOwner is null.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Transform does not have CardPos: {_obj.name}");
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

            if (CommandCentre.Instance.TurboManager_.IsTurboSpin_)
            {
                TurboFillGrid(columnCount , rowCount , decks);
            }
            else if (CommandCentre.Instance.TurboManager_.IsSuperTurboSpin_)
            {
                SuperTurboFillGrid(columnCount , rowCount , decks);
            }
            else
            {
                StartCoroutine(NormalFillGrid(columnCount , rowCount , decks , delayIncrement));
            }

        }

    }

    IEnumerator NormalFillGrid ( int columnCount , int rowCount , Deck [] decks , float delayIncrement )
    {
        if (decks == null || decks.Length == 0)
        {
            Debug.LogError("Decks array is null or empty.");
            yield break;
        }

        CommandCentre.Instance.SoundManager_.PlaySound("cards" , false);
        if (isFirstPlay)
        {
            CommandCentre.Instance.SoundManager_.startSound();
        }

        if (ScatterColPosition().Count > 1)
        {
            CommandCentre.Instance.CardFxManager_.ActivateCardMask();
            CommandCentre.Instance.CardFxManager_.ActivateAllCardFxMask();
        }

        GameDataAPI gameDataAPI_ = CommandCentre.Instance.APIManager_.GameDataAPI_;

        for (int col = 0 ; col < columnCount ; col++)
        {
            if (col >= decks.Length || decks [col] == null)
            {
                Debug.LogError($"Deck at column {col} is null or out of bounds.");
                continue;
            }
            
            Deck currentDeck = decks [col];
            //Debug.Log($"Contains Scatter: {ScatterColPosition().Contains(col)}");
            int firstScatterCol = ScatterColPosition().Count > 0 ? ScatterColPosition().Min() : -1;
            if (col >= 2 && ScatterColPosition().Count>1 /*&& firstScatterCol != -1*/)
            {
                //Activate bg
          
                DeactivateEffects(col);
                yield return new WaitForSeconds(.5f);
                int rowFinished = 0;
                //activate effect
                ActivateEffects(col);
                for (int row = rowCount - 1 ; row >= 0 ; row--) // Reverse row loop
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
                    float delay = ( col * rowCount + ( rowCount - 1 - row ) ) * delayIncrement+0.2f; // Adjust delay for reversed order

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
                                if (newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                                {
                                    CommandCentre.Instance.SoundManager_.PlaySound("ScatterDrop" , false);
                                }
                            }
                            else
                            {
                                Debug.LogError($"CardPos component is missing on target position at row {row}, col {col}.");
                            }

                            CalculateObjectsPlaced();
                            rowFinished++;
                        }));
                    cardSequence.PrependInterval(delay);
                }

                yield return new WaitUntil(()=> rowFinished == rowCount);
                //yield return new WaitForSeconds(.5f);
                //Deactivate effect
                DeactivateEffects(col);
                if (col > 1)
                {
                    int thecol = col - 1;
                    CommandCentre.Instance.CardFxManager_.DeactivatePerColumn(thecol);

                    for (int j = 0 ; j < 4 ; j++)
                    {
                        if (gameDataAPI_.rows [j].infos [thecol].name == "SCATTER")
                        {
                            CommandCentre.Instance.CardFxManager_.ActivateCardFxMask(j , thecol);
                        }
                    }

                    if (col == 2)
                    {
                        CommandCentre.Instance.CardFxManager_.DeactivatePerColumn(0);
                        for (int j = 0 ; j < 4; j++)
                        {
                            if (gameDataAPI_.rows [j].infos [0].name == "SCATTER")
                            {
                                CommandCentre.Instance.CardFxManager_.ActivateCardFxMask(j , 0);
                            }
                        }
                    }

                }
                else
                {
                    CommandCentre.Instance.CardFxManager_.DeactivatePerColumn(col);
                }

            }
            else
            {
                for (int row = rowCount - 1 ; row >= 0 ; row--) // Reverse row loop
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
                    float delay = ( col * rowCount + ( rowCount - 1 - row ) ) * delayIncrement; // Adjust delay for reversed order

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
                                if (newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                                {
                                    CommandCentre.Instance.SoundManager_.PlaySound("ScatterDrop" , false);
                                }
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
    }
    public List<int> ScatterColPosition ()
    {
        List<int> scatterInCol = new List<int>();
        GameDataAPI gameDataAPI_ = CommandCentre.Instance.APIManager_.GameDataAPI_;

        for (int i = 0 ; i < gameDataAPI_.rows.Count ; i++)
        {
            for (int j = 0 ; j < gameDataAPI_.rows [i].infos.Count ; j++)
            {
                if (gameDataAPI_.rows [i].infos [j].name == "SCATTER")
                {
                    scatterInCol.Add(i);
                    break; // Exit loop early since we found a scatter in this column
                }
            }
        }

        return scatterInCol;
    }

    public bool isSameColumn(int col )
    {
        foreach(var col_ in ScatterColPosition ())
        {
            if( col == col_)
            {
                return true;
            }
        }
        return false;
    }


    void ActivateEffects (int col)
    {
        // Add effect activation logic here
       // Debug.Log("Activating scatter effects.");
        scatterUIFx_.showeffect(col);
    }

    void DeactivateEffects (int col)
    {
        // Add effect deactivation logic here
       // Debug.Log("Deactivating scatter effects.");
        scatterUIFx_.HideEffect(col);
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
        CommandCentre.Instance.SoundManager_.PlaySound("cards" , false);
        if (isFirstPlay)
        {
            CommandCentre.Instance.SoundManager_.startSound();
        }
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = rowCount - 1 ; row >= 0 ; row--)
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
                            if (newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                            {
                                CommandCentre.Instance.SoundManager_.PlaySound("ScatterDrop" , false);
                            }
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

    public void SuperTurboFillGrid ( int columnCount , int rowCount , Deck [] decks )
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
        CommandCentre.Instance.SoundManager_.PlaySound("cards" , false);
        if (isFirstPlay)
        {
            CommandCentre.Instance.SoundManager_.startSound();
        }
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = rowCount - 1 ; row >= 0 ; row--)
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
                            if (newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                            {
                                CommandCentre.Instance.SoundManager_.PlaySound("ScatterDrop" , false);
                            }
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
    public bool IsRefillingSequence = false;
    public bool isRefillingSequence ()
    {
        return IsRefillingSequence;
    }
    public void refillGrid ( int objectshidden )
    {
        IsRefillingSequence = true;
        APIManager apiManager = CommandCentre.Instance.APIManager_;
        isRefilling = true;
        Deck [] decks = multiDeckManager.decks;
        objectsPlaced = totalObjectsToPlace - objectshidden;
        float delayIncrement = 0.1f; // Delay between cards, adjust as needed
        int rowCount = 4; // Number of rows
        int columnCount = decks.Length; // Number of columns
        //CommandCentre.Instance.SoundManager_.PlaySound("cards" , false);
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = rowCount - 1 ; row >= 0 ; row--)
            {

                GameObject cardPosHolder = rowData [row].cardPositionInRow [col];
                CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
                GameObject card = cardPos.TheOwner;
                if (!card)
                {
                    Deck currentDeck = decks [col];
                    GameObject newCard = currentDeck.DrawCard();

                    CardData cardInfo = apiManager.refillCardsAPI_.GetCardInfo(col , row);
                    if (cardInfo.name == "BIG_JOKER" || cardInfo.name == "LITTLE_JOKER")
                    {
                        if (cardPos.TheOwner != null)
                        {
                            continue; // Skip placing this card
                        }
                    }
                    cardManager.SetUpRefillCards(newCard.GetComponent<Card>() , col , row);
                    //Debug.Log(newCard.GetComponent<Card>().ActiveCardType.ToString());
                    currentDeck.ResetDeck();
                    Transform targetPos = rowData [row].cardPositionInRow [col].transform;
                    newCard.transform.SetParent(targetPos);
                    newCard.transform.rotation = Quaternion.Euler(0 , 180f , 0);
                    float delay = ( col * rowCount + ( rowCount - 1 - row ) ) * delayIncrement;
                    Sequence cardSequence = DOTween.Sequence();
                    cardSequence.Append(newCard.transform.DOLocalMove(Vector3.zero , moveDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            if (newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                            {
                                CommandCentre.Instance.SoundManager_.PlaySound("ScatterDrop" , false);
                            }
                            newCard.transform.localPosition = Vector3.zero;
                            targetPos.GetComponent<CardPos>().TheOwner = newCard;
                            CalculateObjectsPlaced();
                        }));
                    cardSequence.PrependInterval(delay);
                }
                cardManager.UpdateGrid(col , row);
            }
        }

    }

    public void refillTurbo (int objectshidden)
    {
        IsRefillingSequence = true;
        APIManager apiManager = CommandCentre.Instance.APIManager_;
        isRefilling = true;
        Deck [] decks = multiDeckManager.decks;
        objectsPlaced = totalObjectsToPlace - objectshidden;
        int rowCount = 4; // Number of rows
        int columnCount = decks.Length; // Number of columns
        //CommandCentre.Instance.SoundManager_.PlaySound("cards" , false);
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = rowCount - 1 ; row >= 0 ; row--)
            {

                GameObject cardPosHolder = rowData [row].cardPositionInRow [col];
                CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
                GameObject card = cardPos.TheOwner;
                if (!card)
                {
                    Deck currentDeck = decks [col];
                    GameObject newCard = currentDeck.DrawCard();
                    CardData cardInfo = apiManager.refillCardsAPI_.GetCardInfo(col , row);
                    if (cardInfo.name == "BIG_JOKER" || cardInfo.name == "LITTLE_JOKER")
                    {
                        if (cardPos.TheOwner != null)
                        {
                            continue; // Skip placing this card
                        }
                    }
                    cardManager.SetUpRefillCards(newCard.GetComponent<Card>() , col , row);
                    currentDeck.ResetDeck();
                    Transform targetPos = rowData [row].cardPositionInRow [col].transform;

                    newCard.transform.SetParent(targetPos);
                    newCard.transform.rotation = Quaternion.Euler(0f , 180f , 0f);

                    Sequence cardSequence = DOTween.Sequence();
                    cardSequence.Append(newCard.transform.DOLocalMove(Vector3.zero , moveDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            if (newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                            {
                                CommandCentre.Instance.SoundManager_.PlaySound("ScatterDrop" , false);
                            }
                            newCard.transform.localPosition = Vector3.zero;
                            targetPos.GetComponent<CardPos>().TheOwner = newCard;
                            CalculateObjectsPlaced();
                        }));

                    cardManager.UpdateGrid(col , row);
                }
            }
        }
    }

    public void refillSuperTurbo ( int objectshidden )
    {
        IsRefillingSequence = true;
        APIManager apiManager = CommandCentre.Instance.APIManager_;
        isRefilling = true;
        Deck [] decks = multiDeckManager.decks;
        objectsPlaced = totalObjectsToPlace - objectshidden;
        int rowCount = 4; // Number of rows
        int columnCount = decks.Length; // Number of columns
        //CommandCentre.Instance.SoundManager_.PlaySound("cards" , false);
        for (int col = 0 ; col < columnCount ; col++)
        {
            for (int row = rowCount - 1 ; row >= 0 ; row--)
            {

                GameObject cardPosHolder = rowData [row].cardPositionInRow [col];
                CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
                GameObject card = cardPos.TheOwner;
                if (!card)
                {
                    Deck currentDeck = decks [col];
                    GameObject newCard = currentDeck.DrawCard();
                    CardData cardInfo = apiManager.refillCardsAPI_.GetCardInfo(col , row);
                    if (cardInfo.name == "BIG_JOKER" || cardInfo.name == "LITTLE_JOKER")
                    {
                        if (cardPos.TheOwner != null)
                        {
                            continue; // Skip placing this card
                        }
                    }
                    cardManager.SetUpRefillCards(newCard.GetComponent<Card>() , col , row);
                    currentDeck.ResetDeck();
                    Transform targetPos = rowData [row].cardPositionInRow [col].transform;

                    newCard.transform.SetParent(targetPos);
                    newCard.transform.rotation = Quaternion.Euler(0f , 180f , 0f);

                    Sequence cardSequence = DOTween.Sequence();
                    cardSequence.Append(newCard.transform.DOLocalMove(Vector3.zero , moveDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            if (newCard.GetComponent<Card>().ActiveCardType == CardType.SCATTER)
                            {
                                CommandCentre.Instance.SoundManager_.PlaySound("ScatterDrop" , false);
                            }
                            newCard.transform.localPosition = Vector3.zero;
                            targetPos.GetComponent<CardPos>().TheOwner = newCard;
                            CalculateObjectsPlaced();
                        }));

                    cardManager.UpdateGrid(col , row);
                }
            }
        }
    }

    void CalculateObjectsPlaced ()
    {
        objectsPlaced++;
        if(objectsPlaced==9)
        {
            if (!isRefilling)
            {
                if (!CommandCentre.Instance.TurboManager_.IsTurboSpin_ ||
                    !CommandCentre.Instance.TurboManager_.IsSuperTurboSpin_)
                {
                    CommandCentre.Instance.SoundManager_.PlaySound("cards" , false);
                }
            }
           
        }

        if (isGridFilled())
        {
            //AddSpins();
            if (isFirstPlay)
            {
                isFirstPlay = false;
                CommandCentre.Instance.MainMenuController_.EnableWinMoreMenu();
                CommandCentre.Instance.MainMenuController_.GameplayMenu.SetActive(true);
            }
           //Debug.Log("Grid is filled");
            StartCoroutine(CheckAndContinue());
        }
    }
    IEnumerator CheckAndContinue ()
    {
        if (isRefilling)
        {
            Debug.Log("Refilling"); 
            yield return StartCoroutine(HandleRefill());
        }

        //if won 
        if (CommandCentre.Instance.WinLoseManager_.IsWin())
        {
            Debug.Log("Handling win");
            yield return StartCoroutine(HandleWin());
        }
        //if lost
        else
        {
            Debug.Log("handling no win");
            yield return StartCoroutine(HandleNoWin());
        }
    }

    IEnumerator HandleRefill ()
    {
        isRefilling = false;
        yield return null;
        yield return new WaitForSeconds(.5f);

        yield return new WaitUntil(() => !CommandCentre.Instance.WinLoseManager_.isWinsequence);

        yield return new WaitForSeconds(.25f);

        if (!CommandCentre.Instance.APIManager_.refillCardsAPI_.isError)
        {
            CommandCentre.Instance.APIManager_.GameDataAPI_.RecheckWin();
        }

        CommandCentre.Instance.APIManager_.refillCardsAPI_.isError = false;
    }

    IEnumerator HandleWin ()
    {
        if (CommandCentre.Instance.WinLoseManager_.checkForOtherCards())
        {
            CommandCentre.Instance.APIManager_.UpdateBet();
            if (!CommandCentre.Instance.FreeGameManager_.IsFreeGame)
            {
                CommandCentre.Instance.CashManager_.updateThecashUi();
            }
            CommandCentre.Instance.WinLoseManager_.winSequence();

            //CommandCentre.Instance.APIManager_.refillCardsAPI_.FetchData();

            //yield return new WaitUntil(() => CommandCentre.Instance.APIManager_.refillCardsAPI_.refillDataFetched);

            //if (!CommandCentre.Instance.APIManager_.refillCardsAPI_.IsServerError)
            //{
            //    CommandCentre.Instance.WinLoseManager_.winSequence();
            //}
            //else
            //{
            //    yield return StartCoroutine(HandleServerError());
            //}
        }
        else
        {
            CommandCentre.Instance.WinLoseManager_.isWinsequence = false;
            CommandCentre.Instance.ComboManager_.ResetComboCounter();
            CommandCentre.Instance.WinLoseManager_.winSequence();
        }
        yield return null;
    }

    IEnumerator HandleNoWin ()
    {
        if (CommandCentre.Instance.CardFxManager_.cardFxMask.activeSelf)
        {
            CommandCentre.Instance.CardFxManager_.DeactivateCardFxMask();
        }

        CommandCentre.Instance.WinLoseManager_.isWinsequence = false;

        int combo = CommandCentre.Instance.ComboManager_.ComboCounter;
        if (!CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {

            if (combo >= 3)
            {
                CommandCentre.Instance.PayOutManager_.ShowTotalWinings();
                yield return new WaitForSeconds(5f);
                CommandCentre.Instance.PayOutManager_.HideTotalWinnings();
            }
        }

        CommandCentre.Instance.MainMenuController_.CanSpin = true;
        isRefilling = false;

        if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            if(CommandCentre.Instance.APIManager_.GameDataAPI_.FreeSpins > 0 && CommandCentre.Instance.APIManager_.GameDataAPI_.FreeSpins < 10)
            {
                CommandCentre.Instance.FreeGameManager_.increaseSpins();
            }
        }

        if (CommandCentre.Instance.APIManager_.betUpdaterAPI_.NewCashAmount > 0 && 
            !CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            Debug.Log($"is free Game {CommandCentre.Instance.FreeGameManager_.IsFreeGame}");
            Debug.Log($"Add wins {CommandCentre.Instance.APIManager_.betUpdaterAPI_.NewCashAmount}");
            CommandCentre.Instance.CashManager_.CashAmount = CommandCentre.Instance.APIManager_.betUpdaterAPI_.NewCashAmount;
            double newCashAmount = CommandCentre.Instance.CashManager_.CashAmount;
            CommandCentre.Instance.CashManager_.UpdateCashAmount((float)newCashAmount);
            CommandCentre.Instance.APIManager_.betUpdaterAPI_.NewCashAmount = 0;
            Debug.Log($"Clear wins {CommandCentre.Instance.APIManager_.betUpdaterAPI_.NewCashAmount}");
            //reset payout manager
            CommandCentre.Instance.PayOutManager_.resetCurrentWinings();
            CommandCentre.Instance.CashManager_.ResetWinings();
        }

        IsRefillingSequence = false;
        yield return StartCoroutine(Autospin());
    }

    IEnumerator HandleServerError ()
    {
        //Debug.Log("SeverError - update Grid");
        ServerError.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        ServerError.SetActive(false);

        CommandCentre.Instance.MainMenuController_.CanSpin = true;
        CommandCentre.Instance.APIManager_.refillCardsAPI_.IsServerError = false;
        yield return StartCoroutine(Autospin());
    }



    IEnumerator Autospin ()
    {
        FreeGameManager freeGameManager = CommandCentre.Instance.FreeGameManager_;
        AutoSpinManager autoSpinManager = CommandCentre.Instance.AutoSpinManager_;
        MainMenuController mainMenuController = CommandCentre.Instance.MainMenuController_;
        PayOutManager payOutManager = CommandCentre.Instance.PayOutManager_;
        yield return new WaitUntil(() => !CommandCentre.Instance.WinLoseManager_.isWinsequence);
        yield return new WaitUntil(() => mainMenuController.CanSpin);
        
        // Ensure FreeGameWinUi is deactivated before proceeding
        if (payOutManager.WinUI_.FreeGameWinUi.activeInHierarchy || payOutManager.WinUI_.FreeGameWinUi.activeSelf)
        {
            //Debug.Log("Deactivate Free Game");
            yield return new WaitUntil(() => !payOutManager.WinUI_.FreeGameWinUi.activeInHierarchy &&
                                             !payOutManager.WinUI_.FreeGameWinUi.activeSelf);
            //Debug.Log("Free Game Deactivated");
        }


        if (autoSpinManager.IsAutoSpin)
        {
            if (autoSpinManager.AutoSpinIndex_ < 1)
            {
                autoSpinManager.AutospinToggle.isOn = false;
                autoSpinManager.IsAutoSpin = false;
            }
            else
            {
                mainMenuController.Spin();
            }
        }
        else
        {
            if (freeGameManager.IsFreeGame)
            {
                if (!freeGameManager.IsSpinInit)
                {
                    freeGameManager.IsSpinInit = true;
                }

                if(freeGameManager.FreeSpinCounter >= 1)
                {

                    mainMenuController.Spin();
                }
                else
                {

                    CommandCentre.Instance.FreeGameManager_.DeactivateFreeGame();
                    CommandCentre.Instance.FreeGameManager_.IsFreeGame = false;
                    Debug.Log("Deactivate Free Game ");
                    yield return new WaitUntil(() => !CommandCentre.Instance.PayOutManager_.WinUI_.FreeGameWinUi.activeInHierarchy &&
                    !CommandCentre.Instance.PayOutManager_.WinUI_.FreeGameWinUi.activeSelf);
                    // Debug.Log("Free Game Deactivated");
                }
            }
        }
    }



    public bool isGridFilled ()
    {
        return objectsPlaced >= totalObjectsToPlace;
    }
}
