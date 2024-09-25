using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScatterWinFx
{
    public GameObject Fx_1;
}
public class ScatterFxManager : MonoBehaviour
{
    public List<ScatterWinFx> Thefx = new List<ScatterWinFx>();

    public void ActivateWhichFx ( int whichFx )
    {
        Thefx [whichFx].Fx_1.SetActive(true);

    }

    public void Deactivate()
    {
        foreach (ScatterWinFx fx in Thefx)
        {
            if (fx.Fx_1.activeSelf )
            {
                fx.Fx_1.SetActive(false);
            }
        }
    }
}

