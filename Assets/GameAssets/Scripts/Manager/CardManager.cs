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
    GridManager gridManager;
    APIManager apiManager;
    DemoManager demoManager;
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
        demoManager = CommandCentre.Instance.DemoManager_;
        gridManager = CommandCentre.Instance.GridManager_;
    }

    [ContextMenu("Initialize")]
    public void InitializeCardData ()
    {
        cardsDatas.Clear();

        // Get all values from the CardType enum
        Array cardTypeArray = System.Enum.GetValues(typeof(CardType));

       
        foreach (CardType cardType in cardTypeArray)
        {
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
                    case 3: cardLayout [i , j] = CardType.WILD; break;
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

        CardData cardInfo = new CardData();
        if (gridManager.isRefilling)
        {
            cardInfo = new CardData()
            {
                name = apiManager.refillCardsAPI_.GetCardInfo(col , row).name ,
                substitute = apiManager.refillCardsAPI_.GetCardInfo(col , row).substitute ,
                golden = apiManager.refillCardsAPI_.GetCardInfo(col , row).golden ,
                transformed = apiManager.refillCardsAPI_.GetCardInfo(col , row).transformed ,
            };

            apiManager.GameDataAPI_.rows [row].infos [col] = cardInfo;
        }
        else
        {
            cardInfo = new CardData()
            {
                name = apiManager.GameDataAPI_.GetCardInfo(col , row).name ,
                substitute = apiManager.GameDataAPI_.GetCardInfo(col , row).substitute ,
                golden = apiManager.GameDataAPI_.GetCardInfo(col , row).golden ,
                transformed = apiManager.GameDataAPI_.GetCardInfo(col , row).transformed ,
            };
        }
       

        Debug.Log($"card name - {cardInfo.name} : Is it golden - {cardInfo.golden}");

        if (Enum.TryParse(typeof(CardType) , cardInfo.name , out var cardType))
        {
            //Debug.Log($"Successfully parsed card type: {cardType}");
            card.ActiveCardType = (CardType)cardType;

            if (card.ActiveCardType == CardType.SCATTER)
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

        DemoCardsInfo cardInfo = new DemoCardsInfo
        {
            name = demoManager.DemoSequence_.GetDemoCardInfo(col,row).name,
            Subsitute = demoManager.DemoSequence_.GetDemoCardInfo(col , row).Subsitute,
            isGolden = demoManager.DemoSequence_.GetDemoCardInfo(col , row).isGolden,

        };

        // Debug.Log($"card name - {cardInfo.name} : Is it golden - {cardInfo.golden}");

        if (Enum.TryParse(typeof(CardType) , cardInfo.name , out var cardType))
        {
            //Debug.Log($"Successfully parsed card type: {cardType}");
            card.ActiveCardType = (CardType)cardType;

            if (card.ActiveCardType == CardType.SCATTER)
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
                if (cardInfo.isGolden)
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