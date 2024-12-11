using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("Cards")]
    [Space(10)]
    public GameObject cardPrefab;
    public int poolSize = 50;
    private Queue<GameObject> cardPool;

    public GameObject CardPosholder;
    public List<GameObject> InactiveCardList;
    // Delegate and event to notify when the pool is initialized
    public bool Poolinitialized = false;
    [Header("Cards fx")]
    [Space(10)]
    public GameObject cardfx;
    public int fxPoolSize = 20;
    private Queue<GameObject> cardfxPool;

    [Header("Scatter Cards fx")]
    [Space(10)]
    public GameObject ScatterCardfx;
    public int ScatterCardfxPoolSize = 20;
    private Queue<GameObject> ScatterCardfxPool;

    private void Awake ()
    {
        InitializePool();
        InitializeCardFxPool();
        InitializeScatterCardFxPool();
    }
    #region
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
            card.transform.SetParent(transform);
            card.transform.localPosition = Vector3.zero;
            card.SetActive(false);
            cardPool.Enqueue(card);
        }
    }

  

    [ContextMenu("Get Inactive Cards")]
    void GetAllInactiveCards ()
    {
        List <Transform> templist = new List <Transform>();
        foreach (Transform tr in CardPosholder.transform)
        {
            templist.Add(tr);
        }

        for(int i = 0; i < templist.Count; i++)
        {
            foreach (Transform tr in templist[i])
            {
                if (!tr.gameObject.activeSelf && tr.GetComponent<Card>())
                {

                    InactiveCardList.Add(tr.gameObject);
                }
            }
        }

    }

    public void ReturnAllInactiveCardsToPool ()
    {
        GetAllInactiveCards();
        foreach(GameObject card in InactiveCardList)
        {
            ReturnCard(card);
        }
        InactiveCardList.Clear();
    }
    #endregion 

    #region
    void InitializeCardFxPool ()
    {
        cardfxPool = new Queue<GameObject>();

        for (int i = 0 ; i < fxPoolSize ; i++)
        {
            GameObject cardfx_ = Instantiate(cardfx , transform);
            cardfx_.SetActive(false);
            cardfxPool.Enqueue(cardfx_);
        }
    }

    public GameObject GetCardFx ()
    {
        if (cardfxPool.Count > 0)
        {
            GameObject cardfx_ = cardfxPool.Dequeue();
            cardfx_.SetActive(true);
            return cardfx_;
        }
        else
        {
            Debug.LogWarning("fxPool is empty! Consider increasing fxpool size.");
            return null;
        }
    }

    public void ReturnFx ( GameObject cardfx_ )
    {
        if (cardfx_)
        {
            cardfx_.transform.SetParent(transform);
            cardfx_.SetActive(false);
            cardfxPool.Enqueue(cardfx_);
        }
    }
    #endregion

    #region
    void InitializeScatterCardFxPool ()
    {
        ScatterCardfxPool = new Queue<GameObject>();

        for (int i = 0 ; i < ScatterCardfxPoolSize ; i++)
        {
            GameObject ScatterCardfx_ = Instantiate(ScatterCardfx , transform);
            ScatterCardfx_.SetActive(false);
            ScatterCardfxPool.Enqueue(ScatterCardfx_);
        }
    }

    public GameObject GetScatterCardFx ()
    {
        if (ScatterCardfxPool.Count > 0)
        {
            GameObject ScatterCardfx_ = ScatterCardfxPool.Dequeue();
            ScatterCardfx_.SetActive(true);
            return ScatterCardfx_;
        }
        else
        {
            Debug.LogWarning("fxPool is empty! Consider increasing fxpool size.");
            return null;
        }
    }

    public void ReturnScatterFx ( GameObject ScatterCardfx_ )
    {
        if (ScatterCardfx_)
        {
            ScatterCardfx_.transform.SetParent(transform);
            ScatterCardfx_.SetActive(false);
            ScatterCardfxPool.Enqueue(ScatterCardfx_);
        }
    }
    #endregion
}
