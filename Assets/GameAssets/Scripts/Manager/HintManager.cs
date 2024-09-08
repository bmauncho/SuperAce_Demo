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
    public Sprite [] HintImage;
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
        for( int i = 0; i < HintImage.Length; i++)
        {
            if(i == Hint)
            {
                Hints_.ActiveHint.GetComponent<Image>().sprite = HintImage[i];
                Hints_.ActiveHint.Activate();
            }
        }

        if(whichHint > HintImage.Length)
        {
            whichHint = 0;
        }
    }
}
