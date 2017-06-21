using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchingMachine : MonoBehaviour {

    public GameObject baseball;
    public Transform target;
    public float Vy, Vz, speed;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)){
            GameObject ballInstance = Instantiate(baseball, transform.position, transform.rotation);
            Vector3 movement = new Vector3(0.0f, Vy, Vz);
            ballInstance.GetComponent<Rigidbody>().AddForce(movement * speed); 
        }
	}
}
