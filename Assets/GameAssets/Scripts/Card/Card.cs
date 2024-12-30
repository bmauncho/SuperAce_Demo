using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CardType { ACE, KING, QUEEN, WILD, SPADE, CLUB, HEART, DIAMOND, SCATTER,LITTLE_JOKER,BIG_JOKER }

public class Card : MonoBehaviour
{
    public CardType ActiveCardType;

    public GameObject CardModel;

    [Header("cardData")]
    public SpriteRenderer CardBg;
    public SpriteRenderer theCard;
    public SpriteRenderer Outline;
    public GameObject ScatterSpin;
    public GameObject ScatterWords;
    public GameObject Gloweffect;
    public GameObject Back;

    public void enableModel ()
    {
        CardModel.SetActive(true);
    }

    public void disableModel ()
    {
        CardModel.SetActive(false);
    }

    public void showNormalCard (Sprite _cardBg,Sprite _theCard)
    {
        enableModel();
        CardBg.gameObject.SetActive(true);
        theCard.gameObject.SetActive(true);
        Back.SetActive(true);
        Outline.gameObject.SetActive(false);
        Gloweffect.SetActive(false);
        ScatterWords.SetActive(false);
        ScatterSpin.gameObject.SetActive(false);
        CardBg.sprite = _cardBg;
        theCard.sprite = _theCard;
        ScatterSpin.GetComponentInChildren<ScatterMotions>().StopMovementAndRotation();
    }

    public void showGoldenCard ( Sprite _cardBg , Sprite _theCard ,Sprite _outline)
    {
        enableModel();
        CardBg.gameObject.SetActive(true);
        theCard.gameObject.SetActive(true);
        Outline.gameObject.SetActive(true);
        Gloweffect.SetActive(true);
        Back.SetActive(true);
        ScatterWords.SetActive(false);
        ScatterSpin.gameObject.SetActive(false);
        CardBg.sprite = _cardBg;
        theCard.sprite = _theCard;
        Outline.sprite = _outline;
        ScatterSpin.GetComponentInChildren<ScatterMotions>().StopMovementAndRotation();
    }

    public void showScatterCard ()
    {
        disableModel();
        CardBg.gameObject.SetActive(false);
        theCard.gameObject.SetActive(false);
        Outline.gameObject.SetActive(false);
        Gloweffect.SetActive(false);
        Back.SetActive(false);
        ScatterWords.SetActive(true);
        ScatterSpin.gameObject.SetActive(true);
        int rand = Random.Range(0 , 2);
        if(rand <= 1)
        {
            ScatterSpin.GetComponentInChildren<ScatterMotions>().Bounce();
        }
        else
        {
            ScatterSpin.GetComponentInChildren<ScatterMotions>().Rotate();
        }
    }

    public void showSmall_Jocker ( Sprite _cardBg , Sprite _theCard , Sprite _outline )
    {
        enableModel();
        CardBg.gameObject.SetActive(true);
        theCard.gameObject.SetActive(true);
        Outline.gameObject.SetActive(true);
        Back.SetActive(true);
        Gloweffect.SetActive(true);
        ScatterWords.SetActive(false);
        ScatterSpin.gameObject.SetActive(false);
        CardBg.sprite = _cardBg;
        theCard.sprite = _theCard;
        Outline.sprite = _outline;
        ScatterSpin.GetComponentInChildren<ScatterMotions>().StopMovementAndRotation();
    }

    public void showBig_Jocker ( Sprite _cardBg , Sprite _theCard , Sprite _outline )
    {
        enableModel();
        CardBg.gameObject.SetActive(true);
        theCard.gameObject.SetActive(true);
        Outline.gameObject.SetActive(true);
        Back.SetActive(true);
        Gloweffect.SetActive(true);
        ScatterWords.SetActive(false);
        ScatterSpin.gameObject.SetActive(false);

        CardBg.sprite = _cardBg;
        theCard.sprite = _theCard;
        Outline.sprite = _outline;
        ScatterSpin.GetComponentInChildren<ScatterMotions>().StopMovementAndRotation();
    }

    public void resetCard ()
    {
        enableModel();
        CardBg.gameObject.SetActive(true);
        theCard.gameObject.SetActive(true);
        Back.SetActive(true);
        Outline.gameObject.SetActive(false);
        Gloweffect.SetActive(false);
        ScatterWords.SetActive(false);
        ScatterSpin.gameObject.SetActive(false);
        ScatterSpin.GetComponentInChildren<ScatterMotions>().StopMovementAndRotation();
    }
}