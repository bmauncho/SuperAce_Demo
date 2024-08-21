using System;
using UnityEngine;

public enum CardType { Ace, King, Queen, Jack, Spades, Clubs, Hearts, Diamonds, Scatter }

public class Card : MonoBehaviour
{
    public GameObject TheCard;
    public SpriteRenderer Back;
    public SpriteRenderer cardBg;
    public SpriteRenderer card;
    public SpriteRenderer Outline;
    public CardType cardType;
    public bool IsGoldenCard = false;
    public bool IsScatterCard = false;

    private void Start ()
    {
        cardBg = GetComponentInChildren<SpriteRenderer>();
    }
    public void EnableCardOBJ ()
    {
        if (!TheCard.activeSelf)
        {
            TheCard.SetActive (true);
        }
    }

    public void DisableCardOBJ ()
    {
        if (TheCard.activeSelf)
        {
            TheCard?.SetActive(false);
        }
    }

    public void EnableBackCard ()
    {
        if (!Back.gameObject.activeSelf)
        {
            Back.gameObject.SetActive(true);
        }
    }

    public void DisableBackCard ()
    {
        if (Back.gameObject.activeSelf)
        {
            Back.gameObject.SetActive(false);
        }
    }

    public void SetCardType ()
    {
        cardType = CompareEnumToString();
        if (cardType == CardType.Scatter) { IsGoldenCard = false; }
    }

    public void SetCardBack ( Sprite backSprite = null )
    {
        EnableBackCard();
        EnableCardOBJ();
        cardBg.sprite = backSprite;
    }

    public void SetCard ( Sprite cardSprite = null )
    {
        EnableBackCard();
        EnableCardOBJ();
        card.sprite = cardSprite;
    }

    public void SetCardBackGolden ( Sprite backGoldenSprite = null )
    {
        EnableBackCard();
        EnableCardOBJ();
        SetCardBack(backGoldenSprite);
        IsGoldenCard = true;
        IsScatterCard = false;
    }

    public void SetCardOutLine ( Sprite outLineSprite = null )
    {
        EnableBackCard();
        EnableCardOBJ();
        Outline.sprite = outLineSprite;
    }

    public void SetScatterCard ( Sprite scatterCardSprite = null )
    {
        IsScatterCard = true;
        IsGoldenCard = false;
        SetCardBack();
        SetCard(scatterCardSprite);
        SetCardOutLine();
        SetCardBackGolden();
        DisableBackCard();
        DisableCardOBJ();
    }

    private void OnDisable ()
    {
        IsScatterCard = false;
        IsGoldenCard = false;
    }

    CardType CompareEnumToString ()
    {
        string stringValue = CommandCentre.Instance.CardManager_.CheckCardSpriteAndSetCardType(this.transform);
        if (Enum.TryParse(stringValue , out cardType))
        {
            switch (cardType)
            {
                case CardType.Ace:
                    return cardType;
                case CardType.King:
                    return cardType;
                case CardType.Queen:
                    return cardType;
                case CardType.Jack:
                    return cardType;
                case CardType.Spades:
                    return cardType;
                case CardType.Clubs:
                    return cardType;
                case CardType.Hearts:
                    return cardType;
                case CardType.Diamonds:
                    return cardType;
                case CardType.Scatter:
                    return cardType;
                default:
                    return cardType;
            }
        }
        else
        {
            Debug.Log("The string could not be converted to an enum.");
            return default(CardType);
        }
    }
}
