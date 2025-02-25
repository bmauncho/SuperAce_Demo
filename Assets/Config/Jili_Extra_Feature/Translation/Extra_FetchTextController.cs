using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Extra_FetchTextController : MonoBehaviour
{
    public TextMeshProUGUI myText;
    public string CODE;
    private void OnEnable()
    {
        Setup();
        
    }
    void Setup()
    {
        if (!Extra_LanguageMan.instance||Extra_LanguageMan.instance&&Extra_LanguageMan.instance.Data.Length==0)
        {
            Invoke(nameof(Setup), 0.1f);
        }
        else
        {
            RefreshFetch();
        }
    }
    [ContextMenu("Refresh")]
    public void RefreshFetch() 
    {
        if (CODE == "")
            return;
        myText.SetText(Extra_LanguageMan.instance.RequestForText(CODE));
    }
}
