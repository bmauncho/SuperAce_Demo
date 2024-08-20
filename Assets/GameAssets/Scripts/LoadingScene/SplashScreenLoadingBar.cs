using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SplashScreenLoadingBar : MonoBehaviour
{
    public Image Loadingbar;
    public float load_time;
    float timestamp;

    public void Start()
    {
        nextScene();
    }

    void nextScene ()
    {
        timestamp = Time.time + load_time;
        Loadingbar.DOFillAmount(1f , load_time - 0.5f);
        StartCoroutine(loadYourAsyncScene());
    }

    IEnumerator loadYourAsyncScene ()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainScene");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f && Time.time > timestamp)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
