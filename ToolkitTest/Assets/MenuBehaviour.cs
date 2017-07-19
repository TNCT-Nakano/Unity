using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour {
    public GameObject dialog;
    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void StartSpatialUnderstanding()
    {
        GameObject.Find("SpatialUnderstanding").GetComponent<HoloToolkit.Unity.SpatialUnderstanding>().RequestBeginScanning();
        var msgbox = Instantiate(dialog);
        msgbox.transform.Find("Panel").Find("Message").gameObject.GetComponent<Text>().text = "starting Spatial Understanding.";
    }

    public void StopSpatialUnderstanding()
    {
        GameObject.Find("SpatialUnderstanding").GetComponent<HoloToolkit.Unity.SpatialUnderstanding>().RequestFinishScan();
        var msgbox = Instantiate(dialog);
        msgbox.transform.Find("Panel").Find("Message").gameObject.GetComponent<Text>().text = "stopping Spatial Understanding.";
    }

    public void BackToTitle()
    {
        SceneManager.LoadSceneAsync("StartScene");
    }
}
