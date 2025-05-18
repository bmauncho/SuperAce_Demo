using UnityEngine;
using UnityEngine.UI;

public class TurboSpin : MonoBehaviour
{
    public bool IsNormalSpin = false;
    public bool IsTurboSpin = false;
    public bool IsSuperTurboSpin = false;
    [SerializeField] private Image spinModBtn;
    [SerializeField] private Sprite normalMode;
    [SerializeField] private Sprite turboMode;
    [SerializeField] private Sprite spinturboMode;
    [SerializeField] private GameObject[] TurboIcons;
    public void IsTurboSpinPressed ()
    {
        //if (GetComponentInChildren<Toggle>().isOn)
        //{
        //   CommandCentre.Instance.TurboManager_.EnableTurbospin();
        //}
        //else
        //{
        //    CommandCentre.Instance.TurboManager_.DisableTurbospin();
        //}
    }

    public void SetSpinMode ()
    {
        if (IsNormalSpin)
        {
            SetMode(false , true , false , turboMode);
            SetTurboArrowIcon(false , true , false);
        }
        else if (IsTurboSpin)
        {
            SetMode(false , false , true , spinturboMode);
            SetTurboArrowIcon(false , false , true);
        }
        else if (IsSuperTurboSpin)
        {
            SetMode(true , false , false , normalMode);
            SetTurboArrowIcon(true , false , false);
        }
    }

    private void SetMode ( bool normal , bool turbo , bool superturbo , Sprite modeSprite)
    {
        IsNormalSpin = normal;
        IsTurboSpin = turbo;
        IsSuperTurboSpin = superturbo;
        spinModBtn.sprite = modeSprite;
    }

    public void SetTurboArrowIcon ( bool normal , bool turbo , bool superturbo )
    {
        if(normal)
        {
            for(int i = 0 ; i < TurboIcons.Length ; i++)
            {
                TurboIcons [i].SetActive(false);
            }
        }
        else if (turbo)
        {
            TurboIcons [0].SetActive(true);
        }
        else if (superturbo)
        {
            for (int i = 0 ; i < TurboIcons.Length ; i++)
            {
                TurboIcons [i].SetActive(true);
            }
        }
    }
}
