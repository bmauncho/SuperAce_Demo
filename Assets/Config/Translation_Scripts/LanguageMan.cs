using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public enum TheLanguage
{
    English,
    Chinese,
    Japan,
    Spanish,
    Swahili,
    Danish,
    Thai,
    Indonesia,
    Vietnam,
    Portoguese,
    Korea,
    Burmese
}

public class LanguageMan : MonoBehaviour
{
    public TextAsset TranslationDocument;
    public TheLanguage ActiveLanguage;
    public string All_Game_Text;
    public string[] Data;
    public static LanguageMan instance;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

        }
        Data = TranslationDocument.text.Split(new string[] { "\t", "\n" }, System.StringSplitOptions.None);
        RefreshAll();
        if (Data.Length == 0)
        {
            

        }
        SetExtraLanguage();

    }
   

    public string RequestForText(string CODE)
    {
        for (int i = 0; i < Data.Length; i++)
        {
            if (CODE == Data[i])
            {
                return Data[i + (int)ActiveLanguage+1];
            }
        }

        return "ERROR NO TEXT FOUND";
    }
    [ContextMenu("Refresh")]
    public void RefreshAll()
    {
        if (!Application.isPlaying)
            return;
        FetchTextController[] texts = FindObjectsOfType<FetchTextController>();
        for(int i = 0; i < texts.Length; i++)
        {
            texts[i].RefreshFetch();
        }
    }
    [ContextMenu("AssignCode")]
    public void AssignCodes()
    {
        Data = TranslationDocument.text.Split(new string[] { "\t", "\n" }, System.StringSplitOptions.None);
        FetchTextController[] texts = FindObjectsOfType<FetchTextController>(true);
        for (int i = 0; i < texts.Length; i++)
        {
            bool found = false;
            string thetext = texts[i].GetComponent<TMP_Text>().text;
            for (int r = 0; r < Data.Length; r++)
            {
//                Debug.Log(Data[r]);
                if (Data[r] == thetext)
                {
                    Debug.Log(Data[r]);

                    texts[i].CODE = Data[r - 1];
#if UNITY_EDITOR
                    EditorUtility.SetDirty(texts[i]);
#endif
                    found = true;
                    break;
                }
                
            }
            if (!found)
            {
                Debug.Log("NoTrans_" + thetext);
            }

        }
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
#endif
    }
    public void SetLanguage(Dropdown LanguageDropDown)
    {
        ActiveLanguage = (TheLanguage)LanguageDropDown.value;
        RefreshAll();

        SetExtraLanguage();
    }
    public void _SetLanguage(TheLanguage _Language)
    {
        ActiveLanguage = _Language;
        RefreshAll();

        SetExtraLanguage();
    }
    void SetExtraLanguage()
    {
        if (!Extra_LanguageMan.instance)
        {
            Invoke(nameof(SetExtraLanguage), 1f);
        }
        else
        {
            Extra_LanguageMan.instance.SetLanguage(ActiveLanguage);
        }
    }
    public string FetchTranslation(string TheString)
    {
        for (int r = 0; r < Data.Length; r++)
        {
            if (Data[r] == TheString)
            {
                return Data[r + (int)ActiveLanguage];
            }

        }
        return TheString;
    }
}
