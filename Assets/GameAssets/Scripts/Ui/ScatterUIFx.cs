using UnityEngine;

public class ScatterUIFx : MonoBehaviour
{
    public GameObject[] colFx;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate ()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate ()
    {
        gameObject.SetActive(false);
    }

    public void showeffect (int col)
    {
        colFx [col].gameObject.SetActive(true);
    }

    public void HideEffect (int col)
    {
        colFx [col].gameObject.SetActive(false);
    }
}
