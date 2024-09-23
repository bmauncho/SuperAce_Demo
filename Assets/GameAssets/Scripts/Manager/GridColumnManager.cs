using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

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
    public bool hasCheckedWin = false;
    private bool isShaking = false;
    public bool IsDemoFirstRefill = true;
    public bool IsDemoSecondRefill = false;
    public bool IsDemoManupilationComplete = false;
    public int totalObjectsToPlace;
    public int objectsPlaced;
    public int totalObjectsToRotate;
    public int objectsRotated;

    public int totalObjectsToShake;
    public int objectsShaked;
    public int totalObjectstojump;
    public int objectsjumped;


    public GameObject CardPosHolders;
    public List<GameObject> CardList = new List<GameObject>();
    public List<GridPosColumns> Columns = new List<GridPosColumns>();
    public bool [] bigJokerCardsProcessed = new bool [5];
    public bool [] bigJokerCardsToBeProcessed = new bool [5];

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
        hasCheckedWin = false;
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
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            StartCoroutine(DemoRefill(colIndex , responsibleDeck , newCards , cardPosInColumn));
        }
        else
        {
            if (CommandCentre.Instance.TurboManager_.TurboSpin_)
            {
                StartCoroutine(TurboRefill(colIndex , responsibleDeck , newCards , cardPosInColumn));
            }
            else
            {
                StartCoroutine(NormalRefill(colIndex , responsibleDeck , newCards , cardPosInColumn));
            }
        }
        
        yield return null;
    }
    IEnumerator DemoRefill ( int colIndex , Deck responsibleDeck , List<GameObject> newCards , List<GameObject> cardPosInColumn )
    {
        float delayIncrement = 0.05f;
        int rowCount = 4;
        int positionIndex = 0;

        for (int row = 0 ; row < rowCount ; row++)
        {
            GameObject currentCardPos = cardPosInColumn [positionIndex];
            if (!currentCardPos.GetComponent<CardPos>().TheOwner)
            {
                //Debug.Log(colIndex + " : " + row);
                GameObject newCard = null;
                if (IsDemoFirstRefill)
                {
                    switch (colIndex)
                    {
                        case 0:
                            switch (row)
                            {
                                
                                case 0:
                                    newCard = responsibleDeck.DrawSpecificDemoCard_2("King");
                                    
                                    break;
                            }
                            break;
                        case 2:
                            switch (row)
                            {
                                case 3:
                                    newCard = responsibleDeck.DrawSpecificDemoCard_2( "Diamonds");
                                    
                                    break;
                            }
                            break;
                        case 4:
                            switch (row)
                            {
                                case 2:
                                    newCard = responsibleDeck.DrawSpecificDemoCard_2( "Clubs");
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    if (IsDemoSecondRefill)
                    {
                        Debug.Log(colIndex + " : " + row);
                        switch (colIndex)
                        {
                            case 0:
                                switch (row)
                                {
                                    case 1:
                                        newCard = responsibleDeck.DrawSpecificDemoCard_2("Diamonds");
                                        break;
                                }
                                break;
                            case 1:
                                switch (row)
                                {
                                    case 0:
                                        newCard = responsibleDeck.DrawSpecificDemoCard_2("Ace");
                                        break;
                                }
                                break;
                            case 2:
                                switch (row)
                                {
                                    case 1:
                                        newCard = responsibleDeck.DrawSpecificDemoCard_2("Hearts");
                                        break;
                                }
                                break;
                            case 3:
                                switch (row)
                                {
                                    case 0:
                                        newCard = responsibleDeck.DrawSpecificDemoCard_2("Spades");
                                        break;
                                    case 2:
                                        newCard = responsibleDeck.DrawSpecificDemoCard_2("Diamonds");
                                        break;
                                    case 3:
                                        newCard = responsibleDeck.DrawSpecificDemoCard_2("Diamonds");
                                        break;
                                }
                                break;
                            case 4:
                                switch (row)
                                {
                                    case 0:
                                        newCard = responsibleDeck.DrawSpecificDemoCard_2("Diamonds");
                                        break;
                                    case 3:
                                        newCard = responsibleDeck.DrawSpecificDemoCard_2("Clubs");
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        newCard = responsibleDeck.DrawCard();
                    }
                    
                }
                
                newCard.GetComponent<Card>().ScatterCardAnim.enabled = false;
                if (newCard == null)
                {
                    Debug.LogError("Deck is empty, unable to continue refilling.");
                    break;
                }
                CardPos CardPos = currentCardPos.GetComponent<CardPos>();
                if (!CardPos.TheOwner)
                {
                    CardPos.TheOwner = newCard;
                    newCard.transform.localRotation = Quaternion.Euler(0 , 180f , 0);
                    newCard.transform.SetParent(CardPos.transform);
                    Vector3 targetPosition = Vector3.zero;
                    float delay = ( colIndex * rowCount + row ) * delayIncrement;

                    Sequence cardSequence = DOTween.Sequence();
                    cardSequence.Append(newCard.transform.DOLocalMove(targetPosition , CommandCentre.Instance.GridManager_.moveDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            newCard.transform.localPosition = Vector3.zero;
                            ActivateNewCard(newCard);
                            CalculateObjectsPlaced();
                            if (newCard.GetComponent<Card>().cardType == CardType.Scatter)
                            {
                                newCard.GetComponent<Card>().ScatterCardAnim.enabled = true;
                                CommandCentre.Instance.SoundManager_.PlaySound("ScatterCard" , false , .3f);
                            }
                        }));
                    cardSequence.PrependInterval(delay);
                    CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(colIndex);
                }
            }
            positionIndex++;
        }

        MarkRefillComplete(colIndex);

        // If all refills are complete, check win conditions
        if (AreAllRefillColumnsCompleted())
        {
            yield return StartCoroutine(ShakeCards());
        }
    }

    IEnumerator NormalRefill (int colIndex , Deck responsibleDeck , List<GameObject> newCards , List<GameObject> cardPosInColumn  )
    {
        float delayIncrement = 0.05f;
        int rowCount = 4;
        int positionIndex = 0;

        for (int row = 0 ; row < rowCount ; row++)
        {
            GameObject currentCardPos = cardPosInColumn [positionIndex];
            if (!currentCardPos.GetComponent<CardPos>().TheOwner)
            {
                GameObject newCard = responsibleDeck.DrawCard();
                newCard.GetComponent<Card>().ScatterCardAnim.enabled = false;
                if (newCard == null)
                {
                    Debug.LogError("Deck is empty, unable to continue refilling.");
                    break;
                }
                CardPos CardPos = currentCardPos.GetComponent<CardPos>();
                if (!CardPos.TheOwner)
                {
                    CardPos.TheOwner = newCard;
                    newCard.transform.localRotation = Quaternion.Euler(0 , 180f , 0);
                    newCard.transform.SetParent(CardPos.transform);
                    Vector3 targetPosition = Vector3.zero;
                    float delay = ( colIndex * rowCount + row ) * delayIncrement;

                    Sequence cardSequence = DOTween.Sequence();
                    cardSequence.Append(newCard.transform.DOLocalMove(targetPosition , CommandCentre.Instance.GridManager_.moveDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            newCard.transform.localPosition = Vector3.zero;
                            ActivateNewCard(newCard);
                            CalculateObjectsPlaced();
                            if (newCard.GetComponent<Card>().cardType == CardType.Scatter)
                            {
                                newCard.GetComponent<Card>().ScatterCardAnim.enabled = true;
                                CommandCentre.Instance.SoundManager_.PlaySound("ScatterCard" , false , .3f);
                            }
                        }));
                    cardSequence.PrependInterval(delay);
                    CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(colIndex);
                }
            }
            positionIndex++;
        }

        MarkRefillComplete(colIndex);

        // If all refills are complete, check win conditions
        if (AreAllRefillColumnsCompleted())
        {
            yield return StartCoroutine(ShakeCards());
        }
    }

    IEnumerator TurboRefill ( int colIndex , Deck responsibleDeck , List<GameObject> newCards , List<GameObject> cardPosInColumn )
    {
        for (int i = 0 ; i < cardPosInColumn.Count ; i++)
        {
            if (!cardPosInColumn [i].GetComponent<CardPos>().TheOwner)
            {
                GameObject newCard = responsibleDeck.DrawCard();

                if (newCard == null)
                {
                    Debug.LogError("Deck is empty, unable to continue refilling.");
                    break;
                }
                CardPos CardPos = cardPosInColumn [i].GetComponent<CardPos>();
                if (!CardPos.TheOwner)
                {
                    CardPos.TheOwner = newCard;
                    newCard.transform.localRotation = Quaternion.Euler(0 , 180f , 0);
                    newCard.transform.SetParent(CardPos.transform);
                    Vector3 targetPosition = Vector3.zero;

                    newCard.transform.DOLocalMove(targetPosition , 0.01f)
                        .OnComplete(() =>
                        {
                            newCard.transform.localPosition = Vector3.zero;
                            ActivateNewCard(newCard);
                            CalculateObjectsPlaced();
                           
                        });
                    CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(colIndex);
                }
            }
        }

        MarkRefillComplete(colIndex);

        // If all refills are complete, check win conditions
        if (AreAllRefillColumnsCompleted())
        {
            yield return StartCoroutine(ShakeCards());
        }
    }
    public bool CanRotate ()
    {
        if (totalObjectsToRotate > 0)
        {
            return true;
        }
        return false;
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
                    float duration = .5f;
                    float delayIncrement = 0.05f;
                    float delay = ( columnIndex * 4 + cardIndex ) * delayIncrement;
                    if (CommandCentre.Instance.DemoManager_.IsDemo)
                    {
                        if (IsDemoFirstRefill)
                        {
                            switch (columnIndex)
                            {
                                case 1:
                                    switch (cardIndex)
                                    {
                                        case 0:
                                            CommandCentre.Instance.CardManager_.DealBigJocker(owner.transform);
                                            Debug.Log("BigJocker");
                                            break;
                                    }
                                    break;
                                case 3:
                                    switch (cardIndex)
                                    {
                                        case 0:
                                            CommandCentre.Instance.CardManager_.DealSmallJocker(owner.transform);
                                            Debug.Log("SmallJocker");
                                            break;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            Debug.Log("Random Jocker");
                            CommandCentre.Instance.CardManager_.RandomizeDealing_Jocker(owner.transform);
                        }
                    }
                    else
                    {
                        Debug.Log("Random Jocker");
                        CommandCentre.Instance.CardManager_.RandomizeDealing_Jocker(owner.transform);
                    }
                    

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
       
    }

    public bool IsGridGoldenCardsRotationDone()
    {
        if (objectsRotated <= 0)
        {
            return false;
        }
        else if(objectsRotated >= totalObjectsToRotate) 
        {
            return true;
        }
        return false;
    }

    void CheckWin()
    {
        totalObjectsToRotate = 0;
        objectsRotated = 0;

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
            //EnableSpin();
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
        bigJokerCardsToBeProcessed = new bool[numberOfColumns];

        for (int i = 0; i < numberOfColumns; i++)
        {
            columnsToRefill[i] = false;
            refillColumnCompleted[i] = false;
            bigJokerCardsProcessed[i] = false;
            bigJokerCardsToBeProcessed [i] = false;
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

    public bool AreAllBigJokerProcessed ()
    {
        for(int i = 0 ; i < bigJokerCardsToBeProcessed.Length ; i++)
        {
            if (bigJokerCardsToBeProcessed [i] && !bigJokerCardsProcessed [i])
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
    private IEnumerator ShakeCards ()
    {
        yield return new WaitForSeconds(0.5f);
        if (isShaking)
            yield break;

        isShaking = true;
        totalObjectsToShake = CardsToShake().Count;
        objectsShaked = 0;
        List<Tween> activeTweens = new List<Tween>();

        if (totalObjectsToShake >= 1)
        {
            HashSet<GameObject> cards = new HashSet<GameObject>(CardsToShake());

            foreach (GameObject card in cards)
            {
                // Create a sequence for each card to handle the shake
                // Wait until any active rotation tween is complete
                yield return StartCoroutine(WaitForTweenToComplete(card.transform,activeTweens));
            }

            // Wait for all tweens to complete using Coroutine and WaitForCompletion
            foreach (Tween tween in activeTweens)
            {
                yield return tween.WaitForCompletion(); // Wait for each individual tween to complete
            }

            Debug.Log("Shaking complete");
            Debug.Log($"IsObjectShakedComplete : {IsObjectShakedComplete()}");

            if (IsObjectShakedComplete())
            {
                // Proceed with the next animation
                yield return StartCoroutine(CheckForBigJokerAndAnimate());
            }
        }
        else if (totalObjectsToShake <= 0)
        {
            yield return StartCoroutine(CheckForBigJokerAndAnimate());
        }

        isShaking = false;
    }


    private IEnumerator WaitForTweenToComplete ( Transform target , List<Tween> activeTweens )
    {
        int safetyCounter = 10; // Maximum number of checks
        while (DOTween.IsTweening(target , true) && safetyCounter > 0)
        {
            safetyCounter--;
            yield return new WaitForSeconds(0.1f); // Wait slightly longer between checks to prevent CPU overload
        }

        if (safetyCounter == 0)
        {
            Debug.LogWarning($"Loop exited due to safety for card {target.name}, rotation may not be completed.");
        }
        else
        {
            // Set the final rotation if the tween completed successfully
            target.rotation = Quaternion.Euler(0 , 180 , 0);
            Debug.Log($"Card {target.name} rotation tween completed, rotation set to (0, 180, 0).");
        }
        yield return new WaitForSeconds(0.15f);
        yield return StartCoroutine(ShakingAction(target , activeTweens));
    }

    public IEnumerator ShakingAction (Transform Target , List<Tween> activeTweens)
    {
        Debug.Log("ShakingAction");
        Tween shakeTween = Target.transform.DOShakeRotation(0.25f , new Vector3(0 , 0 , 15) , 8 , 90 , true , ShakeRandomnessMode.Harmonic)
                   .OnComplete(() =>
                   {
                       objectsShaked++;
                       Target.transform.rotation = Quaternion.Euler(0 , 180 , 0);
                       Debug.Log($"Card {Target.name} finished shaking.");
                   });
        activeTweens.Add(shakeTween);
        yield return new WaitForSeconds(0.1f);
    }


    public List<GameObject> CardsToShake ()
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

    int cardsToJump ()
    {
        return CardsToShake().Count *2;
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

    public IEnumerator MarkAllCardsToBeProcessed ()
    {
        foreach (GridPosColumns column in Columns)
        {
            foreach (GameObject cardPos in column.CardsPos)
            {
                CardPos cardPosScript = cardPos.GetComponent<CardPos>();

                if (cardPosScript?.TheOwner != null)
                {
                    Card cardScript = cardPosScript.TheOwner.GetComponent<Card>();
                    int columnIndex = Columns.IndexOf(column);
                    if (cardScript != null && cardScript.cardType == CardType.Big_Jocker)
                    {
                        bigJokerCardsToBeProcessed [columnIndex] = true;
                    }
                }
            }
        }

        yield return null;
    }

    private bool isAnimating = false;  // Add a flag to track if the coroutine is running

    private IEnumerator CheckForBigJokerAndAnimate ()
    {
        // Prevent the coroutine from running multiple times concurrently
        if (isAnimating) yield break;
        isAnimating = true;  // Set the flag to indicate the coroutine is running

        yield return StartCoroutine(MarkAllCardsToBeProcessed());

        Debug.Log("Check For BigJokerAndAnimate");

        totalObjectstojump = cardsToJump();
        objectsjumped = 0;

        if (totalObjectstojump >= 1)
        {
            Debug.Log("start Animation");
            var jumpSequence = DOTween.Sequence();
            bool jokerFound = false;  // Track if a Big_Joker has been processed

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
                            if (!bigJokerCardsProcessed [columnIndex] && bigJokerCardsToBeProcessed [columnIndex])
                            {
                                bigJokerCardsProcessed [columnIndex] = true;

                                if (jokerFound) continue;  // Skip if already processed a Big_Joker

                                // Get two new cards from the pool manager
                                GameObject newCard1 = poolManager.GetCard();
                                GameObject newCard2 = poolManager.GetCard();

                                if (newCard1 == null || newCard2 == null) continue;

                                jokerFound = true;  // Mark that we've found and processed a Big_Joker

                                CommandCentre.Instance.CardManager_.DealBigJocker(newCard1.transform);
                                CommandCentre.Instance.CardManager_.DealBigJocker(newCard2.transform);

                                // Set initial positions
                                Vector3 initialPosition = cardPosScript.TheOwner.transform.position;
                                Quaternion initialRotation = cardPosScript.TheOwner.transform.rotation;
                                // Random positions for the new cards
                                if (CommandCentre.Instance.DemoManager_.IsDemo)
                                {
                                    if (IsDemoFirstRefill)
                                    {
                                        int randomColumnIndex1 = 2;
                                        int randomPositionIndex1 = 1;

                                        int randomColumnIndex2 = 4;
                                        int randomPositionIndex2 = 0;
                                       
                                        // Position and parent new cards
                                        newCard1.transform.SetPositionAndRotation(initialPosition , initialRotation);
                                        newCard2.transform.SetPositionAndRotation(initialPosition , initialRotation);

                                        newCard1.SetActive(true);
                                        newCard2.SetActive(true);

                                        newCard1.transform.SetParent(Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].transform);
                                        newCard2.transform.SetParent(Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].transform);

                                        Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].GetComponent<CardPos>().TheOwner.SetActive(false);
                                        Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].GetComponent<CardPos>().TheOwner.SetActive(false);
                                        Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].GetComponent<CardPos>().TheOwner = newCard1;
                                        Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].GetComponent<CardPos>().TheOwner = newCard2;

                                        CommandCentre.Instance.PoolManager_.ReturnAllInactiveCardsToPool();
                                        CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(randomColumnIndex1);
                                        CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(randomColumnIndex2);

                                        // DOTween jump animations
                                        jumpSequence.Join(newCard1.transform.DOJump(
                                            Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].transform.position ,
                                            2.0f , 1 , 1.0f).OnComplete(() =>
                                            {
                                                objectsjumped++;
                                                Debug.Log($"Card 1 jumped: {objectsjumped}/{totalObjectstojump}");
                                                newCard1.transform.localPosition = Vector3.zero;
                                                newCard1.transform.rotation = Quaternion.Euler(0 , 180 , 0);
                                                CommandCentre.Instance.CardManager_.GetAndAssignSprites(newCard1.transform);
                                            }));

                                        jumpSequence.Join(newCard2.transform.DOJump(
                                            Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].transform.position ,
                                            2.0f , 1 , 1.0f).OnComplete(() =>
                                            {
                                                objectsjumped++;
                                                Debug.Log($"Card 2 jumped: {objectsjumped}/{totalObjectstojump}");
                                                newCard2.transform.localPosition = Vector3.zero;
                                                newCard2.transform.rotation = Quaternion.Euler(0 , 180 , 0);
                                                CommandCentre.Instance.CardManager_.GetAndAssignSprites(newCard2.transform);
                                            }));
                                    }
                                    else
                                    {


                                        int randomColumnIndex1 = Random.Range(0 , 5);
                                        int randomPositionIndex1 = Random.Range(0 , Columns [randomColumnIndex1].CardsPos.Count);

                                        int randomColumnIndex2, randomPositionIndex2;
                                        do
                                        {
                                            randomColumnIndex2 = Random.Range(0 , 5);
                                            randomPositionIndex2 = Random.Range(0 , Columns [randomColumnIndex2].CardsPos.Count);
                                        }
                                        while (( randomColumnIndex1 == randomColumnIndex2 && randomPositionIndex1 == randomPositionIndex2 ) ||
                                                ( randomColumnIndex1 == Columns.IndexOf(column) && randomPositionIndex1 == column.CardsPos.IndexOf(cardPos) ) ||
                                                ( randomColumnIndex2 == Columns.IndexOf(column) && randomPositionIndex2 == column.CardsPos.IndexOf(cardPos) ));

                                        // Position and parent new cards
                                        newCard1.transform.SetPositionAndRotation(initialPosition , initialRotation);
                                        newCard2.transform.SetPositionAndRotation(initialPosition , initialRotation);

                                        newCard1.SetActive(true);
                                        newCard2.SetActive(true);

                                        newCard1.transform.SetParent(Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].transform);
                                        newCard2.transform.SetParent(Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].transform);

                                        Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].GetComponent<CardPos>().TheOwner.SetActive(false);
                                        Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].GetComponent<CardPos>().TheOwner.SetActive(false);
                                        Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].GetComponent<CardPos>().TheOwner = newCard1;
                                        Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].GetComponent<CardPos>().TheOwner = newCard2;

                                        CommandCentre.Instance.PoolManager_.ReturnAllInactiveCardsToPool();
                                        CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(randomColumnIndex1);
                                        CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(randomColumnIndex2);

                                        // DOTween jump animations
                                        jumpSequence.Join(newCard1.transform.DOJump(
                                            Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].transform.position ,
                                            2.0f , 1 , 1.0f).OnComplete(() =>
                                            {
                                                objectsjumped++;
                                                Debug.Log($"Card 1 jumped: {objectsjumped}/{totalObjectstojump}");
                                                newCard1.transform.localPosition = Vector3.zero;
                                                newCard1.transform.rotation = Quaternion.Euler(0 , 180 , 0);
                                                CommandCentre.Instance.CardManager_.GetAndAssignSprites(newCard1.transform);
                                            }));

                                        jumpSequence.Join(newCard2.transform.DOJump(
                                            Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].transform.position ,
                                            2.0f , 1 , 1.0f).OnComplete(() =>
                                            {
                                                objectsjumped++;
                                                Debug.Log($"Card 2 jumped: {objectsjumped}/{totalObjectstojump}");
                                                newCard2.transform.localPosition = Vector3.zero;
                                                newCard2.transform.rotation = Quaternion.Euler(0 , 180 , 0);
                                                CommandCentre.Instance.CardManager_.GetAndAssignSprites(newCard2.transform);
                                            }));
                                    }
                                }
                                else
                                {


                                    int randomColumnIndex1 = Random.Range(0 , 5);
                                    int randomPositionIndex1 = Random.Range(0 , Columns [randomColumnIndex1].CardsPos.Count);

                                    int randomColumnIndex2, randomPositionIndex2;
                                    do
                                    {
                                        randomColumnIndex2 = Random.Range(0 , 5);
                                        randomPositionIndex2 = Random.Range(0 , Columns [randomColumnIndex2].CardsPos.Count);
                                    }
                                    while (( randomColumnIndex1 == randomColumnIndex2 && randomPositionIndex1 == randomPositionIndex2 ) ||
                                            ( randomColumnIndex1 == Columns.IndexOf(column) && randomPositionIndex1 == column.CardsPos.IndexOf(cardPos) ) ||
                                            ( randomColumnIndex2 == Columns.IndexOf(column) && randomPositionIndex2 == column.CardsPos.IndexOf(cardPos) ));

                                    // Position and parent new cards
                                    newCard1.transform.SetPositionAndRotation(initialPosition , initialRotation);
                                    newCard2.transform.SetPositionAndRotation(initialPosition , initialRotation);

                                    newCard1.SetActive(true);
                                    newCard2.SetActive(true);

                                    newCard1.transform.SetParent(Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].transform);
                                    newCard2.transform.SetParent(Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].transform);

                                    Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].GetComponent<CardPos>().TheOwner.SetActive(false);
                                    Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].GetComponent<CardPos>().TheOwner.SetActive(false);
                                    Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].GetComponent<CardPos>().TheOwner = newCard1;
                                    Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].GetComponent<CardPos>().TheOwner = newCard2;

                                    CommandCentre.Instance.PoolManager_.ReturnAllInactiveCardsToPool();
                                    CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(randomColumnIndex1);
                                    CommandCentre.Instance.GridManager_.RefreshCurrentColumnCards(randomColumnIndex2);

                                    // DOTween jump animations
                                    jumpSequence.Join(newCard1.transform.DOJump(
                                        Columns [randomColumnIndex1].CardsPos [randomPositionIndex1].transform.position ,
                                        2.0f , 1 , 1.0f).OnComplete(() =>
                                        {
                                            objectsjumped++;
                                            Debug.Log($"Card 1 jumped: {objectsjumped}/{totalObjectstojump}");
                                            newCard1.transform.localPosition = Vector3.zero;
                                            newCard1.transform.rotation = Quaternion.Euler(0 , 180 , 0);
                                            CommandCentre.Instance.CardManager_.GetAndAssignSprites(newCard1.transform);
                                        }));

                                    jumpSequence.Join(newCard2.transform.DOJump(
                                        Columns [randomColumnIndex2].CardsPos [randomPositionIndex2].transform.position ,
                                        2.0f , 1 , 1.0f).OnComplete(() =>
                                        {
                                            objectsjumped++;
                                            Debug.Log($"Card 2 jumped: {objectsjumped}/{totalObjectstojump}");
                                            newCard2.transform.localPosition = Vector3.zero;
                                            newCard2.transform.rotation = Quaternion.Euler(0 , 180 , 0);
                                            CommandCentre.Instance.CardManager_.GetAndAssignSprites(newCard2.transform);
                                        }));
                                }
                            }
                        }
                    }
                }
            }

            // Play the jump sequence and wait for it to complete
            if (jumpSequence.IsActive())
            {
                Debug.Log("Waiting for jump sequence to complete...");
                yield return jumpSequence.Play().WaitForCompletion();
            }

            Debug.Log("Jump complete");
            yield return new WaitForSeconds(0.5f);
            Debug.Log($"Jump Check : {IsObjectsJumpComplete()}");
            
            if (!IsObjectsJumpComplete())
            {
                while (!IsObjectsJumpComplete())
                {
                    objectsjumped++;
                }
            }
           
            if (IsObjectsJumpComplete() && !hasCheckedWin)
            {
                Debug.Log("wincheck");
                CheckWin();
                hasCheckedWin = true;
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            if (!hasCheckedWin)
            {
                
                CheckWin();
                hasCheckedWin = true;
            }
        }
        IsDemoFirstRefill = false;

        if (!IsDemoFirstRefill && IsDemoSecondRefill)
        {
            IsDemoSecondRefill = false;
            IsDemoManupilationComplete = true;
        }

        if (!IsDemoManupilationComplete)
        {
            if (!IsDemoSecondRefill && !IsDemoFirstRefill)
            {
                IsDemoSecondRefill = true;
            }
        }
        else
        {
            if (!CommandCentre.Instance.GridManager_.IsDemoManipulationComplete)
            {
                CommandCentre.Instance.GridManager_.IsDemoSecondTime_ = true;
            }
            
        }
        
       
        isAnimating = false;  // Reset the flag once the coroutine has finished
    }




    public bool IsObjectsJumpComplete ()
    {
        return objectsjumped >= totalObjectstojump;
    }

}
