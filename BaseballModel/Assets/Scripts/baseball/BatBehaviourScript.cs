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
        /*********
         * げっぱ※
         * Unityはフレームの更新速度が可変であるため、frameによる時間の算出は不適切かと
         * Timeクラスにdeltatimeパラメータがあり、前のフレームからの経過時間が取得できるためこちらを用いるとよい
         */

        //だいたい0.2(1/60)が出る
		frame = Time.deltaTime;

        //加速度による移動
        acc = SBehav.Accel;

        //スレッショルド補正するつもり

        //ローカルに力を加える
        //rb.AddRelativeForce(acc, ForceMode.Acceleration);

        transform.Translate(acc * frame * frame, Space.Self);

        //ジャイロによる回転
        //        Quaternion gyro = Quaternion.Euler(SBehav.Gyro);
        gyro = SBehav.Gyro;
        //スレッショルド補正するつもり

        //gyroの値分回転させる
        /*********
         * げっぱ※
         * rigidbodyのオブジェクトにはtransformの操作が利きません
         * rigidbodyのプロパティに回転をかけるか、AddRelativeTorqueの利用を検討してください
         */

        //スレッショルド補正するつもり

        //gyroの値分回転させる
        // rigidbodyにインスタント速度変化を追加
        //rb.AddRelativeTorque (gyro, ForceMode.VelocityChange);

        transform.Rotate(gyro * frame, Space.Self);

        Debug.Log(acc.ToString() + "," + gyro.ToString());
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
            Debug.Log("Hit!!!");
            UHBehav.stimulate(0, 1, 12, 20);
        }

    }

}
