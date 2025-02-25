using UnityEngine;

public class ExtraMan : MonoBehaviour
{
    public static ExtraMan Instance;
    public GiftsMan giftsMan;
    public MissionsMan missionsMan;
    public ExtraInfo infoTab;
    public FakeLoading fakeLoading;
    private void Awake()
    {
       
        if (Instance)
        {
            Destroy(Instance);
        }
        Instance = this;


    }
 
   
}
