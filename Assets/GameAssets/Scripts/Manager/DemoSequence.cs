using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DemoCardsInfo
{
    public string name;
    public Substitute _Subsitute;
    public bool isGolden;
}

[System.Serializable] 
public class Substitute
{
    public string subsitute_;
    public bool isGolden;
}

[System.Serializable]
public class DemoCards
{
    public List<DemoCardsInfo> cards = new List<DemoCardsInfo>();
}

public class DemoSequence : MonoBehaviour
{
    public int spinCount = 0;
    public List<DemoCards> demoCards = new List<DemoCards>();
    public bool isSetUpCard = true;
    public bool isScattersetUpDone = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("SetUp Cards")]
    public void SetUpCards ()
    {
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));
        bool hasSetUp = false;
        for (int i = 0 ; i < 4 ; i++)
        {
            demoCards.Add(new DemoCards());
            for (int j = 0 ; j < 5 ; j++)
            {
                // Ensure nested objects are initialized
                if (demoCards [i].cards == null)
                {
                    demoCards [i].cards = new List<DemoCardsInfo>();
                }
                if (whichSpin(spinCount , j , i) != null)
                {
                    demoCards [i].cards.Add(whichSpin(spinCount , j , i));
                    hasSetUp = true;
                }

            }
        }
        if (hasSetUp)
        {
            spinCount++;
        }
    }

    [ContextMenu("SetUp scatterCards")]
    public void setUpscatterCards ()
    {
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));

        for (int i = 0 ; i < 4 ; i++)
        {
            demoCards.Add(new DemoCards());
            for (int j = 0 ; j < 5 ; j++)
            {
                // Ensure nested objects are initialized
                if (demoCards [i].cards == null)
                {
                    demoCards [i].cards = new List<DemoCardsInfo>();
                }

                demoCards [i].cards.Add(scatterspinSpin( j , i));
            }
        }

        if (!isScattersetUpDone)
        {
            isScattersetUpDone = true;
        }
    }

    public void setUpFirstFreeCards ()
    {
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));

        for (int i = 0 ; i < 4 ; i++)
        {
            demoCards.Add(new DemoCards());
            for (int j = 0 ; j < 5 ; j++)
            {
                // Ensure nested objects are initialized
                if (demoCards [i].cards == null)
                {
                    demoCards [i].cards = new List<DemoCardsInfo>();
                }

                demoCards [i].cards.Add(firstfreeSpin(j , i));
            }
        }
    }

    public void setUpSecondFreeCards ()
    {
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));

        for (int i = 0 ; i < 4 ; i++)
        {
            demoCards.Add(new DemoCards());
            for (int j = 0 ; j < 5 ; j++)
            {
                // Ensure nested objects are initialized
                if (demoCards [i].cards == null)
                {
                    demoCards [i].cards = new List<DemoCardsInfo>();
                }

                demoCards [i].cards.Add(secondfreeSpin(j , i));
            }
        }
    }

    public void SetUpFreeCards ()
    {
        Debug.Log("called");
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));
        bool hasSetUp = false;
        for (int i = 0 ; i < 4 ; i++)
        {
            demoCards.Add(new DemoCards());
            for (int j = 0 ; j < 5 ; j++)
            {
                // Ensure nested objects are initialized
                if (demoCards [i].cards == null)
                {
                    demoCards [i].cards = new List<DemoCardsInfo>();
                }
                Debug.Log(whichFreeSpin(spinCount , j , i));
                demoCards [i].cards.Add(whichFreeSpin(spinCount , j , i));
                hasSetUp = true;

            }
        }
        if (hasSetUp)
        {
            spinCount++;
        }
    }

    public void setUpThirdFreeCards ()
    {
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));

        for (int i = 0 ; i < 4 ; i++)
        {
            demoCards.Add(new DemoCards());
            for (int j = 0 ; j < 5 ; j++)
            {
                // Ensure nested objects are initialized
                if (demoCards [i].cards == null)
                {
                    demoCards [i].cards = new List<DemoCardsInfo>();
                }

                demoCards [i].cards.Add(thirdfreeSpin(j , i));
            }
        }
    }

    public void setUpForthFreeCards ()
    {
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));

        for (int i = 0 ; i < 4 ; i++)
        {
            demoCards.Add(new DemoCards());
            for (int j = 0 ; j < 5 ; j++)
            {
                // Ensure nested objects are initialized
                if (demoCards [i].cards == null)
                {
                    demoCards [i].cards = new List<DemoCardsInfo>();
                }

                demoCards [i].cards.Add(forthfreeSpin(j , i));
            }
        }
    }

    public void setUpFifthFreeCards ()
    {
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));

        for (int i = 0 ; i < 4 ; i++)
        {
            demoCards.Add(new DemoCards());
            for (int j = 0 ; j < 5 ; j++)
            {
                // Ensure nested objects are initialized
                if (demoCards [i].cards == null)
                {
                    demoCards [i].cards = new List<DemoCardsInfo>();
                }

                demoCards [i].cards.Add(fifthfreeSpin(j , i));
            }
        }
    }


    public void setUpSixthFreeCards ()
    {
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));

        for (int i = 0 ; i < 4 ; i++)
        {
            demoCards.Add(new DemoCards());
            for (int j = 0 ; j < 5 ; j++)
            {
                // Ensure nested objects are initialized
                if (demoCards [i].cards == null)
                {
                    demoCards [i].cards = new List<DemoCardsInfo>();
                }

                demoCards [i].cards.Add(sixthfreeSpin(j , i));
            }
        }
    }


    public void setUpSeventhFreeCards ()
    {
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));

        for (int i = 0 ; i < 4 ; i++)
        {
            demoCards.Add(new DemoCards());
            for (int j = 0 ; j < 5 ; j++)
            {
                // Ensure nested objects are initialized
                if (demoCards [i].cards == null)
                {
                    demoCards [i].cards = new List<DemoCardsInfo>();
                }

                demoCards [i].cards.Add(seventhfreeSpin(j , i));
            }
        }
    }

    public void setUpEighthFreeCards ()
    {
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));

        for (int i = 0 ; i < 4 ; i++)
        {
            demoCards.Add(new DemoCards());
            for (int j = 0 ; j < 5 ; j++)
            {
                // Ensure nested objects are initialized
                if (demoCards [i].cards == null)
                {
                    demoCards [i].cards = new List<DemoCardsInfo>();
                }

                demoCards [i].cards.Add(eigthfreeSpin(j , i));
            }
        }
    }

    public void setUpNinethFreeCards ()
    {
        demoCards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));

        for (int i = 0 ; i < 4 ; i++)
        {
            demoCards.Add(new DemoCards());
            for (int j = 0 ; j < 5 ; j++)
            {
                // Ensure nested objects are initialized
                if (demoCards [i].cards == null)
                {
                    demoCards [i].cards = new List<DemoCardsInfo>();
                }

                demoCards [i].cards.Add(ninethfreeSpin(j , i));
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
            case 2:
                return cardsDatas = Spin_3(col , row);
            default:
                return cardsDatas = null;
        }
    }

    public DemoCardsInfo whichFreeSpin ( int which , int col , int row )
    {
        var cardsDatas = new DemoCardsInfo();
        Debug.Log(which);
        switch (which)
        {
            case 3:
                return cardsDatas = freeSpin_3(col , row);
            case 4:
                return cardsDatas = freeSpin_4(col , row);
            case 5:
                return cardsDatas = freeSpin_5(col , row);
            case 6:
                return cardsDatas = freeSpin_6(col , row);
            case 12:
                return cardsDatas = freeSpin_12(col , row);
            case 13:
                return cardsDatas = freeSpin_13(col , row);
            case 16:
                return cardsDatas = freeSpin_16(col , row);
            case 17:
                return cardsDatas = freeSpin_17(col , row);
            default:
                return cardsDatas = null;
        }
    }

    public DemoCardsInfo scatterspinSpin ( int col , int row )
    {
        var cardsDatas = new DemoCardsInfo();
       return cardsDatas = Spin_4(col , row);
    }

    public DemoCardsInfo firstfreeSpin ( int col , int row )
    {
        var cardsDatas = new DemoCardsInfo();
        return cardsDatas = freeSpin_1(col , row);
    }

    public DemoCardsInfo secondfreeSpin ( int col , int row )
    {
        var cardsDatas = new DemoCardsInfo();
        return cardsDatas = freeSpin_2(col , row);
    }

    public DemoCardsInfo thirdfreeSpin ( int col , int row )
    {
        var cardsDatas = new DemoCardsInfo();
        return cardsDatas = freeSpin_7(col , row);
    }

    public DemoCardsInfo forthfreeSpin ( int col , int row )
    {
        var cardsDatas = new DemoCardsInfo();
        return cardsDatas = freeSpin_8(col , row);
    }

    public DemoCardsInfo fifthfreeSpin ( int col , int row )
    {
        var cardsDatas = new DemoCardsInfo();
        return cardsDatas = freeSpin_9(col , row);
    }

    public DemoCardsInfo sixthfreeSpin ( int col , int row )
    {
        var cardsDatas = new DemoCardsInfo();
        return cardsDatas = freeSpin_10(col , row);
    }

    public DemoCardsInfo seventhfreeSpin ( int col , int row )
    {
        var cardsDatas = new DemoCardsInfo();
        return cardsDatas = freeSpin_11(col , row);
    }

    public DemoCardsInfo eigthfreeSpin ( int col , int row )
    {
        var cardsDatas = new DemoCardsInfo();
        return cardsDatas = freeSpin_14(col , row);
    }

    public DemoCardsInfo ninethfreeSpin ( int col , int row )
    {
        var cardsDatas = new DemoCardsInfo();
        return cardsDatas = freeSpin_15(col , row);
    }

    void AddCardType ( CardType cardType ,bool isGolden = false)
    {
        DemoCardsInfo cardsInfo = new DemoCardsInfo
        {
            name = cardType.ToString(),
            isGolden = isGolden,
        };
    }

    public DemoCardsInfo freeSpin_17 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "LITTLE_JOKER" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "SPADE" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "LITTLE_JOKER", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "SCATTER" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute =  new Substitute{ subsitute_ ="DIAMOND" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden =false},
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_16 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "LITTLE_JOKER" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "SPADE" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "LITTLE_JOKER", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "SCATTER" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute =  new Substitute{ subsitute_ ="DIAMOND" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden =false},
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_15 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "LITTLE_JOKER" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "CLUB" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute =  new Substitute{ subsitute_ ="CLUB" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden =false},
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "QUEEN" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_14 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true},
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_13 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "SPADE" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "KING" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "ACE" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute =  new Substitute{ subsitute_ ="HEART" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "ACE" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_12 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "HEART" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "CLUB" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "LITTLE_JOKER", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "WILD" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_11 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "LITTLE_JOKER" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "KING" , isGolden = true } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_10 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_9 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden =false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_8 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true},
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false } , isGolden = true},
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_7 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "HEART" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "SCATTER", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false } , isGolden = true},
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "HEART" , isGolden = true } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_6 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true},
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "WILD" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "QUEEN" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "LITTLE_JOKER", _Subsitute =  new Substitute{ subsitute_ ="DIAMOND" , isGolden = true } , isGolden = false},
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "QUEEN" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "ACE" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_5 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "HEART" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true},
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute =  new Substitute{ subsitute_ ="LITTLE_JOKER" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "HEART" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_4 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "LITTLE_JOKER", _Subsitute = new Substitute{ subsitute_ = "SPADE" , isGolden = true } , isGolden = false},
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute =  new Substitute{ subsitute_ ="QUEEN" , isGolden = true } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "HEART" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "LITTLE_JOKER", _Subsitute = new Substitute{ subsitute_ = "ACE" , isGolden = true } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "QUEEN" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_3 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "SPADE" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "LITTLE_JOKER" , isGolden = false } , isGolden = true},
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "WILD" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "CLUB" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "HEART" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "SPADE" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute =  new Substitute{ subsitute_ ="CLUB" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "CLUB" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "LITTLE_JOKER" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "CLUB" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "LITTLE_JOKER", _Subsitute = new Substitute{ subsitute_ = "SPADE" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "LITTLE_JOKER", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_2 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "CLUB" , isGolden = true } , isGolden = false},
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "HEART" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "ACE" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "SPADE" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "CLUB" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "LITTLE_JOKER" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "LITTLE_JOKER" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo freeSpin_1 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo Spin_4 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false},
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = true },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SCATTER", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "SCATTER", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "SCATTER", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false } , isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo Spin_3 ( int col , int row )
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ ="", isGolden = false, } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false} , isGolden = true },
                new DemoCardsInfo { name = "WILD", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "SPADE",_Subsitute = new Substitute{ subsitute_ = "" , isGolden = false} , isGolden = true},
                new DemoCardsInfo { name = "DIAMOND", _Subsitute =  new Substitute{ subsitute_ = "HEART" , isGolden = false},isGolden  = false }
            },
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute= new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = ""  ,isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute =  new Substitute{ subsitute_ = "" , isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute =  new Substitute{ subsitute_ = ""  , isGolden =false}, isGolden = false },
                new DemoCardsInfo { name = "SPADE", _Subsitute =  new Substitute{ subsitute_ = ""  ,isGolden =false }, isGolden = false },
            },
            {
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "QUEEN" , isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute =  new Substitute{ subsitute_ ="KING" , isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "KING" , isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "SPADE" , isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ =  "" , isGolden = false} , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false}, isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false}, isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "SCATTER" , isGolden = false}, isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false}, isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false}, isGolden = false }
            }
         };

        // Validate input to prevent index out of bounds
        if (row < 0 || row >= cardData.GetLength(0) || col < 0 || col >= cardData.GetLength(1))
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        return cardData [row , col];
    }

    public DemoCardsInfo Spin_2 (int col , int row ) 
    {
        var cardData = new DemoCardsInfo [,]
         {
            {
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ ="QUEEN", isGolden = false, } , isGolden = false },
                new DemoCardsInfo { name = "BIG_JOKER", _Subsitute =  new Substitute{ subsitute_ ="ACE" , isGolden = false} , isGolden = true },
                new DemoCardsInfo { name = "WILD", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "QUEEN",_Subsitute = new Substitute{ subsitute_ = "SPADE" , isGolden = true} , isGolden = false },
                new DemoCardsInfo { name = "BIG_JOKER", _Subsitute =  new Substitute{ subsitute_ = "DIAMOND" , isGolden = false},isGolden  = false }
            },
            {
                new DemoCardsInfo { name = "QUEEN", _Subsitute= new Substitute{ subsitute_ = "DIAMOND" , isGolden = false } , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = ""  ,isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "BIG_JOKER", _Subsitute =  new Substitute{ subsitute_ = "HEART" , isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute =  new Substitute{ subsitute_ = ""  , isGolden =false}, isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute =  new Substitute{ subsitute_ = "SPADE"  ,isGolden =false }, isGolden = false },
            },
            {
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute =  new Substitute{ subsitute_ ="" , isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "DIAMOND" , isGolden = false} , isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute{ subsitute_ =  "CLUB" , isGolden = false} , isGolden = false }
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute{ subsitute_ = "ACE" , isGolden = false}, isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false}, isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute{ subsitute_ = "" , isGolden = false}, isGolden = false },
                new DemoCardsInfo { name = "LITTLE_JOKER", _Subsitute = new Substitute{ subsitute_ = "CLUB" , isGolden = false}, isGolden = true },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute{ subsitute_ = "CLUB" , isGolden = false}, isGolden = false }
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
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "KING" , isGolden = false}, isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute{ subsitute_ = "BIG_JOKER" , isGolden = false}, isGolden = true },
                new DemoCardsInfo { name = "WILD", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false }
            },
            {
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "ACE",_Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "CLUB", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "ACE", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false }
            },
            {
                new DemoCardsInfo { name = "KING", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "DIAMOND", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute { subsitute_ = "CLUB", isGolden = false }, isGolden = false }
            },
            {
                new DemoCardsInfo { name = "SPADE", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "ACE",_Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute { subsitute_ = "DIAMOND", isGolden = false }, isGolden = false },
                new DemoCardsInfo { name = "HEART", _Subsitute = new Substitute { subsitute_ = "LITTLE_JOKER", isGolden = false }, isGolden = true },
                new DemoCardsInfo { name = "QUEEN", _Subsitute = new Substitute { subsitute_ = "", isGolden = false }, isGolden = false }
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