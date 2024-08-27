using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public int poolSize = 50;
    private Queue<GameObject> cardPool;

    // Delegate and event to notify when the pool is initialized
    public bool Poolinitialized = false;

    private void Awake ()
    {
        InitializePool();
    }

    void InitializePool ()
    {
        cardPool = new Queue<GameObject>();

        for (int i = 0 ; i < poolSize ; i++)
        {
            GameObject card = Instantiate(cardPrefab , transform);
            card.SetActive(false);
            cardPool.Enqueue(card);
        }

        //Debug.Log($"Pool initialized with {poolSize} cards.");
        Poolinitialized = true;
    }

    public GameObject GetCard ()
    {
        if (cardPool.Count > 0)
        {
            GameObject card = cardPool.Dequeue();
            card.SetActive(true);
            return card;
        }
        else
        {
            Debug.LogWarning("Pool is empty! Consider increasing pool size.");
            return null;
        }
    }

    public void ReturnCard ( GameObject card )
    {
        if (card)
        {
            // Return a card to the pool
            card.transform.rotation = Quaternion.Euler(0 , 0 , 0);
            if (card.GetComponent<Card>().Outline.sprite != null)
            {
                card.GetComponent<Card>().SetCardOutLine();
            }
            card.transform.SetParent(transform);
            card.transform.localPosition = Vector3.zero;
            card.SetActive(false);
            cardPool.Enqueue(card);
        }
    }
}
