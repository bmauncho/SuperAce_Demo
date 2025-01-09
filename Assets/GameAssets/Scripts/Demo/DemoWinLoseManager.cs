using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class WinCardData
{
    public string name;
    public string Substitute;
    public bool isGolden;
    public bool canFlip;
    public position position;
}

[System.Serializable]
public class position
{
    public int row;
    public int col;
}

public class DemoWinLoseManager : MonoBehaviour
{
    PoolManager poolManager;
    CardFxManager cardFxManager;
    public DemoSequence demoSequence;
    public DemoGridManager demoGridManager;
    public List<WinCardData> winCards = new List<WinCardData>();

    public int totalobjectstojump = 2;
    public int objectsJumped = 0;

    public int jumpIndex = 0;
    private bool isCanRefillCalled = false; // Flag to ensure method is called only once
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poolManager = CommandCentre.Instance.PoolManager_;
        cardFxManager = CommandCentre.Instance.CardFxManager_;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanRefill ()
    {
        if (isCanRefillCalled)
        {
            Debug.LogWarning("CanRefill has already been called. Skipping execution.");
            return false; // Prevent re-execution
        }

        isCanRefillCalled = true; // Mark the method as called

        List<DemoCards> demoCards = new List<DemoCards>(demoSequence.demoCards);
        if (demoCards == null || demoCards.Count <= 0)
        {
            isCanRefillCalled = false; // Reset the flag before returning
            return false;
        }

        bool hasSubstitute = false; // Flag to track if at least one substitute is found
        HashSet<string> addedCardKeys = new HashSet<string>(); // To track added cards and avoid duplicates

        for (int i = 0 ; i < demoCards.Count ; i++)
        {
            var cards = demoCards [i];
            if (cards == null || cards.cards == null)
            {
                continue; // Skip null entries
            }

            for (int j = 0 ; j < cards.cards.Count ; j++)
            {
                var card = cards.cards [j];
                if (card == null || string.IsNullOrEmpty(card.name))
                {
                    continue; // Skip null or empty cards
                }

                // Create a unique key for the card (row, col, and name)
                string cardKey = $"{i}-{j}-{card.name}";
                if (addedCardKeys.Contains(cardKey))
                {
                    continue; // Skip if card is already added
                }

                if (CommandCentre.Instance.DemoManager_.isScatterSpin && card.name == "SCATTER")
                {
                    winCards.Add(new WinCardData
                    {
                        name = card.name ,
                        Substitute = card._Subsitute?.subsitute_ ,
                        position = new position
                        {
                            row = i ,
                            col = j
                        }
                    });

                    addedCardKeys.Add(cardKey); // Mark SCATTER as added
                    hasSubstitute = true; // SCATTER counts as a substitute
                    continue; // Skip further checks for this card
                }

                // Check for substitute or optional jokers
                if (!string.IsNullOrEmpty(card._Subsitute?.subsitute_) ||
                    card.name == "BIG_JOKER" ||
                    card.name == "LITTLE_JOKER" ||
                    card.isGolden)
                {
                    winCards.Add(new WinCardData
                    {
                        name = card.name ,
                        Substitute = card._Subsitute?.subsitute_ ,
                        position = new position
                        {
                            row = i ,
                            col = j
                        }
                    });
                    addedCardKeys.Add(cardKey); // Mark this card as added
                    hasSubstitute = true;
                }
                
                
            }
        }

        if (CommandCentre.Instance.DemoManager_.isScatterSpin)
        {
            // Filter winCards to keep only SCATTER cards
            winCards = winCards.Where(card => card.name == "SCATTER").ToList();
        }
        else if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            if(winCards.Count <3)
            {
                winCards.Clear();
                hasSubstitute = false;
            }
        }

        // Optional: Log or handle cases when no substitute or joker is found
        if (!hasSubstitute)
        {
            Debug.LogWarning("No substitutes or jokers found for refill.");
        }

        isCanRefillCalled = false; // Reset the flag after the method is complete
        return hasSubstitute; // Return true if at least one substitute was found
    }



    public void DemoWinSequence ()
    {
        StartCoroutine(showWinSequence());
    }

    public IEnumerator showWinSequence ()
    {
        yield return StartCoroutine(showWinningCards());
        yield return null;
    }

    IEnumerator showWinningCards ()
    {
        CommandCentre.Instance.ComboManager_.IncreaseComboCounter();
        CommandCentre.Instance.CardFxManager_.DeactivateCardFxMask();
        CommandCentre.Instance.CardFxManager_.ActivateCardMask();
        for (int i = 0 ; i < winCards.Count ; i++)
        {
            CommandCentre.Instance.CardFxManager_.ActivateCardFxMask(winCards [i].position.row , winCards [i].position.col);
        }

        yield return new WaitForSeconds(1);

        yield return StartCoroutine(HideNormalCards());
        yield return null;
    }


    IEnumerator HideNormalCards ()
    {
        int hiddenCards = 0;
        // Collect all scatter cards
        List<GameObject> scatterCards = new List<GameObject>();

        for (int i = 0 ; i < winCards.Count ; i++)
        {
            int row = winCards [i].position.row;
            int col = winCards [i].position.col;

            // Ensure row and col indices are within valid range
            if (row < 0 || row >= 4 || col < 0 || col >= 5)
            {
                Debug.LogWarning($"Invalid row or column index: row={row}, col={col}");
                continue;
            }

            GameObject cardPosHolder = demoGridManager.colData [col].cardPositionInRow [row];
            CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
            GameObject card = cardPos.TheOwner;

            if (card == null) continue;

            Card cardComponent = card.GetComponent<Card>();

            // Handle golden cards
            if (cardComponent.golden && !cardComponent.wild && !cardComponent.scatter)
            {
                //Debug.Log(" Handle golden cards-1");
                StartCoroutine(rotateNormalGoldenCards(card , col , row));
            }
            // Handle wild cards
            else if (cardComponent.wild && !cardComponent.golden && !cardComponent.scatter)
            {
                StartCoroutine(RotateWildCards(card , col , row));
            }
            // Handle scatter cards
            else if (cardComponent.scatter && !cardComponent.wild && !cardComponent.golden)
            {
                scatterCards.Add(card);
            }
            // Deactivate and return normal cards to the pool
            else
            {
                card.SetActive(false);
                poolManager.ReturnCard(card);
                cardPos.TheOwner = null;
                hiddenCards++;
            }
        }
        if (scatterCards.Count > 0)
        {
            yield return StartCoroutine(RotateScatterCards(scatterCards));
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            // Reset and refill processes
            if (CommandCentre.Instance.TurboManager_.TurboSpin_)
            {
                demoGridManager.refillTurbo(hiddenCards);
            }
            else
            {
                demoGridManager.refillGrid(hiddenCards);
            }
            winCards.Clear();
            cardFxManager.DeactivateCardFxMask();
            //
            // Show current win
            CommandCentre.Instance.PayOutManager_.ShowCurrentWin();
            yield return null;
        }
    }



    private IEnumerator rotateNormalGoldenCards ( GameObject card , int col = 0 , int row = 0 )
    {
        Debug.Log(" Handle golden cards-2");
        card.transform.DORotate(Vector3.zero , .2f)
            .OnComplete(() =>
            {
                CommandCentre.Instance.CardManager_.setUpDemoCards(card.GetComponent<Card>() , col , row);
            });
        yield return new WaitForSeconds(.5f);

        DemoCardsInfo cardInfo = new DemoCardsInfo
        {
            name = demoSequence.GetDemoCardInfo(col , row)._Subsitute.subsitute_
        };
        // set golden cards to either bigJoker or little jocker
        if (Enum.TryParse(typeof(CardType) , cardInfo.name , out var cardType))
        {
            //Debug.Log($"Successfully parsed card type: {cardType}");

            card.GetComponent<Card>().ActiveCardType = (CardType)cardType;
        }

        CommandCentre.Instance.CardManager_.setcard(card.GetComponent<Card>() , col , row);
        yield return new WaitForSeconds(.5f);
        card.transform.DORotate(new Vector3(0 , 180f , 0) , .2f);
        yield return new WaitForSeconds(.5f);
        if (card.GetComponent<Card>().ActiveCardType == CardType.BIG_JOKER)
        {
            yield return StartCoroutine(jumpBigJockerCards(card));
        }

    }

    private IEnumerator RotateScatterCards ( List<GameObject> scatterCards )
    {
        // Ensure there are scatter cards to process
        if (scatterCards == null || scatterCards.Count == 0)
        {
            yield break;
        }

        // Trigger the rotation for all scatter cards
        foreach (var scatterCard in scatterCards)
        {
            if (scatterCard == null) continue;

            ScatterMotions scatterMotions = scatterCard.GetComponentInChildren<ScatterMotions>();
            if (scatterMotions != null)
            {
                scatterMotions.Rotate();
            }
        }

        // Wait for a duration that matches the scatter card rotation animation
        yield return new WaitForSeconds(1);

        // Activate the free game mechanics
        CommandCentre.Instance.FreeGameManager_.IsFreeGame = true;
        CommandCentre.Instance.FreeGameManager_.ActivateFreeGameIntro();

        // Wait for the intro to complete
        yield return new WaitForSeconds(3);

        CommandCentre.Instance.FreeGameManager_.DeactivateFreeGameIntro();
        winCards.Clear();
        cardFxManager.DeactivateCardFxMask();
        CommandCentre.Instance.DemoManager_.isScatterSpin = false;
        CommandCentre.Instance.DemoManager_.DemoSequence_.setUpFirstFreeCards();
    }



    private IEnumerator RotateWildCards ( GameObject goldenCard , int col = 0 , int row = 0 )
    {
        goldenCard.transform.DORotate(Vector3.zero , .5f , RotateMode.FastBeyond360).OnComplete(() =>
        {
            StartCoroutine(PunchScaleRotatedCards(goldenCard.transform , col , row));
        });

        yield return null;
    }

    public IEnumerator PunchScaleRotatedCards ( Transform target , int col = 0 , int row = 0 )
    {
        yield return new WaitForSeconds(.25f);
        Tween PunchScale = target.DOPunchScale(new Vector3(.1f , .1f , .1f) , .5f , 5 , 1).SetEase(Ease.OutQuad);
        yield return PunchScale.WaitForCompletion();
        //rotate
        target.transform.DORotate(new Vector3(0 , 180f , 0) , .5f , RotateMode.FastBeyond360);

        // if Big joker jump two cards to random positions which is not the current card pos
        if (target.GetComponent<Card>().ActiveCardType == CardType.BIG_JOKER)
        {
            yield return StartCoroutine(jumpBigJockerCards(target.gameObject , col , row));
        }
    }


    private IEnumerator jumpBigJockerCards ( GameObject target , int col = 0 , int row = 0 )
    {
        objectsJumped = 0;
        GameObject newCard1 = poolManager.GetCard();
        GameObject newCard2 = poolManager.GetCard();

        // set the cards as BigJocker 

        Vector3 initialPosition = target.transform.position;
        Quaternion initialRotation = target.transform.rotation;


        int randomColumnIndex1 = Random.Range(0 , 5);
        int randomrowIndex1 = Random.Range(0 , 4);

        int randomColumnIndex2, randomrowIndex2 = 0;
        if (jumpIndex == 0)
        {
            randomColumnIndex1 = 2;
            randomrowIndex1 = 1;
            randomColumnIndex2 = 4;
            randomrowIndex2 = 0;
           
        }
        else
        {
            do
            {
                randomColumnIndex2 = Random.Range(0 , 5);
                randomrowIndex2 = Random.Range(0 , 4);
            }
            while (( randomColumnIndex1 == randomColumnIndex2 && randomrowIndex1 == randomrowIndex2 ) ||
                    ( randomColumnIndex1 == col && randomrowIndex1 == row ) ||
                    ( randomColumnIndex2 == col && randomrowIndex2 == row ));
        }

        

        newCard1.transform.SetPositionAndRotation(initialPosition , initialRotation);
        newCard2.transform.SetPositionAndRotation(initialPosition , initialRotation);

        newCard1.SetActive(true);
        newCard2.SetActive(true);

        newCard1.transform.SetParent(demoGridManager.colData [randomColumnIndex1].cardPositionInRow [randomrowIndex1].transform);
        newCard2.transform.SetParent(demoGridManager.colData [randomColumnIndex2].cardPositionInRow [randomrowIndex2].transform);

        demoGridManager.colData [randomColumnIndex1].cardPositionInRow [randomrowIndex1].GetComponent<CardPos>().TheOwner.SetActive(false);
        demoGridManager.colData [randomColumnIndex2].cardPositionInRow [randomrowIndex2].GetComponent<CardPos>().TheOwner.SetActive(false);
        demoGridManager.colData [randomColumnIndex1].cardPositionInRow [randomrowIndex1].GetComponent<CardPos>().TheOwner = newCard1;
        demoGridManager.colData [randomColumnIndex2].cardPositionInRow [randomrowIndex2].GetComponent<CardPos>().TheOwner = newCard2;
        newCard1.GetComponent<Card>().ActiveCardType = CardType.BIG_JOKER;
        newCard2.GetComponent<Card>().ActiveCardType = CardType.BIG_JOKER;
        CommandCentre.Instance.CardManager_.setcard(newCard1.GetComponent<Card>() , randomColumnIndex1 , randomrowIndex1);
        CommandCentre.Instance.CardManager_.setcard(newCard2.GetComponent<Card>() , randomColumnIndex2 , randomrowIndex2);
        CommandCentre.Instance.PoolManager_.ReturnAllInactiveCardsToPool();
        var jumpSequence = DOTween.Sequence();
        // DOTween jump animations
        jumpSequence.Join(newCard1.transform.DOJump(
            demoGridManager.colData [randomColumnIndex1].cardPositionInRow [randomrowIndex1].transform.position ,
            2.0f , 1 , 1.0f).OnComplete(() =>
            {
                objectsJumped++;
                Debug.Log($"Card 1 jumped: {objectsJumped}/{totalobjectstojump}");
                newCard1.transform.localPosition = Vector3.zero;
                newCard1.transform.rotation = Quaternion.Euler(0 , 180 , 0);
                demoSequence.demoCards [randomrowIndex1].cards [randomColumnIndex1].name = newCard1.GetComponent<Card>().ActiveCardType.ToString ();
            }));

        jumpSequence.Join(newCard2.transform.DOJump(
            demoGridManager.colData [randomColumnIndex2].cardPositionInRow [randomrowIndex2].transform.position ,
            2.0f , 1 , 1.0f).OnComplete(() =>
            {
                objectsJumped++;
                Debug.Log($"Card 2 jumped: {objectsJumped}/{totalobjectstojump}");
                newCard2.transform.localPosition = Vector3.zero;
                newCard2.transform.rotation = Quaternion.Euler(0 , 180 , 0);
                demoSequence.demoCards [randomrowIndex2].cards [randomColumnIndex2].name = newCard2.GetComponent<Card>().ActiveCardType.ToString();
            }));


        // Play the jump sequence and wait for it to complete
        if (jumpSequence.IsActive())
        {
            // Debug.Log("Waiting for jump sequence to complete...");
            yield return jumpSequence.Play().WaitForCompletion();
        }

        //Debug.Log("Jump complete");
        yield return new WaitForSeconds(0.5f);
        //Debug.Log($"Jump Check : {IsObjectsJumpComplete()}");

        if (!IsObjectsJumpComplete())
        {
            while (!IsObjectsJumpComplete())
            {
                objectsJumped++;
            }
        }

        if (IsObjectsJumpComplete())
        {
            jumpIndex++;
            //Debug.Log("wincheck");
        }

        yield return null;
    }

    public bool IsObjectsJumpComplete ()
    {
        return objectsJumped >= totalobjectstojump;
    }

}
