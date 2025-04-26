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
    bool isPressed = false;
    private void Start ()
    {
        nextScene();
    }
    public void nextScene ()
    {
        timestamp = Time.time + load_time;
        StartCoroutine(loadYourAsyncScene());
    }
    IEnumerator loadYourAsyncScene ()
    {
        asyncOperation = SceneManager.LoadSceneAsync("MainScene");
        asyncOperation.allowSceneActivation = false;

        yield return null;
    }

    public void ActivateNextScene ()
    {
        if (isPressed) {return; }
        isPressed = true;
        //StartCoroutine(Activation());
    }

    public IEnumerator Activation ()
    {
        //GameManager.Instance.fetchConfigData();
        //yield return new WaitUntil(() => asyncOperation.progress >= 0.9f && Time.time > timestamp);
        //if (ConfigMan.Instance.IsDemo)
        //{
        //    GameManager.Instance.CashAmount = "2000";
        //}
        //else
        //{
        //    yield return new WaitUntil(() => !string.IsNullOrEmpty( GameManager.Instance.playerInfo.wallet_balance ) && GameManager.Instance.isDataFetched);
        //    GameManager.Instance.CashAmount = GameManager.Instance.playerInfo.wallet_balance;
        //}
        //button.GetComponent<Button>().interactable = false;
        //ConfigMan.Instance.TheDebugObj.SetActive(false);
        //isPressed = false;
        //asyncOperation.allowSceneActivation = true;
        //yield return new WaitForSeconds(.5f);
        yield return null ;
    }

}
