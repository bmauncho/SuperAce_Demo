using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPosGenerator : MonoBehaviour
{
    public GameObject CardPositioner;
    public Vector3 Spacing = new Vector3(1.5f , 1.5f , 0f);
    public int gridSizeX = 5;
    public int gridSizeY = 4;
    public string ItemName;
    public List<GameObject> Items = new List<GameObject>();
    public GridManager gridManager;
    public DemoGridManager demoGridManager;

    [ContextMenu("Create Grid")]
    void CreateGrid ()
    {
        Items.Clear();
        int thepos = 1;
        for (int row = 0 ; row < gridSizeY ; row++)
        {
            for (int col = 0 ; col < gridSizeX ; col++)
            {
                GameObject card = Instantiate(CardPositioner , transform);
                card.name = ItemName + "_" + thepos.ToString();
                Vector3 targetPos = new Vector3(col * Spacing.x , -row * Spacing.y , 0f); // Adjust Y direction if necessary
                card.transform.localPosition = targetPos;
                thepos++;
                Items.Add(card);
            }
        }

        AddToDemoGridManager();
        AddToGridManager();
    }

    public void AddToGridManager ()
    {
        gridManager.rowData.Clear();
        for(int row = 0 ;row <4 ; row++)
        {
            gridManager.rowData.Add(new cardPositions());
        }
        int index = 0;
        for(int i = 0 ;i < gridManager.rowData.Count; i++)
        {
            for(int j = 0 ;j<5 ;j++)
            {
                gridManager.rowData [i].cardPositionInRow.Add(Items [index]);
                index++;
            }
        }
    }

    public void AddToDemoGridManager ()
    {
        demoGridManager.colData.Clear();
        for (int row = 0 ; row < 4 ; row++)
        {
            demoGridManager.colData.Add(new cardPositions());
        }
        int index = 0;
        for (int i = 0 ; i < demoGridManager.colData.Count ; i++)
        {
            for (int j = 0 ; j < 5 ; j++)
            {
                demoGridManager.colData [i].cardPositionInRow.Add(Items [index]);
                index++;
            }
        }
    }

    [ContextMenu("Clear Grid")]
    void Clear ()
    {
        List<Transform> objectsToDestroy = new List<Transform>();
        foreach (Transform t in transform)
        {
            objectsToDestroy.Add(t);
        }

        foreach (Transform t in objectsToDestroy)
        {
            DestroyImmediate(t.gameObject);
        }
    }

    [ContextMenu("Disable Markers")]
    void DisableMarkers ()
    {
        foreach (Transform t in transform)
        {
            foreach (Transform _t in t)
            {
                _t.gameObject.SetActive(false);
            }
        }
    }

    [ContextMenu("Enable Markers")]
    void EnableMarkers ()
    {
        foreach (Transform t in transform)
        {
            foreach (Transform _t in t)
            {
                _t.gameObject.SetActive(true);
            }
        }
    }
}
