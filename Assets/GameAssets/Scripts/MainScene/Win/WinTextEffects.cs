using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTextEffects : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(showEffects());
    }

    IEnumerator showEffects ()
    {
        Vector3 newScale = new Vector3 (0.2f, 0.2f, 0.2f);
        Tween PunchScale = transform.DOPunchScale(newScale ,.5f, 10 , 1);
        yield return PunchScale.WaitForCompletion();
        transform.localScale = Vector3.one;
    }
}
