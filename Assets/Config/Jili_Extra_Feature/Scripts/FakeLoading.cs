using UnityEngine;

public class FakeLoading : MonoBehaviour
{
    public float timestamp;
    public void Open(float thet=3)
    {
        timestamp += thet;
        if (timestamp > 3)
        {
            timestamp = 3;
        }
        gameObject.SetActive(true);
    }
    void Update()
    {
        if (timestamp <0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            timestamp -= Time.deltaTime;
        }
    }
}
