using UnityEngine;

public class ViewControl : MonoBehaviour
{
    public GameObject LandScapeView;
    public GameObject PotraitView;
    private void OnEnable()
    {
        SetUp();
    }
    void SetUp()
    {
        Refresh();

    }
    public void Refresh()
    {
        if (FindFirstObjectByType<ViewMan>().IsLandScape)
        {
            LandScapeView.SetActive(true);
            PotraitView.SetActive(false);
        }
        else
        {
            PotraitView.SetActive(true);
            LandScapeView.SetActive(false);
        }
    }
}
