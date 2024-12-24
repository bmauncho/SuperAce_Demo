using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DemoCardsInfo
{
    public string name;
    public bool isGolden;
}
public class DemoSequence : MonoBehaviour
{
    public List<DemoCardsInfo> cards = new List<DemoCardsInfo>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RandomizeCards ()
    {
        cards.Clear();
        List<CardType> cardTypes = new List<CardType>((CardType [])Enum.GetValues(typeof(CardType)));

    }

    void AddCardType ( CardType cardType ,bool isGolden)
    {
        DemoCardsInfo cardsInfo = new DemoCardsInfo
        {
            name = cardType.ToString(),
            isGolden = isGolden

        };
    }

    public void Spin_2 ( Transform whichCard , int col , int row )
    {
       
    }

    public void Spin_1 ( Transform whichCard , int col , int row )
    {
       
    }
}
