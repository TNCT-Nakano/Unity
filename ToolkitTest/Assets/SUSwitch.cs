using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SUSwitch : MonoBehaviour, IInputClickHandler {
    public void OnInputClicked(InputClickedEventData eventData)
    {
        HoloToolkit.Unity.SpatialUnderstanding su = GameObject.Find("SpatialUnderstanding").GetComponent<HoloToolkit.Unity.SpatialUnderstanding>();
        /*
        switch (su.ScanState)
        {
            case HoloToolkit.Unity.SpatialUnderstanding.ScanStates.Scanning:
                su.RequestFinishScan();
                break;
            default:
                su.RequestBeginScanning();
                Debug.Log("now SpatialUnderstanding state is " + su.ScanState);
                break;
        }
        */
        su.RequestFinishScan();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
