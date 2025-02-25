using UnityEngine;
using TMPro;
public class HistoryMan : MonoBehaviour
{
    public TMP_Text GameNameText;
    public GameObject SelectionObj;
    public GameObject HistoryObj;
    private void OnEnable()
    {
        OpenSelection();
    }
    public void OpenSelection()
    {
        SelectionObj.SetActive(true);
        HistoryObj.SetActive(false);
    }
    public void OpenHistory(HistoryBtn which)
    {
        GameNameText.text = which.TheName;
        SelectionObj.SetActive(false);
        HistoryObj.SetActive(true);
    }

}
