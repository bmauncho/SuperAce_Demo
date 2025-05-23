using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [SerializeField] Hints Hints_;
    public float interval = 10;
    public float timer;
    public int whichHint = 1;
    public bool CanStartTimer;
    public bool CanShowHints = false;
    public int hintCount = 3;
    public TMP_SpriteAsset hint_1_Asset;
    public TMP_SpriteAsset hint_3_Asset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanShowHints)
        {
            timer += Time.deltaTime;

            if (timer >= interval)
            {
                if (CanStartTimer)
                {
                    SetHint(whichHint);
                    CanStartTimer = false;
                }
            }
        }
    }

    public void SetHint(int Hint )
    {
        Hints_.ActiveHint.SetHint(Hint);
        whichHint++;
        if (whichHint > hintCount)
        {
            whichHint = 0;
        }

        timer = 0;
    }
}
