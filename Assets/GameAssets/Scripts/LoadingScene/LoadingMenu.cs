using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingMenu : MonoBehaviour
{
    public float load_time;
    float timestamp;
    public void nextScene ()
    {
        timestamp = Time.time + load_time;
        StartCoroutine(loadYourAsyncScene());
        APIManager.instance.fetchConfigData();
    }

    IEnumerator loadYourAsyncScene ()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainScene");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f && Time.time > timestamp 
                && !string.IsNullOrEmpty(APIManager.instance.CashAmount))
            {
                ConfigMan.Instance.TheDebugObj.SetActive(false);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        

    }
}
