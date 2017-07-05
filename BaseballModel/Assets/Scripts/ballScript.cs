using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class ballScript : MonoBehaviour{
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < -10f) Destroy(gameObject);
        
	}

    //衝突時の処理
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
