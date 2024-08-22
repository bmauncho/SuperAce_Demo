using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using DG.Tweening;
using TMPro.Examples;

public class WinLoseManager : MonoBehaviour
{
    public List<GridColumns> columns = new List<GridColumns>();
    public List<GameObject> winCards = new List<GameObject>();
    public bool enableSpin = false;

    public void PopulateGridChecker ( Transform parent )
    {
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
                    Debug.Log("Win! Cards that made the win: " + string.Join(", " , winningCards5.Select(card => GetCardType(card))));
                    return true;
                }
                else
                {
                    GetWinningCards(winningCards4);
                    Debug.Log("Win! Cards that made the win: " + string.Join(", " , winningCards4.Select(card => GetCardType(card))));
                    return true;
                }
            }
            else
            {
                GetWinningCards(winningCards);
                Debug.Log("Win! Cards that made the win: " + string.Join(", " , winningCards.Select(card => GetCardType(card))));
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
        enableSpin = false;
        // Disable the winning cards and remove them from their columns
        foreach (GameObject card in winningCards)
        {
            card.SetActive(false);
        }
        // Reorganize the columns
        Debug.Log("reorganize column cards");
        CommandCentre.Instance.GridColumnManager_.CheckAndFillColumns(columns.Count);
    }
    public void GetWinningCards ( List<GameObject> winningCards )
    {
        if (winCards.Count > 0) { winCards.Clear(); }
        winCards = new List<GameObject>();
        foreach (GameObject card in winningCards)
        {
            winCards.Add(card);
        }
    }
    public void SpinEnd ()
    {
        if (CheckWinCondition())
        {
            HandleWinCondition(winCards);
        }
        else
        {
            enableSpin=true;
            Debug.Log("spin again");
        }
    }
}