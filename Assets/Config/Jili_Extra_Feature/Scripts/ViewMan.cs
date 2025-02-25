using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ViewMan : MonoBehaviour
{
    public bool IsLandScape;
    public Vector2 CurrentScale;
    Vector2 RefScale=new Vector2(1280,720);
    public Vector2 ScaleMultiplier;
    public float NewScaleMultiplier;
    float forceupdatetimestamp;
    public CanvasScaler canvasScaler;
    private void Start()
    {
        forceupdatetimestamp = Time.time + 3;
        RefreshAll();
    }
    private void Update()
    {
        CurrentScale = new Vector2(Screen.width, Screen.height);
        ScaleMultiplier.x = RefScale.x / CurrentScale.x;
        ScaleMultiplier.y = RefScale.y / CurrentScale.y;
        NewScaleMultiplier = (ScaleMultiplier.x / ScaleMultiplier.y);
        if (NewScaleMultiplier > 1.5f)
        {
            if (IsLandScape||forceupdatetimestamp>Time.time)
            {
                SetPotrait();
            }
        }
        else
        {
            if (!IsLandScape || forceupdatetimestamp > Time.time)
            {
                SetLandscape();
            }
        }
        if (canvasScaler)
        {
            if (NewScaleMultiplier < 2.9f)
            {
                canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            }
            else
            {
                canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            }
        }

    }
    
    [ContextMenu("Landscape")]
    public void SetLandscape()
    {
        IsLandScape = true;
      
        RefreshAll();
    }
    [ContextMenu("Potrait")]
    public void SetPotrait()
    {
        IsLandScape = false;
        RefreshAll();
    }
    void RefreshAll()
    {
        ViewControl[] views=FindObjectsOfType<ViewControl>();
        for(int i = 0; i < views.Length; i++)
        {
            views[i].Refresh();
        }
        ViewControl_Obj[] viewsObj = FindObjectsOfType<ViewControl_Obj>();
        for (int i = 0; i < viewsObj.Length; i++)
        {
            viewsObj[i].Refresh();
        }
       
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
#endif

    }
}
