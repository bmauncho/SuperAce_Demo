using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class cardsData
{
    [HideInInspector]public string CardName;
    public CardType cardType;
    public Sprite[] card;
}

public class CardManager : MonoBehaviour
{
    APIManager apiManager;
    public Sprite normalCardBg;
    public Sprite goldenCardBg;

    public Sprite normalOutline;
    public Sprite smallJockerOutline;
    public Sprite bigJockerOutline;
    public Sprite [] cards;


    public List<cardsData> cardsDatas = new List<cardsData>();

    private void Start ()
    {
        apiManager = CommandCentre.Instance.APIManager_;
    }

    [ContextMenu("Initialize")]
    public void InitializeCardData ()
    {
        cardsDatas.Clear();

        // Get all values from the CardType enum
        Array cardTypeArray = System.Enum.GetValues(typeof(CardType));

       
        foreach (CardType cardType in cardTypeArray)
        {
            if (cardType == CardType.WILD)
            {
                // Find all wild cards (Scatter, Little_Joker, Big_Joker)
                Sprite [] newMatchingCards = cards
                  .Where(card => card != null &&
                        ( card.name == "LITTLE_JOKER" ||
                        card.name == "BIG_JOKER" ))
                  .ToArray();

                if (newMatchingCards.Length > 0)
                {
                    AddCardType(cardType , newMatchingCards);
                }
                else
                {
                    Debug.LogWarning($"No matching cards found for CardType {cardType}.");
                }
            }
            else
            {
                // Find the matching card by name
                Sprite matchingCard = cards.FirstOrDefault(card => card != null && card.name == cardType.ToString());

                if (matchingCard != null)
                {
                    AddCardType(cardType , new Sprite [] { matchingCard });
                }
                else
                {
                    Debug.LogWarning($"No matching card found for CardType {cardType}.");
                }
            }
        }
    }


    public void SetUpStartCards ( Card card , int col , int row )
    {
        if (card == null)
            return;

        // Define a 2D array of card types for easy lookup
        CardType [,] cardLayout = new CardType [5 , 5];

        // Populate the array using a loop
        for (int i = 0 ; i < 5 ; i++)
        {
            for (int j = 0 ; j < 5 ; j++)
            {
                switch (j)
                {
                    case 0: cardLayout [i , j] = CardType.ACE; break;
                    case 1: cardLayout [i , j] = CardType.KING; break;
                    case 2: cardLayout [i , j] = CardType.QUEEN; break;
                    case 3: cardLayout [i , j] = CardType.JACK; break;
                    case 4: cardLayout [i , j] = CardType.SPADE; break;
                }
            }
        }

        // Ensure the row and col are within bounds
        if (row >= 0 && row < cardLayout.GetLength(0) && col >= 0 && col < cardLayout.GetLength(1))
        {
            card.ActiveCardType = cardLayout [row , col];
            card.showNormalCard(normalCardBg , thecard(card.ActiveCardType));
        }
    }

    void AddCardType ( CardType cardType , Sprite[] theCard )
    {
        cardsDatas.Add(new cardsData()
        {
            CardName = cardType.ToString() ,
            cardType = cardType ,
            card = theCard
        });
    }


    public void setUpCard (Card card,int col , int row )
    {
        if (!card)
            return;

        CardData cardInfo = new CardData()
        {
            name = apiManager.GameDataAPI_.GetCardInfo(col , row).name ,
            golden = apiManager.GameDataAPI_.GetCardInfo(col , row).golden ,
            transformed = apiManager.GameDataAPI_.GetCardInfo(col , row).transformed ,
        };

       // Debug.Log($"card name - {cardInfo.name} : Is it golden - {cardInfo.golden}");

        if (Enum.TryParse(typeof(CardType) , cardInfo.name , out var cardType))
        {
            //Debug.Log($"Successfully parsed card type: {cardType}");
            card.ActiveCardType = (CardType)cardType;

            if (card.ActiveCardType == CardType.WILD)
            {
                int rand = Random.Range(0, 2);
                if(rand <= 1)
                {
                    card.showSmall_Jocker(goldenCardBg , thecard(card.ActiveCardType) , smallJockerOutline);
                }
                else
                {
                    card.showBig_Jocker(goldenCardBg , thecard(card.ActiveCardType) , bigJockerOutline);
                }
            }
            else if (card.ActiveCardType == CardType.SCATTER)
            {
                card.showScatterCard();
            }
            else if (card.ActiveCardType == CardType.LITTLE_JOKER)
            {
                card.showSmall_Jocker(goldenCardBg , thecard(card.ActiveCardType) , smallJockerOutline);
            }
            else if (card.ActiveCardType == CardType.BIG_JOKER)
            {
                card.showBig_Jocker(goldenCardBg , thecard(card.ActiveCardType) , bigJockerOutline);
            }
            else
            {
                if (cardInfo.golden)
                {
                    card.showGoldenCard(goldenCardBg , thecard(card.ActiveCardType) , normalOutline);
                }
                else
                {
                    card.showNormalCard(normalCardBg , thecard(card.ActiveCardType));
                }
            }
        }
        else
        {
            Debug.LogWarning($"Failed to parse card type: {cardInfo.name}");
        }
    }

    public void setUpDemoCards ( Card card , int col , int row )
    {
        if (!card)
            return;

        CardData cardInfo = new CardData()
        {
            name = apiManager.GameDataAPI_.GetCardInfo(col , row).name ,
            golden = apiManager.GameDataAPI_.GetCardInfo(col , row).golden ,
            transformed = apiManager.GameDataAPI_.GetCardInfo(col , row).transformed ,
        };

        // Debug.Log($"card name - {cardInfo.name} : Is it golden - {cardInfo.golden}");

        if (Enum.TryParse(typeof(CardType) , cardInfo.name , out var cardType))
        {
            //Debug.Log($"Successfully parsed card type: {cardType}");
            card.ActiveCardType = (CardType)cardType;

            if (card.ActiveCardType == CardType.WILD)
            {
                int rand = Random.Range(0 , 2);
                if (rand <= 1)
                {
                    card.showSmall_Jocker(goldenCardBg , thecard(card.ActiveCardType) , smallJockerOutline);
                }
                else
                {
                    card.showBig_Jocker(goldenCardBg , thecard(card.ActiveCardType) , bigJockerOutline);
                }
            }
            else if (card.ActiveCardType == CardType.SCATTER)
            {
                card.showScatterCard();
            }
            else if (card.ActiveCardType == CardType.LITTLE_JOKER)
            {
                card.showSmall_Jocker(goldenCardBg , thecard(card.ActiveCardType) , smallJockerOutline);
            }
            else if (card.ActiveCardType == CardType.BIG_JOKER)
            {
                card.showBig_Jocker(goldenCardBg , thecard(card.ActiveCardType) , bigJockerOutline);
            }
            else
            {
                if (cardInfo.golden)
                {
                    card.showGoldenCard(goldenCardBg , thecard(card.ActiveCardType) , normalOutline);
                }
                else
                {
                    card.showNormalCard(normalCardBg , thecard(card.ActiveCardType));
                }
            }
        }
        else
        {
            Debug.LogWarning($"Failed to parse card type: {cardInfo.name}");
        }
    }


    public Sprite thecard ( CardType cardType )
    {
        
        for (int i = 0 ; i < cardsDatas.Count ; i++)
        {
            int rand = Random.Range(0 , cardsDatas [i].card.Length);
            if (cardsDatas [i].cardType == cardType)
            {
                return cardsDatas [i].card [rand];
            }
        }
        Debug.LogWarning($"CardType {cardType} not found in cardsDatas.");
        return null;
    }


}