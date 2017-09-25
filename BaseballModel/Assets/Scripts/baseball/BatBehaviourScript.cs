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

    private Vector3 acc;
    private Vector3 gyro;

    private Rigidbody rb;
	private float frame;

    // Use this for initialization
    void Start () {
        //rigidbody.centerOfMass = new Vector3(0, 0, 1);
        GameObject bt = GameObject.Find("BluetoothDevices");
        UHBehav = bt.GetComponent<UnlimitedHandBehaviour>();
        SBehav = bt.GetComponent<SensorBehaviour>();
        rb = GetComponent<Rigidbody>();
        
    }

	
	// Update is called once per frame
    //60fps
	void Update () {

        //だいたい0.2(1/60)が出る
		frame = Time.deltaTime;

        //加速度による移動
        acc = SBehav.Accel;
        //acc = new Vector3(0, 9.81f, 0);
        acc.y = -acc.y;
        transform.Translate(acc * frame * frame, Space.Self);
        //重力加速度を引く
        transform.Translate(Vector3.up * 9.7f * frame* frame, Space.World);
        
        //角速度による回転
        gyro = SBehav.Gyro;
        //gyro = new Vector3(0, 0, 0);
        transform.Rotate(gyro, Space.Self);

        Debug.Log(acc.ToString() + "," + gyro.ToString());
    }

    //衝突時
    private void OnCollisionEnter(Collision collision)
    {
        //UnlimitedHandによる衝撃
        if (collision.gameObject.CompareTag("Ball"))
        {
            //チャンネル部位、時間sec max200、電圧max12、鋭さmax20
            Debug.Log("Hit!!!");
            UHBehav.stimulate(0, 1, 12, 20);
        }

    }

}
