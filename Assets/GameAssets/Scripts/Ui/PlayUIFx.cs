using Coffee.UIEffects;
using UnityEngine;
using DG.Tweening;

public class PlayUIFx : MonoBehaviour
{
    public UIShiny UIShiny_;
    [Header("Width")]
    [Space(10)]
    public float myValue;        // The float value to animate
    public float targetValue = 10f;  // Target value
    public float duration = 2f;  // Duration over which the value changes
    private float elapsedTime = 0f;
    private float startValue;    // The initial value before the tween starts

    [Header("Brightness")]
    [Space(10)]
    public float myBrightnessValue;        // The float value to animate
    public float BrightnessTargetValue = 10f;  // Target value
    private float BrightnessStartValue;    // The initial value before the tween starts


    [Header("General")]
    [Space(10)]
    public bool canPlayEffect = false;
    public bool CanLoop = false;

    public GameObject Sparkle1;
    bool CanshowPs_1;
    public GameObject Sparkle2;
    bool CanshowPs_2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlayEffect)
        {
            ShowEffect();
        }
    }
    [ContextMenu("PlayEffect")]
    public void PlayEffect ()
    {
        Sparkle1.SetActive(false);
        Sparkle2.SetActive(false);
        elapsedTime = 0;
        myValue = 0;
        startValue = myValue;
        myBrightnessValue = .5f;
        BrightnessStartValue = myBrightnessValue;

        canPlayEffect = true;
        CanshowPs_1 = false;
        CanshowPs_2 = false;

    }

    void ShowEffect ()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        myValue = Mathf.SmoothStep(startValue , targetValue , t);
        UIShiny_.width = myValue;
       
        BrigtnessEffect();

        if (elapsedTime >= duration)
        {
            if (!CanshowPs_1)
            {
                CanshowPs_1 = true;
                Sparkle1.SetActive(true);
            }

            elapsedTime = duration;
            canPlayEffect = false;
            Invoke(nameof(ShowSparkle2) , .5f);
            if (CanLoop)
            {
                Invoke(nameof(PlayEffect),1f);
            }

        }
    }
    void ShowSparkle2 ()
    {
        if (!CanshowPs_2)
        {
            CanshowPs_2 = true;
            Sparkle2.SetActive(true);
        }
    }
    void BrigtnessEffect ()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        myBrightnessValue = Mathf.SmoothStep(BrightnessStartValue , BrightnessTargetValue , t);
        UIShiny_.brightness = myBrightnessValue;
    }

}
