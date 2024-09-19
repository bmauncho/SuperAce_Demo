using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardfxColumn
{
    public List<GameObject> cardFxPos = new List<GameObject>();
}

public class CardFxManager : MonoBehaviour
{
    public GameObject CardFxHolder;
    public List<CardfxColumn> CardFx = new List<CardfxColumn>();
    public void Activate ()
    {
        //Debug.Log("activateMasks");
        CardFxHolder.SetActive(true);
    }

    public void Deactivate ()
    {
        CardFxHolder.SetActive(false);
    }
   

    public void ReturnToPool ()
    {
        foreach (CardfxColumn c in CardFx)
        {
           foreach(GameObject obj in c.cardFxPos)
            {
                if (obj != null)
                {
                    Cardfx thefx = obj.GetComponentInChildren<Cardfx>();
                    if (thefx)
                    {
                        CommandCentre.Instance.PoolManager_.ReturnFx(thefx.gameObject);
                    }
                }
            }
        }
    }
}
