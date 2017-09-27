using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Transform camera = GameObject.Find("HoloLensCamera").transform;
        Vector3 pos = new Vector3(camera.position.x, camera.position.y - 1, camera.position.z);
        transform.position = camera.transform.position;

        transform.parent = camera.transform;
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
