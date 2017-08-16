using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class PitchingMachine : MonoBehaviour, IInputClickHandler {
    
    public GameObject baseball;
    public Transform target;
    public float Vy, Vz, speed;

    public bool active;
    public float interval;
    private float timeleft;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (active) {
            timeleft -= Time.deltaTime;
            if (timeleft <= 0.0f)
            {
                timeleft = interval;
                ThroughBall();
            }
        }
	}

    void ThroughBall()
    {
        double theta = Math.Asin (18.5 * 9.81 / ( speed * speed) ) / 2;
        Vy = (float)Math.Sin(theta);
        Vz = -(float)Math.Cos(theta);
        GameObject ballInstance = Instantiate(baseball, transform.position, transform.rotation);
        Vector3 movement = new Vector3(0.0f, Vy, Vz);
        ballInstance.GetComponent<Rigidbody>().AddForce(movement * speed * 2);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        ThroughBall();
    }
}
