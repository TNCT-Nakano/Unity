using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BehaviourScript : MonoBehaviour{

    public float Vy,Vz,speed;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        //ThroughMe();
        Destroy(gameObject, 3.0f);
    }
	
	// Update is called once per frame
	void Update () {
        //奈落に落ちた時用
        if (transform.position.z < -5.0f)
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(0f, 0f, 5.0f);
            //ThroughMe();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }
}
