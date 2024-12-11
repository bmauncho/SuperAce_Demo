using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommentaryManager : MonoBehaviour
{
    SoundManager soundManager;
    private void Start ()
    {
        soundManager = CommandCentre.Instance.SoundManager_;
    }

    public void PlayCommentary ( List<GameObject> winningCards )
    {

    }

    private IEnumerator PlayCommentarySequentially ( List<CardType> cardTypes )
    {
        foreach (CardType cardType in cardTypes)
        {
            switch (cardType)
            {
                case CardType.Ace:
                    // Play commentary for Ace
                    //Debug.Log("You won with Aces!");
                    StartCoroutine(PlayComboCommentary());
                    yield return new WaitForSeconds(.5f);
                    soundManager.PlaySound(cardType.ToString() , false , soundManager.maxSound);
                    break;

                case CardType.King:
                    // Play commentary for King
                    //Debug.Log("You won with Spades!");
                    StartCoroutine(PlayComboCommentary());
                    yield return new WaitForSeconds(.5f);
                    soundManager.PlaySound(cardType.ToString() , false , soundManager.maxSound);
                    break;

                case CardType.Queen:
                    // Play commentary for Queen
                    //Debug.Log("You won with Queens!");
                    StartCoroutine(PlayComboCommentary());
                    yield return new WaitForSeconds(.5f);
                    soundManager.PlaySound(cardType.ToString() , false , soundManager.maxSound);
                    break;

                case CardType.Jack:
                    // Play commentary for jack
                    //Debug.Log("You won with Jacks!");
                    StartCoroutine(PlayComboCommentary());
                    yield return new WaitForSeconds(.5f);
                    soundManager.PlaySound(cardType.ToString() , false , soundManager.maxSound);
                    break;

                case CardType.Spades:
                    // Play commentary for Spades
                    //Debug.Log("You won with Spades!");
                    StartCoroutine(PlayComboCommentary());
                    yield return new WaitForSeconds(.5f);
                    soundManager.PlaySound(cardType.ToString() , false , soundManager.maxSound);
                    break;

                case CardType.Hearts:
                    // Play commentary for Hearts
                    //Debug.Log("You won with Hearts!");
                    StartCoroutine(PlayComboCommentary());
                    yield return new WaitForSeconds(.5f);
                    soundManager.PlaySound(cardType.ToString() , false , soundManager.maxSound);
                    break;

                case CardType.Diamonds:
                    // Play commentary for Diamonds
                    //Debug.Log("You won with Diamonds!");
                    StartCoroutine(PlayComboCommentary());
                    yield return new WaitForSeconds(.5f);
                    soundManager.PlaySound(cardType.ToString() , false , soundManager.maxSound);
                    break;

                case CardType.Clubs:
                    // Play commentary for Clubs
                    //Debug.Log("You won with Clubs!");
                    StartCoroutine(PlayComboCommentary());
                    yield return new WaitForSeconds(.5f);
                    soundManager.PlaySound(cardType.ToString() , false , soundManager.maxSound);
                    break;
                default:
                    // Handle any other card types or unexpected cases
                    //Debug.Log("You won with an Scatter card type!");
                    yield return new WaitForSeconds(1.0f);
                    break;
            }
        }
    }


    public IEnumerator PlayComboCommentary ()
    {
        int Combo = CommandCentre.Instance.ComboManager_.GetCombo();

        if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            switch (Combo)
            {
                case 1:
                    //Debug.Log("Free Game Combo 1!");
                    break;

                case 2:
                    //Debug.Log("Free Game Combo 2!");
                    soundManager.PlaySound("Lucky" , false , soundManager.maxSound);
                    yield return new WaitForSeconds(0.25f);
                    soundManager.PlaySound("Double" , false , soundManager.maxSound);
                    break;

                case 3:
                   // Debug.Log("Free Game Combo 3!");
                    yield return new WaitForSeconds(0.1f);
                    soundManager.PlaySound("Triple" , false , soundManager.maxSound);
                    break;

                case 5:
                    //Debug.Log("Free Game Combo 5!");
                    yield return new WaitForSeconds(0.1f);
                    soundManager.PlaySound("FiveTimes" , false , soundManager.maxSound);
                    break;
            }
        }
        else
        {
            switch (Combo)
            {
                case 2:
                   // Debug.Log("Regular Combo 2!");
                    soundManager.PlaySound("Lucky" , false , soundManager.maxSound);
                    yield return new WaitForSeconds(0.1f);
                    soundManager.PlaySound("Double" , false , soundManager.maxSound);
                    break;

                case 4:
                    //Debug.Log("Regular Combo 4!");
                    yield return new WaitForSeconds(0.1f);
                    soundManager.PlaySound("FourTimes" , false , soundManager.maxSound);
                    break;

                case 6:
                   // Debug.Log("Regular Combo 6!");
                    yield return new WaitForSeconds(0.1f);
                    soundManager.PlaySound("SixTimes" , false , soundManager.maxSound);
                    break;

                case 10:
                    //Debug.Log("Regular Combo 10!");
                    yield return new WaitForSeconds(0.1f);
                    soundManager.PlaySound("TenTimes" , false , soundManager.maxSound);
                    break;
            }
        }
    }

}
