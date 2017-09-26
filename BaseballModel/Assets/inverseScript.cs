using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inverseScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(transform.InverseTransformDirection(-transform.up * 9.8f));
        Debug.Log(transform.up);
    }
}
