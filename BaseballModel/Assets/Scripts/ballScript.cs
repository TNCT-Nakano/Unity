using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class ballScript : MonoBehaviour{
    // Use this for initialization

    private Rigidbody rb;

    void Start () {
        rb = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        //下に10mの地点まで行くと破壊
        if (transform.position.y < -10f) Destroy(gameObject);

        //速度表示
        Debug.Log("速度：" + rb.velocity.magnitude + "m/s , " + rb.velocity.magnitude/1000*60*60 + "km/h , ");
	}

    //衝突時の処理
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
