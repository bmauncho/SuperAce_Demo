using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DemoCardsInfo
{
    public string name;
    public string Subsitute;
    public bool isGolden;
}

[System.Serializable]
public class DemoCards
{
    public List<DemoCardsInfo> cards = new List<DemoCardsInfo>();
}

public class DemoSequence : MonoBehaviour
{
    public List<DemoCards> demoCards = new List<DemoCards>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("SetUp Cards")]
    void SetUpCards ()
    {
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));
        int spinCount = 0;
        for(int i = 0;i<4;i++)
        {
            demoCards.Add(new DemoCards());

            for(int j = 0 ; j < 5 ; j++)
            {
                demoCards [i].cards.Add(new DemoCardsInfo());
                demoCards [i].cards [j] = new DemoCardsInfo();
                demoCards [i].cards [j] = whichSpin(spinCount,j , i);
            }
        }
    }

    public DemoCardsInfo whichSpin(int which,int col,int row)
    {
        var cardsDatas= new DemoCardsInfo ();
        switch (which)
        {
            case 0:
                return cardsDatas = Spin_1(col,row);
            case 1:
                return cardsDatas = Spin_2(col , row);
            default:
                return cardsDatas = null;
        }
    }


    void AddCardType ( CardType cardType ,bool isGolden = false)
    {
        DemoCardsInfo cardsInfo = new DemoCardsInfo
        {
            name = cardType.ToString(),
            isGolden = isGolden

        };
    }

    public DemoCardsInfo Spin_2 (int col , int row ) 
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "HEART", Subsitute = "KING", isGolden = false },
                new DemoCardsInfo { name = "HEART", Subsitute = "BIG_JOKER", isGolden = true },
                new DemoCardsInfo { name = "JACK", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "QUEEN", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "ACE", Subsitute = "", isGolden = false }
            },
            {
                new DemoCardsInfo { name = "QUEEN", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "ACE", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "CLUB", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "ACE", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "KING", Subsitute = "", isGolden = false }
            },
            {
                new DemoCardsInfo { name = "KING", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "QUEEN", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "HEART", Subsitute = "CLUB", isGolden = false }
            },
            {
                new DemoCardsInfo { name = "SPADE", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "ACE", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "HEART", Subsitute = "DIAMOND", isGolden = false },
                new DemoCardsInfo { name = "HEART", Subsitute = "LITTLE_JOKER", isGolden = true },
                new DemoCardsInfo { name = "QUEEN", Subsitute = "", isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo Spin_1 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
        {
            {
                new DemoCardsInfo { name = "HEART", Subsitute = "KING", isGolden = false },
                new DemoCardsInfo { name = "HEART", Subsitute = "BIG_JOKER", isGolden = true },
                new DemoCardsInfo { name = "JACK", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "QUEEN", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "ACE", Subsitute = "", isGolden = false }
            },
            {
                new DemoCardsInfo { name = "QUEEN", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "ACE", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "CLUB", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "ACE", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "KING", Subsitute = "", isGolden = false }
            },
            {
                new DemoCardsInfo { name = "KING", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "QUEEN", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "HEART", Subsitute = "CL", isGolden = false }
            },
            {
                new DemoCardsInfo { name = "SPADE", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "ACE", Subsitute = "", isGolden = false },
                new DemoCardsInfo { name = "HEART", Subsitute = "DIAMOND", isGolden = false },
                new DemoCardsInfo { name = "HEART", Subsitute = "LITTLE_JOKER", isGolden = true },
                new DemoCardsInfo { name = "QUEEN", Subsitute = "", isGolden = false }
            }
        };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo GetDemoCardInfo ( int col , int row )
    {
        DemoCardsInfo info = new DemoCardsInfo();
        info = demoCards [row].cards[col];
        return info;
    }

}
