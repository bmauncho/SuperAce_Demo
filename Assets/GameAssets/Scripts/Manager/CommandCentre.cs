using UnityEngine;

public class CommandCentre : MonoBehaviour
{
    public static CommandCentre Instance;
    public PoolManager PoolManager_;
    public CardManager CardManager_;
    public MultiDeckManager MultiDeckManager_;
    public GridManager GridManager_;
    public WinLoseManager WinLoseManager_;
    public GridColumnManager GridColumnManager_;
    public MainMenuController MainMenuController_;
    public SoundManager SoundManager_;
    public SettingsManager SettingsManager_;
    public BetManager BetManager_;
    private void Awake ()
    {
        //Application.targetFrameRate = 30;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }
}
