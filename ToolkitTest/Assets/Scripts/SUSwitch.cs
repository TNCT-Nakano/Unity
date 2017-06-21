using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SUSwitch : MonoBehaviour, IInputClickHandler {
    HoloToolkit.Unity.SpatialUnderstanding su;
    public GameObject menu;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        //menu.GetComponent<Canvas>().enabled = !menu.GetComponent<Canvas>().enabled;
    }

    // Use this for initialization
    void Start () {
        //su = GameObject.Find("SpatialUnderstanding").GetComponent<HoloToolkit.Unity.SpatialUnderstanding>();
    }

    // Update is called once per frame
    void Update () {
	}

    private void OnCollisionEnter(Collision collision)
    {
    }
}
