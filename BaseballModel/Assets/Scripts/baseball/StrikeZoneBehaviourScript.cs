using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeZoneBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

/*    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point;
        Debug.Log("Hit Strike Zone : " + pos);
    }*/

    void OnTriggerEnter(Collider other)
    {
        
        Debug.Log("Hit Strike Zone" );
    }
}
