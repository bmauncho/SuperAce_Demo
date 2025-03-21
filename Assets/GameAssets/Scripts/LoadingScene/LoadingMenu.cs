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
        APIManager.instance.fetchConfigData();
    }

    public void ActivateNextScene ()
    {
        if (asyncOperation.progress >= 0.9f && Time.time > timestamp
                && !string.IsNullOrEmpty(APIManager.instance.CashAmount))
        {
            button.GetComponent<Button>().interactable =false;
            ConfigMan.Instance.TheDebugObj.SetActive(false);
            asyncOperation.allowSceneActivation = true;
        }
    }

    IEnumerator loadYourAsyncScene ()
    {
        asyncOperation = SceneManager.LoadSceneAsync("MainScene");
        asyncOperation.allowSceneActivation = false;

        yield return null;
    }
}
