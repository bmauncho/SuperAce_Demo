using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingMenu : MonoBehaviour
{
    public float load_time;
    float timestamp;
    public AsyncOperation asyncOperation;
    public GameObject button;
    private void Start ()
    {
        nextScene();
    }
    public void nextScene ()
    {
        timestamp = Time.time + load_time;
        StartCoroutine(loadYourAsyncScene());
        //APIManager.instance.fetchConfigData();
    }
    IEnumerator loadYourAsyncScene ()
    {
        asyncOperation = SceneManager.LoadSceneAsync("MainScene");
        asyncOperation.allowSceneActivation = false;

        yield return null;
    }

    public void ActivateNextScene ()
    {
        StartCoroutine(Activation());
    }

    public IEnumerator Activation ()
    {
        APIManager.instance.fetchConfigData();
        yield return new WaitUntil(() => asyncOperation.progress >= 0.9f && Time.time > timestamp);
        if (ConfigMan.Instance.IsDemo)
        {
            APIManager.instance.CashAmount = "2000";
        }
        else
        {
            yield return new WaitUntil(() => !string.IsNullOrEmpty( APIManager.instance.playerInfo.wallet_balance));
            APIManager.instance.CashAmount = APIManager.instance.playerInfo.wallet_balance;
        }
        button.GetComponent<Button>().interactable = false;
        ConfigMan.Instance.TheDebugObj.SetActive(false);
        asyncOperation.allowSceneActivation = true;
        yield return null ;
    }

}
