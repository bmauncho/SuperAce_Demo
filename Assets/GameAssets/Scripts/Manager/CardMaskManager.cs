using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardMaskColumn
{
    public GameObject CardColumnHolder;
    public List<GameObject> cardMasks = new List<GameObject>();
}

public class CardMaskManager : MonoBehaviour
{
    public GameObject CardMask;
    public List<CardMaskColumn> CardMasks = new List<CardMaskColumn>();
    public List<GridPosColumns> Columns = new List<GridPosColumns>();
    public void Activate ()
    {
        //Debug.Log("activateMasks");
        CardMask.SetActive(true);
    }

    public void Deactivate ()
    {
        CardMask.SetActive(false);
    }

    public void DeactivateNormalcards ()
    {
        for (int columnIndex = 0 ; columnIndex < Columns.Count ; columnIndex++)
        {
            GridPosColumns column = Columns [columnIndex];

            for (int cardIndex = 0 ; cardIndex < column.CardsPos.Count ; cardIndex++)
            {
                GameObject cardPos = column.CardsPos [cardIndex];
                CardPos cardPosScript = cardPos.GetComponent<CardPos>();

                if (cardPosScript != null && cardPosScript.TheOwner != null)
                {
                    // Check if the card has a rotation of 180 degrees on the Y-axis
                    if (cardPosScript.TheOwner.transform.rotation == Quaternion.Euler(0 , 180 , 0))
                    {
                        // Assuming the mask is in the same position in the CardMasks list as the card in the Columns
                        if (columnIndex < CardMasks.Count && cardIndex < CardMasks [columnIndex].cardMasks.Count)
                        {
                            GameObject cardMask = CardMasks [columnIndex].cardMasks [cardIndex];

                            if (cardMask.activeSelf)
                            {
                                // Deactivate the card mask
                                cardMask.SetActive(false);
                                cardMask.transform.localScale = Vector3.one;
                            }
                        }
                    }
                }
            }
        }
    }

    public void DeactivateAllCardMasks ()
    {
        for(int i = 0; i < CardMasks.Count; i++)
        {
            for(int j = 0 ; j < CardMasks [i].cardMasks.Count ; j++)
            {
                if(CardMasks [i].cardMasks [j].activeSelf)
                {
                    CardMasks [i].cardMasks [j].SetActive(false);
                    CardMasks [i].cardMasks [j].transform.localScale = Vector3.one;

                }
            }
        }
    }
}
