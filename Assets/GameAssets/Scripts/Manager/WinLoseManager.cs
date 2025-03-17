using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    public List<winData> tempData = new List<winData>();
    //public List<winData> scatterdata = new List<winData>();

    public int totalobjectstojump = 2;
    public int objectsJumped = 0;
    private  HashSet<string> addedKeys = new HashSet<string>();
    public bool isRotatedGoldenCards = false;
    public bool isWinsequence;
    public bool isJumpingCards;
    List<GameObject> BigJockerCards = new List<GameObject>();
    List<Tuple<GameObject , HashSet<Tuple<int , int>>>> BigJockerRotatedCards = new List<Tuple<GameObject , HashSet<Tuple<int , int>>>>();
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

            tempData.Add(new winData
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
        isWinsequence = true;
        yield return StartCoroutine(showWinningCards());
      
        yield return null;
    }

    public IEnumerator showWinningCards ()
    {
        
       // Debug.Log($"TempData count: {tempData.Count}");
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

        CommandCentre.Instance.SoundManager_.PlaySound("win");
        yield return new WaitForSeconds(.5f);
        Activatecardfx();
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

    public void Activatecardfx ()
    {
        var cardFxManager_ = CommandCentre.Instance.CardFxManager_;
        cardFxManager_.Activate();
        if (cardFxManager_ == null || cardFxManager_.CardFx.Count == 0)
        {
            Debug.LogError("CardfxManager or Cardfx not set up correctly.");
            return;
        }

        for (int i = 0 ; i < data.Count ; i++)
        {
            int row = data [i].row;
            int col = data [i].col;
            var cardFx = cardFxManager_.CardFx [row].cardFxPos [col];

            if (cardFx)
            {
                cardFx.SetActive(true);
                GameObject TheCardfx = CommandCentre.Instance.PoolManager_.GetCardFx();
                TheCardfx.transform.SetParent(cardFx.transform);
                TheCardfx.transform.localPosition = Vector3.zero;
            }
            else
            {
                // Deactivate the card mask if it's not a winning card
                cardFx.SetActive(false);
            }
        }
    }


    IEnumerator HideNormalCards ()
    {
        int hiddenCards = 0;
        List<GameObject> scatterCards = new List<GameObject>();
        List<GameObject> bigJockerCards = new List<GameObject>();

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
                //Debug.Log(" Handle golden cards-1");
                StartCoroutine(rotateNormalGoldenCards(card,col,row));
            }
            // Handle wild cards
            else if (cardComponent.wild && cardComponent.golden && !cardComponent.scatter)
            {
                StartCoroutine(WildCardsSequence(card,col,row));
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
            CommandCentre.Instance.SoundManager_.PlaySound("scatterWin_2");
            yield return StartCoroutine(RotateScatterCards(scatterCards));
        }
        else
        {
            
            CommandCentre.Instance.SoundManager_.PlaySound("hidecards");
        }
        yield return new WaitForSeconds(1.5f);

        // Reset and refill processes
        CommandCentre.Instance.CardFxManager_.ReturnToPool();
        CommandCentre.Instance.CardFxManager_.Deactivate();
        cardFxManager.DeactivateCardFxMask();
        ResetWinDataList();
        ClearAddedKeys();
        // Show current win

        if (CommandCentre.Instance.TurboManager_.TurboSpin_)
        {
            yield return StartCoroutine(refill(true,hiddenCards));
        }
        else
        {
            yield return StartCoroutine(refill(false , hiddenCards));
        }

        if (checkForOtherCards())
        {
            CommandCentre.Instance.PayOutManager_.ShowCurrentWin();
        }
        else
        {
            //Debug.Log($"is scatter win : {IsScatterWin()} is other cards : { checkForOtherCards()}");
            if(IsScatterWin() && !checkForOtherCards())
            {
                CommandCentre.Instance.GridManager_.isRefilling =false;
                CommandCentre.Instance.WinLoseManager_.isWinsequence = false;
            }
        }
        yield return new WaitUntil(() => !CommandCentre.Instance.GridManager_.isRefilling);

        if (BigJockerRotatedCards.Count > 0)
        {
            yield return new WaitForSeconds(.25f);
            Debug.Log("BigJockerRotatedCards More Than 0");
            isJumpingCards = true;

            HashSet<Tuple<int , int>> usedIndices = new HashSet<Tuple<int , int>>();
            List<Tuple<int , int>> indexesList = CommandCentre.Instance.APIManager_.refillCardsAPI_.GetBigJockerIndices();

            int bigJockerCount = BigJockerRotatedCards.Count;
            int indicesPerCard = indexesList.Count / bigJockerCount;
            int extraIndices = indexesList.Count % bigJockerCount;

            int currentIndex = 0;
            int completed = 0;

            for (int i = 0 ; i < bigJockerCount ; i++)
            {
                List<Tuple<int , int>> holderList = new List<Tuple<int , int>>();
                int countToAssign = indicesPerCard + ( i < extraIndices ? 1 : 0 ); // Distribute extra indices evenly

                for (int j = 0 ; j < countToAssign && currentIndex < indexesList.Count ; j++)
                {
                    if (!usedIndices.Contains(indexesList [currentIndex]))
                    {
                        holderList.Add(indexesList [currentIndex]);
                        usedIndices.Add(indexesList [currentIndex]);
                        currentIndex++;
                    }
                }

                if (holderList.Count == 0)
                {
                    completed++;
                    continue;
                }

                yield return StartCoroutine(jumpBigJockerCards(BigJockerRotatedCards [i].Item1 , holderList));
                completed++;
            }

            Debug.Log(completed);
            if (completed >= bigJockerCount)
            {
                isJumpingCards = false;
            }

            yield return new WaitUntil(() => !isJumpingCards);
        }

        tempData.Clear();
        BigJockerRotatedCards.Clear();
        isWinsequence = false;
        yield return null;
    }

    IEnumerator refill(bool isTurbo,int hiddenCards )
    {
        if(isTurbo)
        {
            gridManager.refillTurbo(hiddenCards);
        }
        else
        {
            gridManager.refillGrid(hiddenCards);
        }
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
        CommandCentre.Instance.FreeGameManager_.increaseSpins();
        CommandCentre.Instance.FreeGameManager_.ActivateFreeGameIntro();
        CommandCentre.Instance.SoundManager_.PlaySound("scatterWin");
        // Wait for the intro to complete
        yield return new WaitForSeconds(3);

        CommandCentre.Instance.FreeGameManager_.DeactivateFreeGameIntro();
        data.Clear();
        cardFxManager.DeactivateCardFxMask();
        if (CommandCentre.Instance.DemoManager_.IsDemo)
        {
            CommandCentre.Instance.DemoManager_.isScatterSpin = false;
            CommandCentre.Instance.DemoManager_.DemoSequence_.setUpFirstFreeCards();
        }
        
        CommandCentre.Instance.MainMenuController_.CanSpin = true;
    }



    private IEnumerator rotateNormalGoldenCards ( GameObject card,int col = 0,int row= 0 )
    {
        //Debug.Log(" Handle golden cards-2");
        card.transform.DORotate(Vector3.zero , .2f);
        yield return new WaitForSeconds(.5f);

        // set golden cards to either bigJoker or little jocker
         
        CommandCentre.Instance.CardManager_.SetUpRefillCards(card.GetComponent<Card>(),col,row);
        //Debug.Log(card.GetComponent<Card>().ActiveCardType);
        if(card.GetComponent<Card>().ActiveCardType == CardType.BIG_JOKER)
        {
            HashSet<Tuple<int , int>> positions = new HashSet<Tuple<int , int>>
            {
                Tuple.Create(col, row),
            };
            BigJockerRotatedCards.Add(new Tuple<GameObject , HashSet<Tuple<int , int>>>(card,positions));
        }
        yield return new WaitForSeconds(.5f);
        card.transform.DORotate(new Vector3(0 , 180f , 0) , .2f);
        yield return new WaitForSeconds(.5f);
    }


    private IEnumerator WildCardsSequence (GameObject goldenCard, int col = 0 , int row = 0 )
    {
        if (goldenCard.GetComponent<Card>().ActiveCardType == CardType.BIG_JOKER)
        {
            HashSet<Tuple<int , int>> positions = new HashSet<Tuple<int , int>>
            {
                Tuple.Create(col, row),
            };
            BigJockerRotatedCards.Add(new Tuple<GameObject , HashSet<Tuple<int , int>>>(goldenCard , positions));
        }
        yield return new WaitForSeconds(.5f);
        //goldenCard.transform.DORotate(Vector3.zero , .5f , RotateMode.FastBeyond360).OnComplete(() =>
        //{
        //    StartCoroutine(PunchScaleRotatedCards(goldenCard.transform,col,row));
        //});

        yield return null;
    }

    public IEnumerator PunchScaleRotatedCards ( Transform target, int col = 0 , int row = 0 )
    {
        yield return new WaitForSeconds(.25f);
        Tween PunchScale = target.DOPunchScale(new Vector3(.1f , .1f , .1f) , .5f , 5 , 1).SetEase(Ease.OutQuad);
        yield return PunchScale.WaitForCompletion();
        //rotate
        target.transform.DORotate(new Vector3(0,180f,0) , .5f , RotateMode.FastBeyond360);
    }


    private IEnumerator jumpBigJockerCards ( GameObject target , List<Tuple<int , int>> jumpCards )
    {
        if (jumpCards.Count <= 0)
        {
            yield break;
        }

        // Shake animation
        Tween myTween2 = target.transform.DOShakeRotation(1f , 15 , 10 , 90 , false);
        yield return myTween2.WaitForCompletion();
        Debug.Log($"Start jumping");

        // Store initial position and rotation
        Vector3 initialPosition = target.transform.position;
        Quaternion initialRotation = target.transform.rotation;

        List<GameObject> newCards = new List<GameObject>();

        foreach (var jumpCard in jumpCards)
        {
            GameObject newCard = poolManager.GetCard();
            newCard.transform.SetPositionAndRotation(initialPosition , initialRotation);
            newCard.SetActive(true);

            int columnIndex = jumpCard.Item1;
            int rowIndex = jumpCard.Item2;

            // Assign the new card to the grid
            Transform targetTransform = gridManager.rowData [columnIndex].cardPositionInRow [rowIndex].transform;
            newCard.transform.SetParent(targetTransform);

            CardPos cardPos = targetTransform.GetComponent<CardPos>();
            cardPos.TheOwner.SetActive(false);
            cardPos.TheOwner = newCard;

            // Set the new card as a BigJoker
            CommandCentre.Instance.CardManager_.setSpecificCard(newCard.GetComponent<Card>() , "BIG_JOKER");

            newCards.Add(newCard);
        }

        // Return inactive cards to the pool
        CommandCentre.Instance.PoolManager_.ReturnAllInactiveCardsToPool();

        // Create DOTween sequence for jumping
        var jumpSequence = DOTween.Sequence();
        Debug.Log($"Start sequence");

        foreach (var newCard in newCards)
        {
            Transform targetPos = newCard.transform.parent;
            jumpSequence.Join(newCard.transform.DOJump(
                targetPos.position , 3.0f , 1 , 1.0f).OnComplete(() =>
                {
                    objectsJumped++;
                    newCard.transform.localPosition = Vector3.zero;
                    newCard.transform.rotation = Quaternion.Euler(0 , 180 , 0);
                }));
        }

        if (jumpSequence.IsActive())
        {
            yield return jumpSequence.Play().WaitForCompletion();
        }

        yield return new WaitForSeconds(0.5f);

        while (!IsObjectsJumpComplete())
        {
            objectsJumped++;
        }

        if (IsObjectsJumpComplete())
        {
            isJumpingCards = false;
        }
    }


    public bool IsObjectsJumpComplete ()
    {
        return objectsJumped >= totalobjectstojump;
    }

    public bool IsWin ()
    {
        return data.Count >= 3;
    }

    public bool IsScatterWin ()
    {
        HashSet<int> distinctColumns = new HashSet<int>(); // Store unique column indices
        List<winData> toRemove = new List<winData>(); // Store SCATTER entries for removal
        GameDataAPI gameDataAPI = CommandCentre.Instance.APIManager_.GameDataAPI_;
        if (tempData == null || tempData.Count == 0)
        {
            //Debug.LogWarning("TempData is empty or null.");
            return false;
        }

        if (gameDataAPI.IsFreeGame())
        {
            return true;
        }

        foreach (var entry in tempData)
        {
            if (entry != null && entry.name == "SCATTER")
            {
                //Debug.Log($"Found SCATTER at column: {entry.col}"); // Debugging
                distinctColumns.Add(entry.col);
                toRemove.Add(entry);
            }
        }

        //Debug.Log($"Distinct columns count: {distinctColumns.Count}");

        // Ensure at least 3 unique columns contain SCATTER cards
        if (distinctColumns.Count < 3)
        {
            foreach (var entry in toRemove)
            {
                data.Remove(entry);
                tempData.Remove(entry);
            }
            return false;
        }

        return true;
    }



    public bool checkForOtherCards ()
    {
        HashSet<string> otherCardsList = new HashSet<string>();
        if (tempData == null || tempData.Count == 0)
        {
            //Debug.LogWarning("checkForOtherCards - TempData is empty or null.");
            return false;
        }
        foreach (var entry in tempData)
        {
            if (entry != null && entry.name != "SCATTER")
            {
                otherCardsList.Add(entry.name);
            }
        }

        if(otherCardsList.Count > 0)
        {
            return true;
        }

        return false;
    }

}