using UnityEngine;

public class EventsMenu : MonoBehaviour
{
    public void Activate ()
    {
        this.gameObject.SetActive (true);
    }

    public void Deactivate ()
    {
        this .gameObject.SetActive (false);
    }
}
