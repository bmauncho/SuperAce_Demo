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

    public void Activate ()
    {
        //Debug.Log("activateMasks");
        CardMask.SetActive(true);
    }

    public void Deactivate ()
    {
        CardMask.SetActive(false);
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
