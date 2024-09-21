using DG.Tweening;
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
    public CashManager CashManager_;
    public CardMaskManager CardMaskManager_;
    public FreeGameManager FreeGameManager_;
    public ComboManager ComboManager_;
    public PayOutManager PayOutManager_;
    public HintManager HintManager_;
    public CommentaryManager CommentaryManager_;
    public AutoSpinManager AutoSpinManager_;
    public TurboManager TurboManager_;
    public CardFxManager CardFxManager_;
    public DemoManager DemoManager_;

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

        DOTween.SetTweensCapacity(2000, 150);

    }
}
