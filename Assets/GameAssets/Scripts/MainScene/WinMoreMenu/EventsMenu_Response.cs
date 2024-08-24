using UnityEngine;

public class EventsMenu_Response : MonoBehaviour
{
    public void Deactivate ()
    {
        this.gameObject.SetActive (false);
    }
    public void Activate ()
    {
        this.gameObject.SetActive (true);
    }
}
