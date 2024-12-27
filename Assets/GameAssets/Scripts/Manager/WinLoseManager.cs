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
            if (data [i].name == "SCATTER" ||
                data [i].name == "LITTLE_JOKER" ||
                data [i].name == "BIG_JOKER" ||
                data [i].name == "WILD")
            {

                if (col >= 0 && col < gridManager.rowData.Count &&
                    row >= 0 && row < gridManager.rowData [col].cardPositionInRow.Count)
                {
                    GameObject cardPosHolder = gridManager.rowData [row].cardPositionInRow [col];
                    CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
                    GameObject card = cardPos.TheOwner;

                    if (card)
                    {
                        CardType cardType = card.GetComponent<Card>().ActiveCardType;
                        switch (cardType)
                        {
                            case CardType.SCATTER:
                                card.GetComponent<Card>().ScatterSpin.GetComponentInChildren<ScatterMotions>().Rotate();
                                break;
                            case CardType.LITTLE_JOKER:
                            case CardType.BIG_JOKER:
                                StartCoroutine(RotateGoldenCards(card));
                                break;
                            case CardType.WILD:
                                break;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"Invalid row or column index: row={row}, col={col}");
                }
            }
            else
            {
                // Check if col and row are within valid ranges
                if (col >= 0 && col < gridManager.rowData [col].cardPositionInRow.Count  &&
                    row >= 0 && row < gridManager.rowData.Count)
                {
                    GameObject cardPosHolder = gridManager.rowData [row].cardPositionInRow [col];
                    CardPos cardPos = cardPosHolder.GetComponent<CardPos>();
                    GameObject card = cardPos.TheOwner;

                    if (card)
                    {
                        card.SetActive(false);
                        poolManager.ReturnCard(card.GetComponent<Card>().gameObject);
                        cardPos.TheOwner = null;
                        hiddenCards++;
                    }
                }
                else
                {
                    Debug.LogWarning($"Invalid row or column index: row={row}, col={col}");
                }
               
            }
        }
        yield return new WaitForSeconds(1.5f);
        cardFxManager.DeactivateCardFxMask ();
        ResetWinDataList ();
        if (CommandCentre.Instance.TurboManager_.TurboSpin_)
        {
            gridManager.refillTurbo (hiddenCards);
        }
        else
        {
            gridManager.refillGrid (hiddenCards);
        }
        CommandCentre.Instance.PayOutManager_.ShowCurrentWin();
        yield return null;
    }


    private IEnumerator RotateGoldenCards (GameObject goldenCard)
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

        target.transform.DORotate(new Vector3(0,180f,0) , .5f , RotateMode.FastBeyond360);
    }

    IEnumerator RotateWildCards ()
    {
        for(int i = 0;i<data.Count;i++)
        {
            if(data [i] != null)
            {
                if(data [i].name == "LITTLE_JOKER" ||data [i].name == "BIG_JOKER")
                {

                }
            }
        }
        yield return null ;
    }

    public bool IsWin ()
    {
        return data.Count > 0;
    }
}