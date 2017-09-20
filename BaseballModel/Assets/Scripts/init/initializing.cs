using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class initializing : MonoBehaviour {
    public GameObject infoPanel;
    public UnlimitedHandBehaviour uh;
#if TEST
    private bool isOpen=false;
    private int counter;
#endif
	// Use this for initialization
	void Start () {
        var waitingWindow = Instantiate(infoPanel);
        waitingWindow.transform.Find("Panel").Find("Message").gameObject.GetComponent<Text>().text = "Now starting.\nPlease wait for Bluetooth connection.";
        StartCoroutine(waitConnection(waitingWindow));
#if TEST
        StartCoroutine(isOpenSTUB());
#endif
    }

#if TEST
    IEnumerator isOpenSTUB()    //テスト用、起動から3秒後にisOpenをtrueにします（Bluetooth通信確立を想定）
    {
        yield return new WaitForSeconds(10f);
        isOpen = true;
    }
#endif

    IEnumerator waitConnection(GameObject dialog)   //Bluetooth通信確立まで待機します
    {
#if TEST
        yield return new WaitUntil(()=> { return isOpen; });
#else
        yield return new WaitUntil(()=> { return uh.isOpen; });
#endif

        SceneManager.LoadSceneAsync("StartScene");
    }
}
