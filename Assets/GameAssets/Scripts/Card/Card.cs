using System;
using UnityEngine;

public enum CardType { Ace, King, Queen, Jack, Spades, Clubs, Hearts, Diamonds, Scatter,Small_Jocker,Big_Jocker }

public class Card : MonoBehaviour
{
    [Header("References")]
    public GameObject TheCard;
    public SpriteRenderer Back;
    public SpriteRenderer cardBg;
    public SpriteRenderer card;
    public SpriteRenderer Outline;
    public CardType cardType;
    [Header("Variables")]
    public bool IsGoldenCard = false;
    public bool IsScatterCard = false;
    public bool IsSmallJocker = false;
    public bool IsBigJocker = false;
    public int CardSortPos = 5;
    public Vector3 CardScale;

    public GameObject ScatterWords;
    public Animator ScatterCardAnim;

    public ParticleSystem ScatterRotate;

    private void Start ()
    {
        cardBg = GetComponentInChildren<SpriteRenderer>();
        CardScale = this.transform.localScale;
    }

    private void Update ()
    {
        IsScatterCard = ( cardType == CardType.Scatter );

        if (cardType == CardType.Big_Jocker || cardType == CardType.Small_Jocker)
        {
            IsGoldenCard = true;
        }
        IsBigJocker = ( cardType == CardType.Big_Jocker );
        IsSmallJocker = ( cardType == CardType.Small_Jocker );


        if (IsGoldenCard)
        {
            if (IsBigJocker)
            {
                Outline.sprite = CommandCentre.Instance.CardManager_.BigJockerOutline;
            }
            else if (IsSmallJocker)
            {
                Outline.sprite = CommandCentre.Instance.CardManager_.SmallJockerOutline;
            }
            else
            {
                Outline.sprite = CommandCentre.Instance.CardManager_.DefaultOutline;
            }
            cardBg.sprite = CommandCentre.Instance.CardManager_.Goldbackground;
        }
        else
        {
            cardBg.sprite = CommandCentre.Instance.CardManager_.Normalbackground;
            if (IsScatterCard)
            {
                cardBg.sprite = null;
                ScatterWords.SetActive(true);
            }
            else
            {
                ScatterWords.SetActive(false);
                ScatterCardAnim.enabled = false;
            }
        }

    }

    public void SetCardSortPos ( int pos )
    {
        CardSortPos = pos;
        Back.sortingOrder = pos;
    }
    public void EnableCardOBJ ()
    {
        if (!TheCard.activeSelf)
        {
            TheCard.SetActive(true);
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
        if (cardType == CardType.Scatter)
        {
            IsGoldenCard = false;
            //this is a safety net
            IsScatterCard = true;
            ScatterCardAnim.enabled = false;
        }
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
        SetCardBack();
        SetCard(scatterCardSprite);
        SetCardOutLine();
        SetCardBackGolden();
        DisableBackCard();
        DisableCardOBJ();
        IsGoldenCard = false;
        IsScatterCard = true;
    }

    public void SetSmallJocker ( Sprite smallJoker = null )
    {
        IsScatterCard = false;
        IsGoldenCard = true;
        IsSmallJocker = true;
        IsBigJocker = false;
        SetCard(smallJoker);
    }
    public void SetBigJocker ( Sprite bigJoker = null )
    {
        IsScatterCard = false;
        IsGoldenCard = true;
        IsSmallJocker = false;
        IsBigJocker = true;
        SetCard(bigJoker);
    }

    private void OnDisable ()
    {
        IsScatterCard = false;
        IsGoldenCard = false;
    }

    CardType CompareEnumToString ()
    {
        string stringValue = CommandCentre.Instance.CardManager_.CheckCardSpriteAndSetCardType(this.transform);
        if (Enum.TryParse(stringValue , true , out cardType))
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
                case CardType.Big_Jocker:
                    return cardType;
                case CardType.Small_Jocker:
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

    public void EnableScatterRotate ()
    {
        card.sprite = null;
        ScatterRotate.gameObject.SetActive(true);
        ScatterCardAnim.enabled = false;
    }

    public void DisableScatterRotate ()
    {
        card.sprite = CommandCentre.Instance.CardManager_.ScatterCard;
        ScatterCardAnim.enabled = true;
        ScatterRotate.gameObject.SetActive(false);
    }
}