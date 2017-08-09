using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviourScript : MonoBehaviour{

    private float x, y, z;
    private BlueComm bc;

    // Use this for initialization
    void Start () {
        //rigidbody.centerOfMass = new Vector3(0, 0, 1);

        bc = GetComponent<BlueComm>();
    }

	
	// Update is called once per frame
	void Update () {
        /*
        //マウスの位置によって回転させる
        Vector3 pos = Input.mousePosition;
        transform.rotation = Quaternion.Euler(pos.x/2 , 0, 0);
        */

        //加速度による移動
        /*
        var dir = bc.Acceleration;
        if(dir != null)
        {
            dir.Normalize();
            transform.Translate(dir);
        }
        */

        //ジャイロによる回転
        Quaternion gyro = Quaternion.Euler(bc.Gyro); //Gyroはz,x,yの順
        if(gyro != null)
            transform.rotation *= gyro; //gyroの値分回転させる
        
    }

}
