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

    private Vector3 gravity = new Vector3(0,9.8f,0);

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

        gravity.y = SBehav.Accel.magnitude;
    }

	
	// Update is called once per frame
    //60fps
	void Update () {

        //だいたい0.2(1/60)が出る
		frame = Time.deltaTime;

        //角速度による回転
        gyro = SBehav.Gyro;
        gyro.x = -gyro.x;
        gyro.y = -gyro.y;
        transform.Rotate(gyro, Space.Self);

        //加速度による移動
        acc = SBehav.Accel;
        var move = Quaternion.Inverse(transform.rotation) * acc - gravity;
        move.y = -move.y;
        transform.Translate(Quaternion.Inverse(transform.rotation)* acc* frame * frame, Space.World);

        Debug.Log(acc+","+move);
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
