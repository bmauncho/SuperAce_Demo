namespace Config_Assets
{
    using UnityEngine;
    using TMPro;
    public class JiliLevelText : MonoBehaviour
    {
        public TMP_Text TheText;
        void Update()
        {
            string TheString = "Current JILI LV";
            TheString = Extra_LanguageMan.instance.FetchTranslation(TheString);
            TheText.text = TheString + " " + ExtraMan.Instance.giftsMan.GetJiliLevel().ToString();
        }
    }
}
