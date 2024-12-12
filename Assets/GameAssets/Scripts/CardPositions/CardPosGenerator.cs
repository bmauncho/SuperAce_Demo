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
        for (int col = 0 ; col < gridSizeX ; col++)
        {
            for (int row = 0 ; row < gridSizeY ; row++)
            {
                GameObject card = Instantiate(CardPositioner , transform);
                card.name = ItemName+"_"+ thepos.ToString();
                Vector3 targetPos = new Vector3(col * Spacing.x , row * Spacing.y , 0f);
                card.transform.localPosition = targetPos;
                thepos++;
            }
        }
    }

    [ContextMenu("Clear Grid")]
    void clear ()
    {
        List<Transform>objectsToDestroy = new List<Transform>();
        foreach(Transform t in transform)
        {
            objectsToDestroy.Add(t);
        }

        foreach(Transform t in objectsToDestroy)
        {
            DestroyImmediate(t.gameObject);
        }
    }

    [ContextMenu("Disable Markers")]
    void disableMarkers ()
    {
        List<Transform> markersToHide = new List<Transform>();
        foreach (Transform t in transform)
        {
            markersToHide.Add(t);
        }

        foreach(Transform t in markersToHide)
        {
            foreach(Transform _t in t)
            {
                _t.gameObject.SetActive(false);
            }
        }
    }

    [ContextMenu("enable Markers")]
    void enableMarkers ()
    {
        List<Transform> markersToShow = new List<Transform>();
        foreach (Transform t in transform)
        {
            markersToShow.Add(t);
        }

        foreach (Transform t in markersToShow)
        {
            foreach (Transform _t in t)
            {
                _t.gameObject.SetActive(true);
            }
        }
    }
}
