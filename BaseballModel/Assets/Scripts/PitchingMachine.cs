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
        double theta = Math.Atan(-0.5 / 18 - 0.5 * -9.81 * 18 / (speed * 1000 / 3600));//前提として、y=1.5から投げるとしてるよ
        Debug.Log(theta);
        Vy = (float)Math.Sin(theta);
        Vz = -(float)Math.Cos(theta);
        GameObject ballInstance = Instantiate(baseball, transform.position, transform.rotation);
        Vector3 movement = new Vector3(0.0f, Vy, Vz);
        ballInstance.GetComponent<Rigidbody>().AddForce(movement * (speed * 1000 / 3600) / (float)Math.Cos(theta), ForceMode.VelocityChange);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        ThroughBall();
    }
}
