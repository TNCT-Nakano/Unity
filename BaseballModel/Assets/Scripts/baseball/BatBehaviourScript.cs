using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviourScript : MonoBehaviour{

    private float x, y, z;
    private UnlimitedHandBehaviour UHBehav;
    private SensorBehaviour SBehav;

    //加速度と角速度のスレッショルド
    private Vector3 accThres = new Vector3(1,1,1);
    private Vector3 gyroThres = new Vector3(1, 1, 1);

    private Rigidbody rb;

    private int frame = 60;

    // Use this for initialization
    void Start () {
        //rigidbody.centerOfMass = new Vector3(0, 0, 1);

        UHBehav = GetComponent<UnlimitedHandBehaviour>();
        SBehav = GetComponent<SensorBehaviour>();
        rb = GetComponent<Rigidbody>();
    }

	
	// Update is called once per frame
    //60fps
	void Update () {
        /*
        //マウスの位置によって回転させる
        Vector3 pos = Input.mousePosition;
        transform.rotation = Quaternion.Euler(pos.x/2 , 0, 0);
        */

        //加速度による移動
        var acc = SBehav.Accel;
        //スレッショルド補正するつもり
        
        //ローカルに力を加える
        rb.AddRelativeForce(acc/frame/frame, ForceMode.Acceleration);

        //ジャイロによる回転
        Quaternion gyro = Quaternion.Euler(SBehav.Gyro);
        //スレッショルド補正するつもり

        //gyroの値分回転させる
        transform.rotation *= gyro;
        
    }



    //50fps
    private void FixedUpdate()
    {
        
    }

    //衝突時
    private void OnCollisionEnter(Collision collision)
    {
        //UnlimitedHandによる衝撃
        if (collision.gameObject.CompareTag("Ball"))
        {
            //チャンネル部位、時間sec max200、電圧max12、鋭さmax20
            UHBehav.stimulate(0, 1, 12, 20);
        }

    }

}
