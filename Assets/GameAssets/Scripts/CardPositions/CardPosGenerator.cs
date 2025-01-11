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

    [ContextMenu("Create Grid")]
    void CreateGrid ()
    {
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
