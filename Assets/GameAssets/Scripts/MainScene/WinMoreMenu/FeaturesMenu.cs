using UnityEngine;
using UnityEngine.UI;

public class FeaturesMenu : MonoBehaviour
{
    [SerializeField] Button TheFeaturesBtn;

    private void Start ()
    {
        Activate();
    }

    public void Activate ()
    {
        SimulateButtonSelect();
        this.gameObject.SetActive(true);
    }

    public void Deactivate ()
    {
        this.gameObject.SetActive(false);
    }

    void SimulateButtonSelect ()
    {
        TheFeaturesBtn.Select();
    }
}