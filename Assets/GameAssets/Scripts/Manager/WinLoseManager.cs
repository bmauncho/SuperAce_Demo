using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class WinLoseManager : MonoBehaviour
{
    public bool enableSpin = false;
    public bool IsCheckingWin = false;
    public bool IsScatterWin = false;
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
        SpinEnd();
    }

   

    public bool CheckWinCondition ()
    {
        // Check for scatter win condition in columns 1, 3, and 5
        List<GameObject> scatterWinningCards = CheckSimilarScatterCards(columns [0].Cards , columns [1].Cards , columns [2].Cards , columns [3].Cards , columns [4].Cards);
        if (scatterWinningCards != null)
        {
            IsScatterWin =true;
            GetWinningCards(scatterWinningCards);
            Debug.Log("Scatter Win! Cards that made the win: " + string.Join(", ", scatterWinningCards.Select(card => GetCardType(card))));
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

    private List<GameObject> CheckSimilarCards ( params List<GameObject> [] columns )
    {
        // Create a list to store all winning cards
        List<GameObject> winningCards = new List<GameObject>();

        // Iterate through each card in the first column
        foreach (GameObject card in columns [0])
        {
            bool foundInAllColumns = true;
            // Create a temporary list for this iteration
            List<GameObject> tempWinningCards = new List<GameObject> { card };

            // Check for matching cards in all other columns
            for (int i = 1 ; i < columns.Length ; i++)
            {
                // Get all cards in the current column that match the card type
                List<GameObject> matchingCards = columns [i].Where(c => IsSimilarCardType(card , c)).ToList();

                if (matchingCards.Count > 0)
                {
                    // Add all matching cards to the temporary list
                    tempWinningCards.AddRange(matchingCards);
                }
                else
                {
                    foundInAllColumns = false;
                    break;
                }
            }

            // If matching cards were found in all columns, add them to the winningCards list
            if (foundInAllColumns)
            {
                winningCards.AddRange(tempWinningCards);
            }
        }

        // Return the list of winning cards if any were found, otherwise return null
        return winningCards.Count > 0 ? winningCards : null;
    }


    private bool IsSimilarCardType ( GameObject card1 , GameObject card2 )
    {
        string cardType1 = GetCardType(card1);
        string cardType2 = GetCardType(card2);

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

    private List<GameObject> CheckSimilarScatterCards ( params List<GameObject> [] columns )
    {
        // Get all possible combinations of 3 or more columns, always including the first column
        var combinations = GetCombinations(columns.ToList() , 3 , includeFirstColumn: true)
            .Select(combination => combination.ToList()); // Convert IEnumerable<T> to List<T>

        foreach (var combination in combinations)
        {
            // Check for scatter cards in this combination of columns
            var scatterCards = CheckScatterCardsInColumns(combination.ToArray());

            if (scatterCards != null)
            {
                return scatterCards;
            }
        }

        return null;
    }

    private List<GameObject> CheckScatterCardsInColumns ( List<GameObject> [] columns )
    {
        // Iterate through each card in the first column
        foreach (GameObject card in columns [0])
        {
            // Check if the card is a scatter card
            if (IsScatterCard(card))
            {
                bool foundInAllColumns = true;

                // Create a temporary list for this iteration
                List<GameObject> tempScatterCards = new List<GameObject> { card };

                // Check for matching scatter cards in all other columns
                for (int i = 1 ; i < columns.Length ; i++)
                {
                    // Get all scatter cards in the current column that match the card type
                    List<GameObject> matchingScatterCards = columns [i].Where(c => IsScatterCard(c) && GetCardType(c) == GetCardType(card)).ToList();

                    if (matchingScatterCards.Count > 0)
                    {
                        // Add all matching scatter cards to the temporary list
                        tempScatterCards.AddRange(matchingScatterCards);
                    }
                    else
                    {
                        foundInAllColumns = false;
                        break;
                    }
                }

                // If matching scatter cards were found in all columns, return the list
                if (foundInAllColumns)
                {
                    return tempScatterCards;
                }
            }
        }

        return null;
    }

    private IEnumerable<IEnumerable<T>> GetCombinations<T> ( IEnumerable<T> list , int length , bool includeFirstColumn )
    {
        if (length == 1)
        {
            if (includeFirstColumn)
            {
                return list.Take(1).Select(t => new List<T> { t });
            }
            else
            {
                return list.Select(t => new List<T> { t });
            }
        }

        if (includeFirstColumn)
        {
            var firstColumn = list.First();
            var rest = list.Skip(1);

            return GetCombinations(rest , length - 1 , includeFirstColumn: false)
                .Select(t => t.Prepend(firstColumn).ToList());
        }
        else
        {
            return GetCombinations(list , length - 1 , includeFirstColumn: includeFirstColumn)
                 .Select(t => t.Concat(list.Where(o => !t.Contains(o)))).Cast<IEnumerable<T>>();
        }
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

    public void HandleWinCondition ( List<GameObject> winningCards )
    {
        Debug.Log(GetPayLines(winningCards));
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


    public void SpinEnd ()
    {
        if (CheckWinCondition())
        {
            HandleWinCondition(winCards);
        }
        else
        {
            CommandCentre.Instance.ComboManager_.ResetComboCounter();
            enableSpin = true;
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

    public void WinningCardsDoPunchScale ( List<GameObject> winningCards )
    {
        // Convert winningCards to a HashSet for O(1) lookups
        HashSet<GameObject> winningCardsSet = new HashSet<GameObject>(winningCards);

        // Get all card positions from CardsPosHolder
        var cardPositions = CardsPosHolder.GetComponentsInChildren<CardPos>();

        foreach (var cardPos in cardPositions)
        {
            var owner = cardPos.TheOwner;
            if (owner != null && winningCardsSet.Contains(owner))
            {
                cardPos.TheOwner.transform.DOPunchScale(new Vector3(.2f,.2f,.2f) , .5f , 0 , .2f);
            }
        }
    }

    public void PunchScaleActiveCardMasks ( Vector3 punchScale , float duration , int vibrato = 10 , float elasticity = 1f )
    {
        // Ensure that the CardMaskManager is available
        var cardMaskManager = CommandCentre.Instance.CardMaskManager_;
        if (cardMaskManager == null || cardMaskManager.CardMasks.Count == 0)
        {
            Debug.LogError("CardMaskManager or CardMasks not set up correctly.");
            return;
        }

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
                    cardMask.transform.DOPunchScale(punchScale , duration , vibrato , elasticity);
                }
            }
        }
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
        CommandCentre.Instance.CardMaskManager_.Activate();
        ActivateCardMaskForWinningCards();
        yield return new WaitForSeconds(1);
        WinningCardsDoPunchScale(winningCards);
        PunchScaleActiveCardMasks(new Vector3(.2f , .2f , .2f) , .5f , 0 , .2f);
        GetGoldenCards(winningCards);
        if (goldenCards.Count > 0)
        {
            yield return new WaitForSeconds(1);
            RotateGoldenCards();
        }
        yield return new WaitForSeconds(.5f);
        DeactivateWinningCards(winningCards);
        CommandCentre.Instance.CardMaskManager_.DeactivateAllCardMasks();
        CommandCentre.Instance.CardMaskManager_.Deactivate();
        yield return new WaitForSeconds(1);
        winCards.Clear();
        CommandCentre.Instance.GridColumnManager_.CheckAndFillColumns(columns.Count);
    }


    public IEnumerator ShowScatterWinSequence ( List<GameObject> winningCards )
    {
        //Scatter cards rotate
        //screen
        //clear wining cards
        //enable spin
        enableSpin=true;
        IsScatterWin = false;
        winCards.Clear();
        yield return null;
    }

    public int GetPayLines ( List<GameObject> winningCards )
    {
        int payLines = 0;
        int columnsToCheck = GetNumberOfColumnsWithWinningCards(winningCards);
        Debug.Log($"Columns to check : {columnsToCheck}");

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

    private int GetNumberOfColumnsWithWinningCards ( List<GameObject> winningCards )
    {
        return columns.Count(col => col.Cards.Any(card => winningCards.Contains(card)));
    }
}