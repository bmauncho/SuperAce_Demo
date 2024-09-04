using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class TotalWinUI : MonoBehaviour
{
    public TMP_Text TotalWinings;

    private void OnEnable ()
    {
        StartCoroutine(AnimateWinnings());
    }

    public IEnumerator AnimateWinnings ()
    {
        float endValue = CommandCentre.Instance.CashManager_.CurrentWinings;
        float duration = 1;
        TotalWinings.text = "0";
        Tween t = DOTween.To(GetCurrentValue , SetValue , endValue , duration)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        TotalWinings.text = endValue.ToString("F0");
                    });

        yield return new WaitForSeconds(3);
        CommandCentre.Instance.PayOutManager_.HideTotalWinnings();
    }


    float GetCurrentValue ()
    { 
        return 0; 
    }

    void SetValue ( float x ) 
    { 
        TotalWinings.text = x.ToString("F0"); 
    }
}
