using System.Collections;
using UnityEngine;

public class CardPosGenerator : MonoBehaviour
{
    public GameObject CardPositioner;
    public Vector3 Spacing = new Vector3(1.5f , 1.5f , 0f);
    public int gridSizeX = 5;
    public int gridSizeY = 4;

    [ContextMenu("Create Grid")]
    void CreateGrid ()
    {
        int thepos = 1;
        for (int col = 0 ; col < gridSizeX ; col++)
        {
            for (int row = 0 ; row < gridSizeY ; row++)
            {
                GameObject card = Instantiate(CardPositioner , transform);
                card.name = "Pos_" + thepos.ToString();
                Vector3 targetPos = new Vector3(col * Spacing.x , row * Spacing.y , 0f);
                card.transform.localPosition = targetPos;
                thepos++;
            }
        }
    }
}
