using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[System.Serializable]
public class winData
{
    public string name;
    public int row;
    public int col;
}

public class WinLoseManager : MonoBehaviour
{
    GridManager gridManager;
    PoolManager poolManager;
    CardFxManager cardFxManager;
    public List<winData> data = new List<winData>();
    public List<winData> scatterdata = new List<winData>();

    public int totalobjectstojump = 2;
    public int objectsJumped = 0;
    private  HashSet<string> addedKeys = new HashSet<string>();

    private void Start ()
    {
        gridManager = CommandCentre.Instance.GridManager_;
        poolManager = CommandCentre.Instance.PoolManager_;
        cardFxManager = CommandCentre.Instance.CardFxManager_;
        data.Clear();
        addedKeys.Clear();
    }

   

    public void GetWinningCard ( CardData _data , int row , int col )
    {
        // Create a unique key for the card
        string key = $"{row}-{col}";

        // Check if the key already exists in the HashSet
        if (!addedKeys.Contains(key))
        {
            // Add the card data if it's not a duplicate
            data.Add(new winData
            {
                name = _data.name ,
                row = row ,
                col = col ,
            });

            // Add the key to the HashSet to prevent future duplicates
            addedKeys.Add(key);
        }
    }

    public void ClearAddedKeys ()
    {
        addedKeys.Clear();
    }

    public void ResetWinDataList ()
    {
        data.Clear();
    }

    [ContextMenu("Test")]
    public void winSequence ()
    {
        StartCoroutine(startWinSequence());
    }

    public IEnumerator startWinSequence ()
    {
        yield return StartCoroutine(showWinningCards());
      
        yield return null;
    }

    public IEnumerator showWinningCards ()
    {
        objectsJumped = 0;
        CommandCentre.Instance.ComboManager_.IncreaseComboCounter();
        CommandCentre.Instance.CardFxManager_.DeactivateCardFxMask();
        CommandCentre.Instance.CardFxManager_.ActivateCardMask();
        for (int i = 0;i<data.Count ; i++)
        {
            CommandCentre.Instance.CardFxManager_.ActivateCardFxMask(data [i].row , data [i].col);
        }
        List<CardType> winningCards = new List<CardType>();
        for (int j = 0 ; j < data.Count ; j++)
        {
            // Assuming `data[j].name` is a string and `CardType` is an enum
            if (Enum.TryParse(data [j].name , out CardType matchingCard))
            {
                // Log the match
                //Debug.Log($"Match found: {data [j].name}");

                // Add the matching CardType to the list
                winningCards.Add(matchingCard);
            }
        }


        CommandCentre.Instance.CommentaryManager_.PlayCommentary(winningCards);
        yield return new WaitForSeconds(.5f);
        yield return StartCoroutine(WinEffect());
        yield return new WaitForSeconds(.5f); 

        yield return StartCoroutine(HideNormalCards());

        yield return null;
    }

    IEnumerator WinEffect ()
    {
        int tweensToComplete = data.Count;
        int tweensCompleted = 0;
        //Debug.Log($"Tweens to complete: {tweensToComplete}");
        var cardFxManager_ = CommandCentre.Instance.CardFxManager_;

        for (int i = 0 ; i < data.Count ; i++)
        {
            int row = data [i].row;
            int col = data [i].col;

            var cardfxMask= cardFxManager_.CardFxMask [row].cardFxPos [col];

            GameObject cardPosHolder = gridManager.rowData [row].cardPositionInRow [col];
            CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
            GameObject card = cardPos.TheOwner;

            if (card)
            {
                //card.GetComponent<Card>().enableWinEffect();
                card.transform.DOPunchScale(new Vector3(0.2f , 0.2f , 0.2f) , 0.5f , 5 , 1)
                    .OnComplete(() =>
                    {
                        tweensCompleted++;
                        //Debug.Log($"Tweens completed: {tweensCompleted} / {tweensToComplete}");
                    });
                cardfxMask.transform.DOPunchScale(new Vector3(0.2f , 0.2f , 0.2f) , 0.5f , 5 , 1);
            }
        }

        // Wait until all tweens are completed
        yield return new WaitUntil(() => tweensCompleted >= tweensToComplete);
       // Debug.Log("All tweens completed. Coroutine finished.");
    }




    IEnumerator HideNormalCards ()
    {
        int hiddenCards = 0;
        List<GameObject> scatterCards = new List<GameObject>();
        for (int i = 0 ; i < data.Count ; i++)
        {
            int row = data [i].row;
            int col = data [i].col;

            // Ensure row and col indices are within valid range
            if (row < 0 || row >= 4 || col < 0 || col >= 5)
            {
                Debug.LogWarning($"Invalid row or column index: row={row}, col={col}");
                continue;
            }

            GameObject cardPosHolder = gridManager.rowData [row].cardPositionInRow [col];
            CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
            GameObject card = cardPos.TheOwner;
            
            if (card == null) continue;

            Card cardComponent = card.GetComponent<Card>();

            // Handle golden cards
            if (cardComponent.golden && !cardComponent.wild && !cardComponent.scatter)
            {
                Debug.Log(" Handle golden cards-1");
                StartCoroutine(rotateNormalGoldenCards(card,col,row));
            }
            // Handle wild cards
            else if (cardComponent.wild && !cardComponent.golden && !cardComponent.scatter)
            {
                StartCoroutine(RotateWildCards(card,col,row));
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

        yield return new WaitForSeconds(1.5f);

        // Reset and refill processes
        cardFxManager.DeactivateCardFxMask();
        ResetWinDataList();
        ClearAddedKeys();

        if (CommandCentre.Instance.TurboManager_.TurboSpin_)
        {
            gridManager.refillTurbo(hiddenCards);
        }
        else
        {
            gridManager.refillGrid(hiddenCards);
        }

        // Show current win
        CommandCentre.Instance.PayOutManager_.ShowCurrentWin();
        yield return null;
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
        data.Clear();
        cardFxManager.DeactivateCardFxMask();
        CommandCentre.Instance.DemoManager_.isScatterSpin = false;
        CommandCentre.Instance.DemoManager_.DemoSequence_.setUpFirstFreeCards();
        CommandCentre.Instance.MainMenuController_.CanSpin = true;
    }



    private IEnumerator rotateNormalGoldenCards ( GameObject card,int col = 0,int row= 0 )
    {
        Debug.Log(" Handle golden cards-2");
        card.transform.DORotate(Vector3.zero , .2f)
            .OnComplete(() =>
            {
                CommandCentre.Instance.CardManager_.setUpCard(card.GetComponent<Card>(),col,row);
            });
        yield return new WaitForSeconds(.5f);
     

        // set golden cards to either bigJoker or little jocker
        int rand = Random.Range(0 , 2);
        if(rand <= 1)
        {
            card.GetComponent<Card>().ActiveCardType = CardType.LITTLE_JOKER;
        }
        else
        {
            card.GetComponent<Card>().ActiveCardType = CardType.BIG_JOKER;
        }
         
        CommandCentre.Instance.CardManager_.setcard(card.GetComponent<Card>(),col,row);
        yield return new WaitForSeconds(.5f);
        card.transform.DORotate(new Vector3(0 , 180f , 0) , .2f);
        yield return new WaitForSeconds(.5f);
        if(card.GetComponent<Card>().ActiveCardType == CardType.BIG_JOKER)
        {
            yield return StartCoroutine(jumpBigJockerCards(card));
        }

    }


    private IEnumerator RotateWildCards (GameObject goldenCard, int col = 0 , int row = 0 )
    {
        goldenCard.transform.DORotate(Vector3.zero , .5f , RotateMode.FastBeyond360).OnComplete(() =>
        {
            StartCoroutine(PunchScaleRotatedCards(goldenCard.transform,col,row));
        });

        yield return null;
    }

    public IEnumerator PunchScaleRotatedCards ( Transform target, int col = 0 , int row = 0 )
    {
        yield return new WaitForSeconds(.25f);
        Tween PunchScale = target.DOPunchScale(new Vector3(.1f , .1f , .1f) , .5f , 5 , 1).SetEase(Ease.OutQuad);
        yield return PunchScale.WaitForCompletion();
        //rotate
        target.transform.DORotate(new Vector3(0,180f,0) , .5f , RotateMode.FastBeyond360);

        // if Big joker jump two cards to random positions which is not the current card pos
        if (target.GetComponent<Card>().ActiveCardType == CardType.BIG_JOKER)
        {
            yield return StartCoroutine(jumpBigJockerCards(target.gameObject,col,row));
        }
    }


    private IEnumerator jumpBigJockerCards (GameObject target , int col = 0 , int row = 0 )
    {
        GameObject newCard1 = poolManager.GetCard();
        GameObject newCard2 = poolManager.GetCard();

        // set the cards as BigJocker 

        Vector3 initialPosition = target.transform.position;
        Quaternion initialRotation = target.transform.rotation;


        int randomColumnIndex1 = Random.Range(0 , 5);
        int randomrowIndex1 = Random.Range(0 , 4);

        int randomColumnIndex2, randomrowIndex2 = 0;

        do
        {
            randomColumnIndex2 = Random.Range(0 , 5);
            randomrowIndex2 = Random.Range(0 , 4);
        }
        while (( randomColumnIndex1 == randomColumnIndex2 && randomrowIndex1 == randomrowIndex2 ) ||
                ( randomColumnIndex1 == col && randomrowIndex1 == row ) ||
                ( randomColumnIndex2 == col && randomrowIndex2 == row ));

        newCard1.transform.SetPositionAndRotation(initialPosition , initialRotation);
        newCard2.transform.SetPositionAndRotation(initialPosition , initialRotation);

        newCard1.SetActive(true);
        newCard2.SetActive(true);

        newCard1.transform.SetParent(gridManager.rowData[randomColumnIndex1].cardPositionInRow[randomrowIndex1].transform);
        newCard2.transform.SetParent(gridManager.rowData [randomColumnIndex2].cardPositionInRow [randomrowIndex2].transform);

        gridManager.rowData [randomColumnIndex1].cardPositionInRow [randomrowIndex1].GetComponent<CardPos>().TheOwner.SetActive(false);
        gridManager.rowData [randomColumnIndex2].cardPositionInRow [randomrowIndex2].GetComponent<CardPos>().TheOwner.SetActive(false);
        gridManager.rowData [randomColumnIndex1].cardPositionInRow [randomrowIndex1].GetComponent<CardPos>().TheOwner = newCard1;
        gridManager.rowData [randomColumnIndex2].cardPositionInRow [randomrowIndex2].GetComponent<CardPos>().TheOwner = newCard2;

        CommandCentre.Instance.PoolManager_.ReturnAllInactiveCardsToPool();
        var jumpSequence = DOTween.Sequence();
        // DOTween jump animations
        jumpSequence.Join(newCard1.transform.DOJump(
            gridManager.rowData [randomColumnIndex1].cardPositionInRow [randomrowIndex1].transform.position ,
            2.0f , 1 , 1.0f).OnComplete(() =>
            {
                objectsJumped++;
                Debug.Log($"Card 1 jumped: {objectsJumped}/{totalobjectstojump}");
                newCard1.transform.localPosition = Vector3.zero;
                newCard1.transform.rotation = Quaternion.Euler(0 , 180 , 0);
            }));

        jumpSequence.Join(newCard2.transform.DOJump(
            gridManager.rowData [randomColumnIndex2].cardPositionInRow [randomrowIndex2].transform.position ,
            2.0f , 1 , 1.0f).OnComplete(() =>
            {
                objectsJumped++;
                Debug.Log($"Card 2 jumped: {objectsJumped}/{totalobjectstojump}");
                newCard2.transform.localPosition = Vector3.zero;
                newCard2.transform.rotation = Quaternion.Euler(0 , 180 , 0);;
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
            //Debug.Log("wincheck");
        }

        yield return null ;
    }

    public bool IsObjectsJumpComplete ()
    {
        return objectsJumped >= totalobjectstojump;
    }

    public bool IsWin ()
    {
        return data.Count > 0;
    }

    public bool IsScatterWin ()
    {
        HashSet<int> distinctColumns = new HashSet<int>(); // To store distinct column indices

        foreach (var entry in data)
        {
            if (entry != null && entry.name == "SCATTER")
            {
                distinctColumns.Add(entry.col); // Add the column index to the HashSet
            }
        }

        // Check if there are 3 or more distinct columns with "SCATTER" entries
        return distinctColumns.Count >= 3;
    }


}