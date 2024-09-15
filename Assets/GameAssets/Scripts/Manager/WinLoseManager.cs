using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;

public class WinLoseManager : MonoBehaviour
{
    public bool enableSpin = false;
    public bool IsCheckingWin = false;
    public bool IsScatterWin = false;
    public bool WinningCardAnimationsComplete_ = false;
    private bool isPunchScaleActiveCardMasksRunning = false;
    private bool isWinningCardsDoPunchScaleRunning = false;
    public bool PunchScaleActiveCardMasksAnimationsComplete_ = false;
    private bool isScatterWinSequenceRunning = false;
    public GameObject CardsPosHolder;
    [Space(10)]
    [Header("Lists")]
    public List<GridColumns> columns = new List<GridColumns>();
    public List<GameObject> winCards = new List<GameObject>();
    public List<GameObject> goldenCards = new List<GameObject>();

    public void PopulateGridChecker ( Transform parent )
    {
        enableSpin = false;
        // Loop through each child in the parent Transform
        columns = new List<GridColumns>(CommandCentre.Instance.GridManager_.Columns);
        Debug.Log("Start Spin End");
        StartCoroutine(SpinEnd());
        Debug.Log("Polulate Grid Checker");
    }

   

    public bool CheckWinCondition ()
    {
        if (CheckScatterWin())
        {
            return true;
        }

        // Check if the first 3 columns have a similar card
        List<GameObject> winningCards = CheckSimilarCards(columns [0].Cards , columns [1].Cards , columns [2].Cards);
        if (winningCards != null)
        {
            // Check if the first 4 columns have a similar card
            List<GameObject> winningCards4 = CheckSimilarCards(columns [0].Cards , columns [1].Cards , columns [2].Cards , columns [3].Cards);
            if (winningCards4 != null)
            {
                // Check if the first 5 columns have a similar card
                List<GameObject> winningCards5 = CheckSimilarCards(columns [0].Cards , columns [1].Cards , columns [2].Cards , columns [3].Cards , columns [4].Cards);
                if (winningCards5 != null)
                {
                    GetWinningCards(winningCards5);
                    //Debug.Log("Win! Cards that made the win: " + string.Join(", " , winningCards5.Select(card => GetCardType(card))));
                    return true;
                }
                else
                {
                    GetWinningCards(winningCards4);
                    //Debug.Log("Win! Cards that made the win: " + string.Join(", " , winningCards4.Select(card => GetCardType(card))));
                    return true;
                }
            }
            else
            {
                GetWinningCards(winningCards);
               // Debug.Log("Win! Cards that made the win: " + string.Join(", " , winningCards.Select(card => GetCardType(card))));
                return true;
            }
        }
        return false;
    }


    public bool CheckScatterWin ()
    {
        // Check if the first 3 columns have a similar card
        List<GameObject> winningScatterCards = CheckScatterCardsInColumns(columns [0].Cards , columns [1].Cards , columns [2].Cards , columns [3].Cards , columns [4].Cards);
        if (winningScatterCards != null)
        {
            IsScatterWin = true;
            GetWinningCards(winningScatterCards);
            Debug.Log("Scatter Win! Cards that made the win: " + string.Join(", " , winningScatterCards.Select(card => GetCardType(card))));
            return true;
        }
        return false;
    }

    private List<GameObject> CheckSimilarCards ( params List<GameObject> [] columns )
    {
        List<GameObject> winningCards = new List<GameObject>();

        // Iterate through each card in the first column
        foreach (GameObject card in columns [0])
        {
            string cardType = GetCardType(card);
            bool isFirstCardWildcard = IsWildcard(card);
            bool foundInAllColumns = true;
            bool wildcardUsed = isFirstCardWildcard;

            // Temporary list to store the matching cards in each column
            List<GameObject> tempWinningCards = new List<GameObject> { card };

            // Start checking from the first column and continue to the next columns
            for (int i = 0 ; i < columns.Length ; i++)
            {
                // Find cards in the current column that match the type of the current card
                var matchingCards = columns [i].Where(c => IsSimilarCardType(cardType , GetCardType(c)) || ( isFirstCardWildcard && IsWildcard(c) )).ToList();

                if (matchingCards.Count > 0)
                {
                    // Add all matching cards to the temp list
                    foreach (var matchingCard in matchingCards)
                    {
                        if (!tempWinningCards.Contains(matchingCard))
                        {
                            tempWinningCards.Add(matchingCard);
                        }
                    }
                }
                else if (!wildcardUsed)
                {
                    // Check if there is a wildcard in this column
                    var wildcard = columns [i].FirstOrDefault(c => IsWildcard(c));
                    if (wildcard != null)
                    {
                        tempWinningCards.Add(wildcard);
                        wildcardUsed = true;
                    }
                    else
                    {
                        foundInAllColumns = false;
                        break;
                    }
                }
                else
                {
                    foundInAllColumns = false;
                    break;
                }
            }

            // Handle the case where the first card is a wildcard
            if (isFirstCardWildcard && foundInAllColumns)
            {
                // Since the first card is a wildcard, it matches any type
                string secondColumnType = GetCardType(columns [1].FirstOrDefault(c => !IsWildcard(c)));

                for (int j = 2 ; j < columns.Length ; j++)
                {
                    var currentColumnCards = columns [j];
                    if (!currentColumnCards.Any(c => IsWildcard(c) || IsSimilarCardType(secondColumnType , GetCardType(c))))
                    {
                        foundInAllColumns = false;
                        break;
                    }
                }
            }

            // If a valid combination was found, add the cards to the winning list
            if (foundInAllColumns)
            {
                if (tempWinningCards.Count > winningCards.Count)
                {
                    winningCards = new List<GameObject>(tempWinningCards);
                }
            }
        }

        // Return the list of winning cards if any were found, otherwise return null
        return winningCards.Count > 0 ? winningCards : null;
    }

    private bool IsSimilarCardType ( string cardType1 , string cardType2 )
    {
        // Check if the card types are the same
        if (cardType1 == cardType2)
        {
            return true;
        }
        // Check if one of the cards is a joker (big or small) and the other is any type
        else if (( cardType1 == "Big_Jocker" || cardType1 == "Small_Jocker" ) || ( cardType2 == "Big_Jocker" || cardType2 == "Small_Jocker" ))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private List<GameObject> CheckScatterCardsInColumns ( params List<GameObject> [] columns )
    {
        // Ensure there are at least three columns to check
        if (columns.Length < 3)
        {
            Debug.LogWarning("Not enough columns to check for scatter cards.");
            return null;
        }

        // Dictionary to track the scatter cards and the columns they appear in
        Dictionary<CardType , List<GameObject>> scatterCards = new Dictionary<CardType , List<GameObject>>();

        // Iterate through each column
        for (int i = 0 ; i < columns.Length ; i++)
        {
            //Debug.Log($"Checking column {i} with {columns [i].Count} cards.");

            foreach (GameObject card in columns [i])
            {
                // Check if the card is a scatter card
                if (IsScatterCard(card))
                {
                    CardType cardType = card.GetComponent<Card>().cardType;
                    //Debug.Log($"Found scatter card of type {cardType} in column {i}.");

                    // If the scatter card type is already tracked, add this card to the list
                    if (scatterCards.ContainsKey(cardType))
                    {
                        scatterCards [cardType].Add(card);
                        //Debug.Log($"Added card to existing list for type {cardType}. Total count: {scatterCards [cardType].Count}");
                    }
                    else
                    {
                        // Otherwise, start a new list for this scatter card type
                        scatterCards [cardType] = new List<GameObject> { card };
                        //Debug.Log($"Started new list for scatter card type {cardType}.");
                    }
                }
            }
        }

        // Check each scatter card type to see if it appears in three or more columns
        foreach (var entry in scatterCards)
        {
            var scatterCardList = entry.Value;

            // Group the cards by column and count distinct columns
            int distinctColumns = scatterCardList.Select(card => GetColumn(card , columns)).Distinct().Count();
            //Debug.Log($"Scatter card type {entry.Key} found in {distinctColumns} distinct columns.");

            if (distinctColumns >= 3)
            {
                //Debug.Log($"Scatter card type {entry.Key} found in three or more columns. Returning the list of cards.");
                return scatterCardList;
            }
        }

        // If no scatter card is found in three or more columns, return null
       // Debug.Log("No scatter card found in three or more columns.");
        return null;
    }

    private int GetColumn ( GameObject card , List<GameObject> [] columns )
    {
        for (int i = 0 ; i < columns.Length ; i++)
        {
            if (columns [i].Contains(card))
            {
                return i;
            }
        }
        return -1;
    }

    private bool IsScatterCard ( GameObject card )
    {
        // Check if the card type is CardType.Scatter
        return card.GetComponent<Card>().cardType == CardType.Scatter;
    }

    private string GetCardType ( GameObject card )
    {
        return card.GetComponent<Card>().cardType.ToString();
    }

    private bool IsWildcard ( GameObject card )
    {
        return card.GetComponent<Card>().cardType == CardType.Big_Jocker ||
            card.GetComponent<Card>().cardType == CardType.Small_Jocker;
    }

    public void HandleWinCondition ( List<GameObject> winningCards )
    {
        Debug.Log(string.Join(", " , GetWinningCardType()));

        Debug.Log(GetPayLines());
        // Start the coroutine to handle the winning sequence and golden card rotation
        StartCoroutine(WaitForRepositioningAndShowWinningSequence(winningCards));
    }

    private IEnumerator WaitForRepositioningAndShowWinningSequence ( List<GameObject> winningCards )
    {

        if (IsScatterWin)
        {
            yield return StartCoroutine(ShowScatterWinSequence(winningCards));
        }
        else
        {
            Debug.Log("IncreaseComboCounter");
            CommandCentre.Instance.ComboManager_.IncreaseComboCounter();
            // Start the winning sequence once repositioning is complete
            yield return StartCoroutine(ShowWinningSequence(winningCards));
        }

    }

    public void GetWinningCards ( List<GameObject> winningCards )
    {
        winCards.Clear();

        // Use a HashSet to ensure unique cards
        HashSet<GameObject> uniqueWinningCards = new HashSet<GameObject>(winningCards);

        winCards.AddRange(uniqueWinningCards);
    }


    public IEnumerator SpinEnd ()
    {
        Debug.Log("SpinEnd");
        if (CheckWinCondition())
        {
            Debug.Log("Spin Won");
            HandleWinCondition(winCards);
            IsScatterWin = false;
        }
        else
        {
            //if Combo >= 3 && <= 5 show win screen
            //Debug.Log($"the combo is X{CommandCentre.Instance.ComboManager_.GetCombo()} the real combo is{CommandCentre.Instance.ComboManager_.ComboCounter}");
            yield return StartCoroutine(ShowTotalWins());
            CommandCentre.Instance.ComboManager_.ResetComboCounter();
            Debug.Log("SpinAgain");
            enableSpin = true;

            float timeout = 12f; // Maximum time to wait in seconds
            float timer = 0f;

            while (CommandCentre.Instance.MainMenuController_.isBtnPressed && timer < timeout)
            {
                yield return null;
                timer += Time.deltaTime;
            }

            if (timer >= timeout)
            {
                Debug.LogWarning("Button was not pressed within the timeout period.");
                // Handle timeout case here, like forcing a spin or showing a message to the player
            }

            
            if (CommandCentre.Instance.AutoSpinManager_.IsAutoSpin)
            {
                if (CommandCentre.Instance.AutoSpinManager_.AutoSpinIndex_>=1)
                {
                    yield return new WaitForSeconds(1f);
                    Debug.Log("AutoSpin");
                    CommandCentre.Instance.MainMenuController_.Spin();
                }
                else
                {
                    CommandCentre.Instance.AutoSpinManager_.IsAutoSpin = false;
                    CommandCentre.Instance.AutoSpinManager_.Autospin.AutoSpinToggle.isOn = false;
                }
               
            }
            else
            {
                yield return null;
            }

        }
    }

    IEnumerator ShowTotalWins ()
    {
        if (CommandCentre.Instance.ComboManager_.ComboCounter >= 3)
        {
            Debug.Log("ShowTotalWinings");
            CommandCentre.Instance.PayOutManager_.ShowTotalWinings();
            CommandCentre.Instance.CashManager_.IncreaseCash(CommandCentre.Instance.PayOutManager_.CurrentWin);
        }

        // Timeout settings
        float timeout = 12f; // Maximum time to wait (in seconds)
        float timer = 0f;

        // Wait until the WinUI is no longer showing or until the timeout occurs
        while (CommandCentre.Instance.PayOutManager_.WinUI_.IsShowingTotalWinings && timer < timeout)
        {
            yield return null;  // Wait for the next frame
            timer += Time.deltaTime;  // Increase the timer
        }

        if (timer >= timeout)
        {
            Debug.LogWarning("Timeout reached while waiting for WinUI to close.");
            // Handle timeout case here, if necessary
            // e.g., Force close WinUI or proceed regardless of the UI state.
        }

    }

    public void ActivateCardMaskForWinningCards ()
    {
        // Ensure that the CardMaskManager is available
        var cardMaskManager = CommandCentre.Instance.CardMaskManager_;
        if (cardMaskManager == null || cardMaskManager.CardMasks.Count == 0)
        {
            Debug.LogError("CardMaskManager or CardMasks not set up correctly.");
            return;
        }

        // Loop through each column in the WinLoseManager
        for (int colIndex = 0 ; colIndex < columns.Count ; colIndex++)
        {
            var gridColumn = columns [colIndex];
            var cardMaskColumn = cardMaskManager.CardMasks [colIndex];

            // Loop through each card in the grid column
            for (int cardIndex = 0 ; cardIndex < gridColumn.Cards.Count ; cardIndex++)
            {
                GameObject card = gridColumn.Cards [cardIndex];
                GameObject cardMask = cardMaskColumn.cardMasks [cardIndex];

                // Check if the card is a winning card and has a corresponding card mask
                if (winCards.Contains(card) && cardMask != null)
                {
                    // Activate the card mask
                    cardMask.SetActive(true);
                }
                else if (cardMask != null)
                {
                    // Deactivate the card mask if it's not a winning card
                    cardMask.SetActive(false);
                }
            }
        }
    }

    public void DeactivateWinningCards ( List<GameObject> winningCards )
    {
        enableSpin = false;
        // Convert winningCards to a HashSet for O(1) lookups
        HashSet<GameObject> winningCardsSet = new HashSet<GameObject>(winningCards);

        // Get all card positions from CardsPosHolder
        var cardPositions = CardsPosHolder.GetComponentsInChildren<CardPos>();

        foreach (var cardPos in cardPositions)
        {
            var owner = cardPos.TheOwner;
            if (owner != null && winningCardsSet.Contains(owner))
            {
                owner.SetActive(false);
                cardPos.TheOwner = null;
            }
        }

        CommandCentre.Instance.PoolManager_.ReturnAllInactiveCardsToPool();
        
    }

    public IEnumerator WinningCardsDoPunchScale ( List<GameObject> winningCards )
    {
        if (isWinningCardsDoPunchScaleRunning) yield break; // Prevent multiple calls
        isWinningCardsDoPunchScaleRunning = true;
        // Convert winningCards to a HashSet for O(1) lookups
        HashSet<GameObject> winningCardsSet = new HashSet<GameObject>(winningCards);

        // Get all card positions from CardsPosHolder
        var cardPositions = CardsPosHolder.GetComponentsInChildren<CardPos>();

        int totalAnimations = 0;
        int completedAnimations = 0;
        List<Tween> activeTweens = new List<Tween>();
        foreach (var cardPos in cardPositions)
        {
            var owner = cardPos.TheOwner;
            if (owner != null && winningCardsSet.Contains(owner))
            {
                totalAnimations++; // Increment the total animation count
                activeTweens.Add(cardPos.transform.DOPunchScale(new Vector3(.1f , .1f , .1f) , .5f , 8 , 1)
                    .OnComplete(() =>
                    {
                        cardPos.transform.localScale = Vector3.one;
                        completedAnimations++; // Increment the completed animation count
                    }));
            }
        }
        foreach (Tween tween in activeTweens)
        {
            Debug.Log("Waiting for all objects to finish PunchScale tweening...");
            yield return tween.WaitForCompletion();
        }
        Debug.Log($"completed anims - {completedAnimations} : Total anims - {totalAnimations}");
        // Check if all animations are done
        if (completedAnimations == totalAnimations)
        {
             WinningCardAnimationsComplete_=AllWinningCardAnimationsComplete();
        }
        isWinningCardsDoPunchScaleRunning = false; // Reset the flag
    }

    // Method to call when all winning card animations are complete
    private bool AllWinningCardAnimationsComplete ()
    {
        return true;
    }


    public IEnumerator PunchScaleActiveCardMasks ()
    {
        if (isPunchScaleActiveCardMasksRunning) yield break; // Prevent multiple calls
        isPunchScaleActiveCardMasksRunning = true;
        // Ensure that the CardMaskManager is available
        var cardMaskManager = CommandCentre.Instance.CardMaskManager_;
        if (cardMaskManager == null || cardMaskManager.CardMasks.Count == 0)
        {
            Debug.LogError("CardMaskManager or CardMasks not set up correctly.");
            yield break;
        }

        int totalAnimations = 0;
        int completedAnimations = 0;
        List<Tween> activeTweens = new List<Tween>();
        // Loop through each column in the CardMaskManager
        for (int colIndex = 0 ; colIndex < cardMaskManager.CardMasks.Count ; colIndex++)
        {
            var cardMaskColumn = cardMaskManager.CardMasks [colIndex];

            // Loop through each card mask in the column
            for (int maskIndex = 0 ; maskIndex < cardMaskColumn.cardMasks.Count ; maskIndex++)
            {
                GameObject cardMask = cardMaskColumn.cardMasks [maskIndex];

                // Apply punch scale animation to active card masks
                if (cardMask != null && cardMask.activeInHierarchy)
                {
                    totalAnimations++; // Increment the total animation count

                   activeTweens.Add(cardMask.transform.DOPunchScale(new Vector3(.1f , .1f , .1f) , .5f , 8 , 1)
                       .OnComplete(() =>
                       {
                           cardMask.transform.localScale = Vector3.one;
                           completedAnimations++; // Increment the completed animation count
                       }));
                }
            }
        }
        foreach (Tween tween in activeTweens)
        {
            Debug.Log("Waiting for all objects to finish PunchScale tweening...");
            yield return tween.WaitForCompletion();
        }
        Debug.Log($"completed anims - {completedAnimations} : Total anims - {totalAnimations}");
        // Check if all animations are done
        if (completedAnimations == totalAnimations)
        {
            PunchScaleActiveCardMasksAnimationsComplete_=AllPunchScaleActiveCardMasksAnimationsComplete();
        }
        isPunchScaleActiveCardMasksRunning = false; // Reset the flag
    }

    // Method to call when all animations are complete
    private bool AllPunchScaleActiveCardMasksAnimationsComplete ()
    {
        return true;
    }


    public bool IsGoldenCard(GameObject card )
    {
        if (card)
        {
            if (card.GetComponent<Card>().IsGoldenCard)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsJockerCards(GameObject card )
    {
        if (card)
        {
            if (card.GetComponent<Card>().IsBigJocker|| card.GetComponent<Card>().IsSmallJocker)
            {
                return true;
            }
        }
        return false;
    }

    private void RotateGoldenCards ()
    {
        foreach (GameObject goldenCard in goldenCards)
        {
            goldenCard.transform.DORotate(Vector3.zero , .5f, RotateMode.FastBeyond360);
        }
    }

    public void GetGoldenCards ( List<GameObject> winningCards )
    {
        goldenCards.Clear();
        // Use a HashSet to track added cards and avoid duplicates
        HashSet<GameObject> uniqueGoldenCards = new HashSet<GameObject>();
        List<GameObject> cardsToRemove = new List<GameObject>();

        // Check for golden cards
        foreach (GameObject card in winningCards)
        {
            if (IsGoldenCard(card) && !IsJockerCards(card)) // Assuming IsGoldenCard is a function that checks if a card is golden
            {
                if (!uniqueGoldenCards.Contains(card))
                {
                    uniqueGoldenCards.Add(card);
                    goldenCards.Add(card);
                    cardsToRemove.Add(card);
                }
            }
        }
        // Remove golden cards from the winning cards list
        foreach (GameObject card in cardsToRemove)
        {
            winningCards.Remove(card);
        }
    }

    public IEnumerator ShowWinningSequence( List<GameObject> winningCards )
    {
        yield return new WaitForSeconds(.25f);
        CommandCentre.Instance.CardMaskManager_.Activate();
        ActivateCardMaskForWinningCards();
        yield return new WaitForSeconds(1);
        CommandCentre.Instance.PayOutManager_.ShowCurrentWin();
        CommandCentre.Instance.CommentaryManager_.PlayCommentary(winningCards);
        // Start WinningCardsDoPunchScale and mark it as complete when done
        StartCoroutine(WinningCardsDoPunchScale(winningCards));
        // Start PunchScaleActiveCardMasks and mark it as complete when done
        StartCoroutine(PunchScaleActiveCardMasks());

        while(!PunchScaleActiveCardMasksAnimationsComplete_ && !WinningCardAnimationsComplete_)
        {
            yield return null;
        }

        GetGoldenCards(winningCards);
        if (goldenCards.Count > 0)
        {
            yield return new WaitForSeconds(1);
            RotateGoldenCards();
        }
        yield return new WaitForSeconds(1);
        PunchScaleActiveCardMasksAnimationsComplete_ = false;
        WinningCardAnimationsComplete_ = false;
        CommandCentre.Instance.PayOutManager_.HideCurrentWin();
        CommandCentre.Instance.CardMaskManager_.DeactivateNormalcards();
        DeactivateWinningCards(winningCards);
        yield return new WaitForSeconds(1);
        winCards.Clear();
        CommandCentre.Instance.CardMaskManager_.DeactivateAllCardMasks();
        CommandCentre.Instance.CardMaskManager_.Deactivate();
        CommandCentre.Instance.GridColumnManager_.CheckAndFillColumns(columns.Count);
    }


    public IEnumerator ShowScatterWinSequence ( List<GameObject> winningCards )
    {
        // Check if the coroutine is already running
        if (isScatterWinSequenceRunning)
            yield break;

        isScatterWinSequenceRunning = true;

        yield return new WaitForSeconds(.25f);

        if (!CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            CommandCentre.Instance.FreeGameManager_.ActivateFreeGame();
            Debug.Log("Free Game Enabled");
            winCards.Clear();
            yield return new WaitForSeconds(3);
            CommandCentre.Instance.FreeGameManager_.DeactivateFreeGameIntro();

        }
        else
        {
            CommandCentre.Instance.FreeGameManager_.resetFreeSpins();
            winCards.Clear();
        }
        PopulateGridChecker(CommandCentre.Instance.GridManager_.CardsParent.transform);
        Debug.Log("Free Game Disabled");
        enableSpin = true;

        float timeout = 12f; // Maximum time to wait in seconds
        float timer = 0f;

        while (CommandCentre.Instance.MainMenuController_.isBtnPressed && timer < timeout)
        {
            yield return null;
            timer += Time.deltaTime;
        }

        if (timer >= timeout)
        {
            Debug.LogWarning("Button was not pressed within the timeout period.");
            // Handle timeout case here, like forcing a spin or showing a message to the player
        }

        if (CommandCentre.Instance.AutoSpinManager_.IsAutoSpin)
        {
            if (CommandCentre.Instance.AutoSpinManager_.AutoSpinIndex_ >= 1)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log("AutoSpin");
                CommandCentre.Instance.MainMenuController_.Spin();
            }
            else
            {
                CommandCentre.Instance.AutoSpinManager_.IsAutoSpin = false;
                CommandCentre.Instance.AutoSpinManager_.Autospin.AutoSpinToggle.isOn = false;
            }

        }
        else
        {
            yield return null;
        }
        // Reset the flag once the coroutine has finished
        isScatterWinSequenceRunning = false;
    }

    public int GetPayLines ()
    {
        int payLines = 0;
        int columnsToCheck = GetNumberOfColumnsWithWinningCards();
        //Debug.Log($"Columns to check : {columnsToCheck}");

        // Check for row-based paylines across columns
        // Check if the first 3 columns have a similar card
        if (columnsToCheck >= 3 && CheckSimilarCards(columns [0].Cards , columns [1].Cards , columns [2].Cards) != null)
        {
            payLines++;

            // Check if the first 4 columns have a similar card
            if (columnsToCheck >= 4 && CheckSimilarCards(columns [0].Cards , columns [1].Cards , columns [2].Cards , columns [3].Cards) != null)
            {
                payLines++;

                // Check if the first 5 columns have a similar card
                if (columnsToCheck == 5 && CheckSimilarCards(columns [0].Cards , columns [1].Cards , columns [2].Cards , columns [3].Cards , columns [4].Cards) != null)
                {
                    payLines++;
                }
            }
        }

        // Return the total number of pay lines detected
        return payLines;
    }

    public int GetNumberOfColumnsWithWinningCards ()
    {
        return columns.Count(col => col.Cards.Any(card => winCards.Contains(card)));
    }

    public List<int> GetNumberOfWinningCards ()
    {
        List<int> similarWinningCardCounts = new List<int>();
        if (winCards.Count == 0)
        {
            //Debug.Log("No Winning Cards");
            return similarWinningCardCounts;
        }
        int wildCardCount = 0; 
        Dictionary<CardType , int> cardTypeCounts = new Dictionary<CardType , int>();

        foreach (GameObject cardObj in winCards)
        {
            Card card = cardObj.GetComponent<Card>();
            CardType cardType = card.cardType;

            // Count the occurrence of each card type, including wild cards
            if (cardType == CardType.Big_Jocker || cardType == CardType.Small_Jocker)
            {
                // If it's a wild card, increment a wild card count
                wildCardCount++;
            }
            else
            {
                // Count the occurrence of each card type
                if (cardTypeCounts.TryGetValue(cardType , out int count))
                {
                    cardTypeCounts [cardType] = count + 1;
                }
                else
                {
                    cardTypeCounts [cardType] = 1;
                }
            }
        }

        // If there are multiple card types, return the count of each type
        if (cardTypeCounts.Count > 1)
        {
            foreach (var entry in cardTypeCounts)
            {
                if(entry.Key == CardType.Big_Jocker || entry.Key == CardType.Small_Jocker)
                {
                    cardTypeCounts.Remove(entry.Key);
                }
                similarWinningCardCounts.Add(entry.Value + wildCardCount);
            }

            
            foreach (var entry in cardTypeCounts)
            {
                Debug.Log($"{entry.Key}: {entry.Value + wildCardCount} cards");
            }

            //Debug.Log("Mixed Winning Card Types:" + string.Join(", " , similarWinningCardCounts));

            return similarWinningCardCounts;
        }

        // If there's only one card type, return its count
        if (cardTypeCounts.Count == 1)
        {
            CardType singleType = cardTypeCounts.Keys.First();
            similarWinningCardCounts.Add(cardTypeCounts [singleType] + wildCardCount);
            //Debug.Log($"Single Winning Card Type: {singleType} : {cardTypeCounts [singleType]} cards");

            return similarWinningCardCounts;
        }

        return similarWinningCardCounts;
    }


    public List<string> GetWinningCardType ()
    {
        List<string> result = new List<string>();

        if (winCards.Count == 0)
        {
            //Debug.Log("No Winning Cards");
            return result;
        }

        int wildCardCount = 0; // To count the number of wild cards
        Dictionary<CardType , int> cardTypeCounts = new Dictionary<CardType , int>(); // To store counts of each card type

        foreach (GameObject cardObj in winCards)
        {
            Card card = cardObj.GetComponent<Card>();
            CardType cardType = card.cardType;

            // Check if the card is a wild card
            if (cardType == CardType.Big_Jocker || cardType == CardType.Small_Jocker)
            {
                wildCardCount++; // Count the wild card
                continue; // Move to the next card
            }

            // Count the occurrence of each card type
            if (cardTypeCounts.ContainsKey(cardType))
            {
                cardTypeCounts [cardType]++;
            }
            else
            {
                cardTypeCounts [cardType] = 1;
            }
        }

        // Check if all cards are wild cards
        if (cardTypeCounts.Count == 0 && wildCardCount == winCards.Count)
        {
            //Debug.Log("No Winning Card Type"); // All cards are wild, not a winning set
            return result;
        }

        // If there are multiple card types, return them as mixed types
        if (cardTypeCounts.Count > 1)
        {
            
            result.AddRange(cardTypeCounts.Keys.Select(cardType => cardType.ToString()));
           // Debug.Log("Mixed Winning Card Types:" + string.Join(", " , result.Select(cardType => cardType.ToString())));
            if (wildCardCount > 0)
            {
                //Debug.Log($"{wildCardCount} wild card(s) included");
            }
            return result;
        }

        // If there is only one card type, return it along with any wild cards
        if (cardTypeCounts.Count == 1)
        {
            CardType singleType = cardTypeCounts.Keys.First();
            result.Add(singleType.ToString());
            if (wildCardCount > 0)
            {
               // Debug.Log($"{wildCardCount} wild card(s) included");
            }
           // Debug.Log("Single Winning Card Type:" + string.Join(", " , result.Select(cardType => cardType.ToString())));
            return result;
        }

        //Debug.Log("No Winning Card Type");
        return result;
    }
}