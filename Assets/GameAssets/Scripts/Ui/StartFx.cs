using System.Collections;
using UnityEngine;

public class StartFx : MonoBehaviour
{
    public GameObject [] FxHolders;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowStartFx ();
    }
    [ContextMenu("Activate")]
    public void Activate ()
    {
        this.gameObject.SetActive (true);
    }

    public void Deactivate ()
    {
        this .gameObject.SetActive (false);
    }
    [ContextMenu("Startfx")]
    public void ShowStartFx ()
    {
        StartCoroutine(ActivateStartFxSequence());
    }

    public IEnumerator ActivateStartFxSequence ()
    {
        for (int i = 0 ; i < FxHolders.Length ; i++)
        {
            FxHolders [i].gameObject.SetActive(true); // Activate the current UI element
            yield return new WaitForSeconds(.25f); // Wait for 1 second
        }
        yield return new WaitForSeconds(5);
        for (int i = 0 ; i < FxHolders.Length ; i++)
        {
            FxHolders [i].gameObject.SetActive(false); // Activate the current UI element
        }

        Deactivate ();

    }
}
