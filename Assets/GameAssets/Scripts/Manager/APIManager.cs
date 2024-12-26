using UnityEngine;
public class APIManager : MonoBehaviour
{
    public GameDataAPI GameDataAPI_;
    public BetPlacingAPI betPlacingAPI_;
    public BetUpdaterAPI betUpdaterAPI_;
    public RefillCardsAPI refillCardsAPI_;

    public void FetchGameData ()
    {
        GameDataAPI_.FetchInfo ();
    }

    public void PlaceBet ()
    {
        betPlacingAPI_.Bet ();
    }

    public void UpdateBet ()
    {
        betUpdaterAPI_.UpdateBet ();
    }
}


