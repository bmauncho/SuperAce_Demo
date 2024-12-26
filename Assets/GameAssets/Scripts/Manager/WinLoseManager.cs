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
            if (data [i].name != "SCATTER" ||
                data [i].name != "LITTLE_JOKER" ||
                data [i].name != "BIG_JOKER")
            {
                int row = data [i].row;
                int col = data [i].col;

                // Check if col and row are within valid ranges
                if (col >= 0 && col < gridManager.rowData.Count &&
                    row >= 0 && row < gridManager.rowData [col].cardPositionInRow.Count)
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
            else
            {
                yield return StartCoroutine(RotateWildCards());
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

    IEnumerator RotateWildCards ()
    {
        yield return null ;
    }

    public bool IsWin ()
    {
        return data.Count > 0;
    }
}