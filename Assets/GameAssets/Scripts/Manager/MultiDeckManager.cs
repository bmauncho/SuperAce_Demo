using UnityEngine;

public class MultiDeckManager : MonoBehaviour
{
    public GameObject DecksParent;
    public int numberOfDecks = 5;
    public Vector3 firstDeckPosition = new Vector3(0 , 0 , 0);
    public Vector3 deckOffset = new Vector3(2 , 0 , 0);
    public Deck [] decks;

    private void Start ()
    {

        Invoke(nameof(InitializeDecks) , .25f);
    }

    public void InitializeDecks ()
    {
        ArrangeDecksInRow();
    }

    void ArrangeDecksInRow ()
    {
        for (int i = 0 ; i < numberOfDecks ; i++)
        {
            if (decks [i] != null)
            {
                Vector3 newPosition = firstDeckPosition + ( deckOffset * i );
                decks [i].transform.position = newPosition;
            }
            else
            {
                Debug.LogError("Deck " + i + " is null");
            }
        }
        DecksParent.SetActive(true);
    }

    public Deck GetDeck ( int index )
    {
        if (index >= 0 && index < decks.Length)
        {
            return decks [index];
        }
        return null;
    }

    public void ClearAllDecks ()
    {
        foreach (Deck deck in decks)
        {
            deck.ClearDeck();
        }
    }
}
