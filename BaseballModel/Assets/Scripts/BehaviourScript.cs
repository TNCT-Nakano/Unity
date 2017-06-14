using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class BehaviourScript : MonoBehaviour, IInputClickHandler, ISpeechHandler {

    public float Vy,Vz,speed;
	//public UH unlimitedhand;

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
        if (transform.position.z < 5.0f)
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(0f, 0f, 5.0f);
            Vector3 movement = new Vector3(0.0f, Vy, Vz);
            rb.AddForce(movement * speed);
        }
    }
    
    public void OnInputClicked(InputClickedEventData eventData)
    {
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(0f, 0f, 5.0f);
        Vector3 movement = new Vector3(0.0f, Vy, Vz);
        rb.AddForce(movement * speed);
    }

    public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        if (eventData.RecognizedText.ToLower() == "come on")
        {
            //めんどくさいのでとりあえず引数ヌル渡して同じ関数呼び出す
            OnInputClicked(null);
        }
    }
	void OnCollisionEnter(Collision collision){
		//unlimitedhand.stimulate (2);
	}
}
