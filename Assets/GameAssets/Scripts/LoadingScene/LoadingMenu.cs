using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class LoadingMenu : MonoBehaviour
{
    public float load_time;
    private float timestamp;
    public GameObject button;
    private bool isPressed = false;

    [SerializeField] private AssetReference mainScene;

    private AsyncOperationHandle<SceneInstance> sceneHandle;

    private void Start ()
    {
        nextScene();
    }

    public void nextScene ()
    {
        StartCoroutine(loadYourAsyncScene());
    }

    IEnumerator loadYourAsyncScene ()
    {
        sceneHandle = Addressables.LoadSceneAsync(mainScene,LoadSceneMode.Additive , false);
        yield return sceneHandle;

        if (sceneHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Failed to load scene from Addressables.");
        }

        if (sceneHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Scene loaded successfully.");
        }
    }

    public void ActivateNextScene ()
    {
        if (isPressed) return;

        isPressed = true;
        //StartCoroutine(Activation());
    }

    //public IEnumerator Activation ()
    //{
    //    //GameManager.Instance.fetchConfigData();

    //    yield return new WaitUntil(() =>
    //        sceneHandle.IsValid() &&
    //        sceneHandle.PercentComplete >= 0.9f);

    //    if (ConfigMan.Instance.IsDemo)
    //    {
    //        GameManager.Instance.CashAmount = "2000";
    //    }
    //    else
    //    {
    //        yield return new WaitUntil(() =>
    //            !string.IsNullOrEmpty(GameManager.Instance.playerInfo.wallet_balance) &&
    //            GameManager.Instance.isDataFetched);
    //        GameManager.Instance.CashAmount = GameManager.Instance.playerInfo.wallet_balance;
    //    }

    //    ConfigMan.Instance.TheDebugObj.SetActive(false);
    //    isPressed = false;

    //    // Now activate the loaded scene
    //    yield return sceneHandle.Result.ActivateAsync();
    //    SceneManager.UnloadSceneAsync(0);
    //    yield return new WaitForSeconds(0.5f);
    //}
}
