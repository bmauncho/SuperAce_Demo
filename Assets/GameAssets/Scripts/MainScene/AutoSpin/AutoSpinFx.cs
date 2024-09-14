using DG.Tweening;
using UnityEngine;

public class AutoSpinFx : MonoBehaviour
{
    public Transform Target;
    float degreesPerSecond = 360;
    public bool CanShowFx = false;
    public GameObject TheParticleSystem;

    private void Update ()
    {
        if (CanShowFx)
        {
            Target.Rotate(new Vector3(0 , 0 , degreesPerSecond) * Time.deltaTime);
        }
    }

    public void Activate ()
    {
        Target.gameObject.SetActive(true);
        Showfx();
    }

    public void Deactivate ()
    {
        Hidefx();
    }

    public void Showfx ()
    {
        CanShowFx = true;
        Target.GetComponent<CanvasGroup>().DOFade(1 , 5f).OnComplete(() =>
        {
            TheParticleSystem.SetActive(true);
        });
    }

    public void Hidefx ()
    {
        Target.GetComponent<CanvasGroup>().DOFade(0 , 5f).OnComplete(() =>
        {
            CanShowFx = false;
            TheParticleSystem.SetActive(false);
            Target.gameObject.SetActive(false);
        });
    }
}
