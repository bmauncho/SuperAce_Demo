using UnityEngine;
using UnityEngine.UI;
public class HeaderBtns : MonoBehaviour
{
    public bool CanDeactivateBtn = true;
    [System.Serializable]
    public class HeaderFunction
    {
        public GameObject TargetObj;
        public Button TheBtn;
        public Button[] SameBtns;
        public GameObject TheBtnHolder;
    }
    public HeaderFunction[] funtions;
    private void Start()
    {
        Pressed(funtions[0].TheBtn);
    }
    public void Pressed(Button Which)
    {
        for(int i = 0; i < funtions.Length; i++)
        {
            if(funtions[i].TheBtn == Which)
            {
                if (funtions[i].TargetObj)
                {
                    funtions[i].TargetObj.SetActive(true);
                }
                if (funtions[i].TheBtn&&CanDeactivateBtn)
                {
                    funtions[i].TheBtn.interactable = false;

                }
              
                if (funtions[i].TheBtnHolder) { 
                    funtions[i].TheBtnHolder.SetActive(true);

                }
               
            }
            else
            {
                if (funtions[i].TargetObj)
                {
                    funtions[i].TargetObj.SetActive(false);

                }
                if (funtions[i].TheBtn&&CanDeactivateBtn)
                {
                    funtions[i].TheBtn.interactable = true;


                }
                if (funtions[i].TheBtnHolder)
                {
                    funtions[i].TheBtnHolder.SetActive(false);

                }
            }

            for (int r = 0; r < funtions[i].SameBtns.Length; r++)
            {
                funtions[i].SameBtns[r].interactable = funtions[i].TheBtn.interactable;
            }
        }
    }
}
