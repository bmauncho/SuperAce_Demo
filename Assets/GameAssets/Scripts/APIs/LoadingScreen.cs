using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using DG.Tweening;
public class LoadingScreen : MonoBehaviour
{
    [Header("Scene Reference")]
    [SerializeField] private AssetReference playScene;
    public GameObject TextHolder;
    public TMP_Text LoadingText;
    public TMP_Text ContinueText;
    public RectTransform ContinueBtn;

    [Header("UI Elements")]
    [SerializeField] private Image loadingSlider;


    private float progress;
    private bool permissionAsked;
    private bool isSliderLoaded;
    private bool isSceneReady;



    private void Start ()
    {
        isSliderLoaded = false;
        StartCoroutine(LoadScene(playScene));
    }

    private void Update ()
    {
        UpdateImageFillAmount(loadingSlider , progress);
        LoadingText.text = $"{Mathf.FloorToInt(loadingPercentage())}%";
        if (progress >= 1f && !permissionAsked)
        {
            permissionAsked = true;
        }

        if (progress >= 1f && permissionAsked)
        {
            if (!isSliderLoaded)
            {
                isSliderLoaded = true;
                // Show the start button or any other UI element
                ContinueBtn.gameObject.SetActive(true);
                ContinueBtn.DOSizeDelta(new Vector2(150, 40), .25f);
                Invoke(nameof(ActivateContinueText), .15f);
                LoadingText.gameObject.SetActive(false);
                TextHolder.SetActive(false);
            }
        }
    }
    [ContextMenu("Activate")]
    public void Activate ()
    {
        if (isSceneReady) { return; }
        isSceneReady = true;
    } 

    private void UpdateImageFillAmount ( Image image , float amount )
    {
        if (image != null)
            image.fillAmount = amount;
    }

    private void ShowProgress ( float amount )
    {
        progress = amount;
    }

    IEnumerator LoadScene(AssetReference Which)
    {
        //Not allowing scene activation immediately
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(Which, LoadSceneMode.Additive, false);
        while (!handle.IsDone)
        {
            ShowProgress(handle.PercentComplete);
            yield return new WaitForSeconds(0.1f);
        }

        //One way to handle manual scene activation.
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            ShowProgress(handle.PercentComplete);
        }

        yield return new WaitUntil(()=> isSceneReady);

        Debug.Log($"isSceneReady: {isSceneReady}");

        GameManager.Instance.FetchConfigData();

        yield return new WaitUntil(() => GameManager.Instance.IsDataFetched());

        Debug.Log($"isDataFetched : {GameManager.Instance.IsDataFetched()}");
        handle.Result.ActivateAsync();
        ConfigMan.Instance.TheDebugObj.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SceneManager.UnloadSceneAsync(0);
    }

    public float loadingPercentage()
    {
        return loadingSlider.fillAmount * 100f;
    }

    void ActivateContinueText()
    {
        ContinueText.gameObject.SetActive(true);
    }
}