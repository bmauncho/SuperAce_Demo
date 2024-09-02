using UnityEngine;

public class BannerController : MonoBehaviour
{
    public GameObject Normal_Banner;
    public GameObject FreeGame_Banner;
    
    public void ActivateNormalBanner ()
    {
        Normal_Banner.SetActive (true);
    }

    public void DeactivateNormalBanner ()
    {
        Normal_Banner.SetActive (false);
    }

    public void ActivateFreeGameBanner ()
    {
        FreeGame_Banner.SetActive (true);
    }

    public void DeactivateFreeGameBanner ()
    {
        FreeGame_Banner.SetActive (false);
    }
}
