using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pay
{
    public string SymbolName;
    public List<float> Payouts = new List<float>();
}
public class PayOutManager : MonoBehaviour
{
    public List<Pay> PayList = new List<Pay>();

    private Dictionary<string , int> cardToIndex = new Dictionary<string , int>()
    {
        {"Ace", 0},
        {"King", 1},
        {"Queen", 2},
        {"Jack", 3},
        {"Hearts", 4},
        {"Spades", 5},
        {"Diamonds", 6},
        {"Clubs", 7}
    };

    [ContextMenu("Get PayOutAmount")]
    public void Test ()
    {
        GetCardPayOut("Jack" , 4);
    }

    public float GetCardPayOut ( string card , int column )
    {
        
        if (cardToIndex.TryGetValue(card , out int index))
        {
            if (column >= 3 && column <= 5)
            {
                Debug.Log(card + " : " + PayList [index].Payouts [column - 3]);
                return PayList [index].Payouts [column - 3];
            }
        }
        return 0;
    }

    public float TotalWinings (float Payout,int PayLines,float Bet,float Combo)
    {
        return Payout*PayLines*Bet*Combo;
    }
}
