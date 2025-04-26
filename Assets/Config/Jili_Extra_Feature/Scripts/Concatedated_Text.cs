namespace Config_Assets
{
    using UnityEngine;
    using TMPro;
    [System.Serializable]
    public class ConcaText
    {
        public string TheText;
        public bool UseTranslation = true;
        public bool FetchTime;
    }
    public class Concatedated_Text : MonoBehaviour
    {
        public ConcaText[] TheTexts;
        private void OnEnable()
        {
            string FullString = "";
            for (int i = 0; i < TheTexts.Length; i++)
            {
                string which = TheTexts[i].TheText;
                if (TheTexts[i].UseTranslation)
                {
                    which = Extra_LanguageMan.instance.FetchTranslation(which);
                }
                else if (TheTexts[i].FetchTime)
                {
                    which = ExtraMan.Instance.missionsMan.MidnightTime().ToShortTimeString();
                }
                FullString = FullString + which;
            }
            GetComponent<TMP_Text>().text = FullString;
        }
    }
}
