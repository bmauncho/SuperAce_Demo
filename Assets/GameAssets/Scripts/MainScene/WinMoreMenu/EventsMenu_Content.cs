using UnityEngine;

public class EventsMenu_Content : MonoBehaviour
{
    [SerializeField] EventsLoader Loader;
    [SerializeField] EventsMenu_Response noContentResponse;
    // Update is called once per frame
    void Update()
    {
        if (noContentResponse.gameObject.activeSelf) return;
        if (!Loader.gameObject.activeSelf)
        {
            noContentResponse.Activate();
        }
    }

    public void Deactivate ()
    {
        Loader.Deactivate();
        noContentResponse.Deactivate();
    }

    public void Refresh ()
    {
        Loader.refresh();
        noContentResponse.Deactivate();
    }
}
