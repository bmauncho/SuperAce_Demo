using UnityEngine;

public class HistoryBtn : MonoBehaviour
{
    public string TheName = "Game_A";
    public void Pressed()
    {
        GetComponentInParent<HistoryMan>().OpenHistory(this);
    }
}
