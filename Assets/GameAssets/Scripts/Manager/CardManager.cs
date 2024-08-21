using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Sprite backSprite;
    public Sprite Normalbackground;
    public Sprite Goldbackground;
    public Sprite DefaultOutline;
    public Sprite ScatterCard;
    public Sprite [] cardSprites;

    private void Start ()
    {

    }

    public Sprite RamomizeCards ()
    {
        int RandomSprite = 0;
        for (int i = 0 ; i < cardSprites.Length ; i++)
        {
            RandomSprite = Random.Range(0 , cardSprites.Length);
        }
        
        return cardSprites [RandomSprite];
    }

    public void DealNormalCards(Transform card )
    {
        card.GetComponent<Card>().SetCardBack(Normalbackground);

        card.GetComponent<Card>().SetCard(RamomizeCards());
        card.GetComponent<Card>().SetCardType();
    }

    public void DealGoldenCards(Transform card )
    {
        card.GetComponent<Card>().SetCardBackGolden(Goldbackground);
        card.GetComponent<Card>().SetCardOutLine(DefaultOutline);
        card.GetComponent<Card>().SetCard(RamomizeCards());
        card.GetComponent<Card>().SetCardType();
    }

    public void DealScatterCards(Transform card )
    {
        card.GetComponent<Card>().SetScatterCard(ScatterCard);
        card.GetComponent<Card>().SetCardType();
    }

    public void RandomizeDealing_golden ( Transform card )
    {
        float normalCardProbability = 0.8f;

        float randomValue = Random.value;

        if (randomValue < normalCardProbability)
        {
            // Deal a normal card
            DealNormalCards(card);
        }
        else
        {
            // Deal a golden card
            DealGoldenCards(card);
        }
    }
    public void RandomizeDealing_Scatter ( Transform card )
    {
        float normalCardProbability = 0.8f;
        float randomValue = Random.value;
        if (randomValue < normalCardProbability)
        {
            // Deal a normal card
            DealNormalCards(card);
        }
        else
        {
            // Deal a ScatterCard
            DealScatterCards(card);
        }
    }

    public string CheckCardSpriteAndSetCardType (Transform Card)
    {
        string cardName = "";
        cardName = Card.GetComponent<Card>().card.sprite.name;
        return cardName;
    }
}