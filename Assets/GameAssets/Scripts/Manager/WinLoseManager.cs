using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using DG.Tweening;
using TMPro.Examples;

public class WinLoseManager : MonoBehaviour
{
    public bool enableSpin = false;
    public bool IsCheckingWin = false;
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
                List<GameObject> matchingCards = columns [i].Where(c => GetCardType(c) == GetCardType(card)).ToList();

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

    private string GetCardType ( GameObject card )
    {
        return card.GetComponent<Card>().cardType.ToString();
    }

    public void HandleWinCondition ( List<GameObject> winningCards )
    {
        // Start the coroutine to handle the winning sequence and golden card rotation
        StartCoroutine(WaitForRepositioningAndShowWinningSequence(winningCards));
    }

    private IEnumerator WaitForRepositioningAndShowWinningSequence ( List<GameObject> winningCards )
    {
        // Start the winning sequence once repositioning is complete
        yield return StartCoroutine(ShowWinningSequence(winningCards));
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
                    //Debug.Log($"Applied DOPunchScale to CardMask at index {maskIndex} in column {colIndex}.");
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

    private void RotateGoldenCards ()
    {
        foreach (GameObject goldenCard in goldenCards)
        {
            //.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            goldenCard.transform.DORotate(Vector3.zero , .5f, RotateMode.FastBeyond360);
            Debug.Log($"Rotated golden card {goldenCard.name} by 180 degrees.");
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
            if (IsGoldenCard(card)) // Assuming IsGoldenCard is a function that checks if a card is golden
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
}