using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class BatBehaviourScript : MonoBehaviour, INavigationHandler {

    private float x, y, z;
    private Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody.centerOfMass = new Vector3(0, 0, 1);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = Input.mousePosition;
        transform.rotation = Quaternion.Euler(pos.x , 0, 0);
    }

    public void OnNavigationStarted(NavigationEventData eventData)
    {
        x = eventData.CumulativeDelta.x * 90;
        y = eventData.CumulativeDelta.y * 90;
        z = eventData.CumulativeDelta.z * 90;
    }

    public void OnNavigationUpdated(NavigationEventData eventData)
    {
        x = eventData.CumulativeDelta.x * 90;
        y = eventData.CumulativeDelta.y * 90;
        z = eventData.CumulativeDelta.z * 90;
        Debug.Log("x:" + x + " y:" + y + " z:" + z);
    }

    public void OnNavigationCompleted(NavigationEventData eventData)
    {

    }

    public void OnNavigationCanceled(NavigationEventData eventData)
    {

    }
}
