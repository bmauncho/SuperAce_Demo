using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class EventsLoader : MonoBehaviour
{
    public GameObject Spinner;
    public float degreesPerSecond = 20;
    public float interval = 5;
    float timer;

    private Tweener rotationTweener;
    public UnityEvent onComplete;
    private void OnEnable ()
    {
        timer = 0;
        refresh();
    }

    void Update ()
    {
        // Not needed, DOTween will handle the rotation
    }

    public void refresh ()
    {
        timer = 0;
        Activate();
        RotateSpinner();
    }

    void RotateSpinner ()
    {
        if (rotationTweener != null)
        {
            rotationTweener.Kill();
        }
        rotationTweener = Spinner.transform.DORotate(new Vector3(0 , 0 , degreesPerSecond * interval) , interval , RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            { 
                timer = interval;
                Deactivate();
                onComplete.Invoke();
            });
    }

    public void Activate ()
    {
        this.gameObject.SetActive(true);
    }
    public void Deactivate ()
    {
        this.gameObject.SetActive(false);
    }
}