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
        Vector3 newScale = new Vector3 (1.1f, 1.1f, 1.1f);
        Tween PunchScale = transform.DOPunchScale(newScale , 10 , 1);
        yield return PunchScale.WaitForCompletion();
        transform.localScale = Vector3.one;
    }
}
