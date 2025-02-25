using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MissionStat : MonoBehaviour
{
    public GameObject LockObj;
    public TMP_Text CountText;
    public TMP_Text TotalBetText;
    public Slider FillSlider;
    public int Level;
    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (!ExtraMan.Instance)
        {
            Invoke(nameof(Refresh), 0.1f);
            return;
        }
        float Amount = ExtraMan.Instance.missionsMan.GetMissionPoints(Level);
        float MaxAmount = ExtraMan.Instance.missionsMan.MissionGoals[Level];

        FillSlider.value = Amount / MaxAmount;
        CountText.text = "(" + Amount.ToString() + "/" + MaxAmount.ToString() + ")";
        string TheString = "Total bet over";
        TheString = Extra_LanguageMan.instance.FetchTranslation(TheString);
        TotalBetText.text = TheString+" " + MaxAmount.ToString();


        LockObj.SetActive(!ExtraMan.Instance.missionsMan.IsMissionUnlocked(Level));
        if (LockObj.activeSelf)
        {

        }
        else
        {

        }
    }
}
