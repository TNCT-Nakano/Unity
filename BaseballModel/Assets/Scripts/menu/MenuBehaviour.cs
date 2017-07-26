using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;

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
        SpatialUnderstanding.Instance.RequestBeginScanning();
        var msgbox = Instantiate(dialog);
        msgbox.transform.Find("Panel").Find("Message").gameObject.GetComponent<Text>().text = "starting Spatial Understanding.";
    }

    public void StopSpatialUnderstanding()
    {
        SpatialUnderstanding.Instance.RequestFinishScan();
        var msgbox = Instantiate(dialog);
        msgbox.transform.Find("Panel").Find("Message").gameObject.GetComponent<Text>().text = "stopping Spatial Understanding.";
    }

    public void ToggleShowSpatialMesh()
    {
        SpatialMappingManager.Instance.DrawVisualMeshes ^= true; //xorによってトグル実現
        var msgbox = Instantiate(dialog);
        msgbox.transform.Find("Panel").Find("Message").gameObject.GetComponent<Text>().text = "toggle into "+ GameObject.Find("SpatialMapping").GetComponent<HoloToolkit.Unity.SpatialMapping.SpatialMappingManager>().DrawVisualMeshes.ToString();
    }

    public void MakePlanes()
    {
        SurfaceMeshesToPlanes.Instance.MakePlanes();
    }

    public void BackToTitle()
    {
        SceneManager.LoadSceneAsync("StartScene");
    }
}
