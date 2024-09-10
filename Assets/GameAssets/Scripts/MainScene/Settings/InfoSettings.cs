using UnityEngine;

public class InfoSettings : MonoBehaviour
{
    [SerializeField] GameObject Bg;
    [SerializeField] GameObject Loader;
    [SerializeField] GameObject Content;


    public void ResetInfo ()
    {
        Bg.SetActive(true);
        Loader.SetActive(true);
        Content.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
