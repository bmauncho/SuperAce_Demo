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
        for(int i = 0;i<colFx.Length ; i++)
        {
            colFx [i].gameObject.SetActive(i == col );
        }
    }
}
