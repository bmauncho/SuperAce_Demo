using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    private void Start ()
    {
        gridManager = CommandCentre.Instance.GridManager_;
        poolManager = CommandCentre.Instance.PoolManager_;
        cardFxManager = CommandCentre.Instance.CardFxManager_;
        data.Clear();
    }
    public void GetWinningCard (CardData _data,int row, int col)
    {
        data.Add(new winData
        {
            name = _data.name,
            row = row,
            col = col,
        });
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
        CommandCentre.Instance.CardFxManager_.DeactivateCardFxMask();
        CommandCentre.Instance.CardFxManager_.ActivateCardMask();
        for (int i = 0;i<data.Count ; i++)
        {
            CommandCentre.Instance.CardFxManager_.ActivateCardFxMask(data [i].row , data [i].col);
        }

        yield return new WaitForSeconds(1); 

        yield return StartCoroutine(HideNormalCards());

        yield return null;
    }

    IEnumerator HideNormalCards ()
    {
        int hiddenCards = 0;

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
                StartCoroutine(RotateWildCards(card));
            }
            // Handle scatter cards
            else if (cardComponent.scatter && !cardComponent.wild && !cardComponent.golden)
            {
                cardComponent.ScatterSpin.GetComponentInChildren<ScatterMotions>().Rotate();
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

        yield return new WaitForSeconds(1.5f);

        // Reset and refill processes
        cardFxManager.DeactivateCardFxMask();
        ResetWinDataList();

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



    private IEnumerator rotateNormalGoldenCards ( GameObject card,int col = 0,int row= 0 )
    {
        Debug.Log(" Handle golden cards-2");
        card.transform.DORotate(Vector3.zero , .2f)
            .OnComplete(() =>
            {
                CommandCentre.Instance.CardManager_.setUpCard(card.GetComponent<Card>(),col,row);
            });
        yield return new WaitForSeconds(1f);
        card.transform.DORotate(new Vector3(0 , 180f , 0), .2f);

        // set 
    }


    private IEnumerator RotateWildCards (GameObject goldenCard)
    {
        goldenCard.transform.DORotate(Vector3.zero , .5f , RotateMode.FastBeyond360).OnComplete(() =>
        {
            StartCoroutine(PunchScaleRotatedCards(goldenCard.transform));
        });

        yield return null;
    }

    public IEnumerator PunchScaleRotatedCards ( Transform target )
    {
        yield return new WaitForSeconds(.25f);
        Tween PunchScale = target.DOPunchScale(new Vector3(.1f , .1f , .1f) , .5f , 5 , 1).SetEase(Ease.OutQuad);
        yield return PunchScale.WaitForCompletion();

        //setUp Replacement card

        //rotate
        target.transform.DORotate(new Vector3(0,180f,0) , .5f , RotateMode.FastBeyond360);

        // if Big joker jump two cards to random positions which is not the current card pos
    }

    public bool IsWin ()
    {
        return data.Count > 0;
    }
}