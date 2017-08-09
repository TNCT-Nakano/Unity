using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init : MonoBehaviour {

    public GameObject floor;

	// Use this for initialization
	void Start () {
        Vector3 pos = transform.position;
        pos.y = floor.transform.position.y;
        transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
