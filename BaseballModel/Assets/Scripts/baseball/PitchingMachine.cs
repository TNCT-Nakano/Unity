using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class PitchingMachine : MonoBehaviour, IInputClickHandler {
    
    public GameObject baseball;
    public Transform target;
    private float Vy, Vz;
    public float speed;

    public bool active;
    public float interval;
    private float timeleft;

    private bool standby=false;
    public Material defaultMaterial;
    public Material standbyMaterial;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (active) {
            timeleft -= Time.deltaTime;
            if (!standby && timeleft <= 1.0f)
            {
                standby = true;
                GetComponent<Renderer>().material = standbyMaterial;
            }
            if (timeleft <= 0.0f)
            {
                timeleft = interval;
                ThroughBall();
                standby = false;
                GetComponent<Renderer>().material = defaultMaterial;
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
