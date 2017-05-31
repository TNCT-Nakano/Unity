using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourScript : MonoBehaviour {

    public float Vy,Vz,speed;
	public UH unlimitedhand;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        Vector3 movement = new Vector3(0.0f, Vy, Vz);
        rb.AddForce(movement * speed);

	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.z < -5.0f)
        {
            print("Space key is held down");
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(0f, 0f, 5.0f);
            Vector3 movement = new Vector3(0.0f, Vy, Vz);
            rb.AddForce(movement * speed);
        }
    }

	void OnCollisionEnter(Collision collision){
		unlimitedhand.stimulate (2);
	}
}
