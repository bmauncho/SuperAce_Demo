using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WinCardData
{
    public string name;
    public string Substitute;
    public bool isGolden;
    public bool canFlip;
    public int [,] position;
}

public class DemoWinLoseManager : MonoBehaviour
{
    public DemoSequence demoSequence;
    public DemoGridManager demoGridManager;
    public List<WinCardData> winCards = new List<WinCardData>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanRefill ()
    {
        var demoCards = demoSequence.demoCards;
        for (int i = 0;i< demoCards.Count ; i++)
        {
            var cards = demoCards[i];
            for(int j = 0 ; j < cards.cards.Count ; j++)
            {
                if (!string.IsNullOrEmpty(cards.cards [j].Subsitute))
                {
                    winCards.Add(new WinCardData
                    {
                        name = cards.cards [j].name,
                        Substitute = cards.cards [j].Subsitute,
                    });
                    return true;
                }
            }
        }
        return false;
    }
}
