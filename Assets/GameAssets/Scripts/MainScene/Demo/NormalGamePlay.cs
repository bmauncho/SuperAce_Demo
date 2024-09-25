using DG.Tweening;
using UnityEngine;

public class NormalGamePlay : MonoBehaviour
{
    public GameObject currentWinnings;
    public float BounceAmt;

    public void Bounce ()
    {
        currentWinnings.transform.DOScale(BounceAmt , .25f).OnComplete(() =>
        {
            Debounce();
        });
    }

    public void Debounce ()
    {
        currentWinnings.transform.DOScale(1 , .25f);
    }
}
