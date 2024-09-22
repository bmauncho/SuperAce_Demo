using UnityEngine;

public class DemoSequence : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spin_2 ( Transform whichCard , int col , int row )
    {
        switch (col)
        {
            case 0:
                switch (row)
                {
                    case 0:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Diamonds");

                        break;
                    case 1:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Hearts");
                        break;
                    case 2:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Spades");
                        break;
                    case 3:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "",true);
                        break;
                }
                break;
            case 1:
                switch (row)
                {
                    case 0:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Jack" , false , true);
                        break;
                    case 1:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Queen",false,true);
                        break;
                    case 2:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Ace");
                        break;
                    case 3:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "King");
                        break;
                }
                break;
            case 2:
                switch (row)
                {
                    case 0:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Spades");
                        break;
                    case 1:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Ace");
                        break;
                    case 2:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "" , true);
                        break;
                    case 3:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Hearts");
                        break;
                }
                break;
            case 3:
                switch (row)
                {
                    case 0:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Jack");
                        break;
                    case 1:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Ace");
                        break;
                    case 2:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Hearts");
                        break;
                    case 3:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Diamonds");
                        break;
                }
                break;
            case 4:
                switch (row)
                {
                    case 0:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Hearts");
                        break;
                    case 1:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Spades");
                        break;
                    case 2:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "" , true);
                        break;
                    case 3:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "King");
                        break;
                }
                break;
        }
    }

    public void Spin_1 ( Transform whichCard , int col , int row )
    {
        switch (col)
        {
            case 0:
                switch (row)
                {
                    case 0:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Hearts");
                        break;
                    case 1:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Queen");
                        break;
                    case 2:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "King");
                        break;
                    case 3:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Spades");
                        break;
                }
                break;
            case 1:
                switch (row)
                {
                    case 0:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Hearts" , false , true);
                        break;
                    case 1:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Ace");
                        break;
                    case 2:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Diamonds");
                        break;
                    case 3:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Ace");
                        break;
                }
                break;
            case 2:
                switch (row)
                {
                    case 0:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Jack");
                        break;
                    case 1:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Clubs");
                        break;
                    case 2:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Diamonds");
                        break;
                    case 3:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Hearts");
                        break;
                }
                break;
            case 3:
                switch (row)
                {
                    case 0:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Queen");
                        break;
                    case 1:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Ace");
                        break;
                    case 2:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Queen");
                        break;
                    case 3:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Hearts" , false , true);
                        break;
                }
                break;
            case 4:
                switch (row)
                {
                    case 0:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Ace");
                        break;
                    case 1:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "King");
                        break;
                    case 2:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Hearts");
                        break;
                    case 3:
                        CommandCentre.Instance.CardManager_.DealSpecificDemoCardType(whichCard , "Queen");
                        break;
                }
                break;
        }
    }
}
