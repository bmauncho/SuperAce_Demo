using System;
using UnityEngine;

public enum CardType { Ace, King, Queen, Jack, Spades, Clubs, Hearts, Diamonds, Scatter,Small_Jocker,Big_Jocker }

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



    private void Update ()
    {
        
    }

    public void enableModel ()
    {
        CardModel.SetActive(true);
    }

    public void disableModel ()
    {
        CardModel.SetActive(false);
    }

    public void showNormalCard ()
    {
        enableModel();
    }

    public void showGoldenCard ()
    {
        enableModel();
    }

    public void showScatterCard ()
    {
        disableModel();
    }

    public void showSmall_Jocker ()
    {
        enableModel();
    }

    public void showBig_Jocker ()
    {
        enableModel();
    }
}