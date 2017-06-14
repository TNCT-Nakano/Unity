using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SUSwitch : MonoBehaviour, IInputClickHandler {
    HoloToolkit.Unity.SpatialUnderstanding su;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        //when understanding space, stop it on click
        if (su.ScanState == HoloToolkit.Unity.SpatialUnderstanding.ScanStates.Scanning)
            su.RequestFinishScan();

        //when other

    }

    // Use this for initialization
    void Start () {
		su = GameObject.Find("SpatialUnderstanding").GetComponent<HoloToolkit.Unity.SpatialUnderstanding>();

    }

    // Update is called once per frame
    void Update () {
        //transform.rotation.Set(45f,45f,45f,0f);
	}
}
