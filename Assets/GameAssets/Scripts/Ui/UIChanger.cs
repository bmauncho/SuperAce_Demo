using UnityEngine;
using UnityEngine.UI;

public class UIChanger : MonoBehaviour
{
    public Image content;
    public Sprite English;
    public Sprite Chinese;

    // Update is called once per frame
    void Update ()
    {
        if (LanguageMan.instance)
        {
            TheLanguage lan = LanguageMan.instance.ActiveLanguage;
            switch (lan)
            {
                case TheLanguage.English:
                    content.sprite = English;
                    break;
                case TheLanguage.Chinese:
                    content.sprite = Chinese;
                    break;
                default:
                    content.sprite = English; // Default to English if no match
                    break;
            }
        }
        else
        {
            content.sprite = English; // Default to English if LanguageMan is not available
        }
    }
}
