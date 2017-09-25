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

    int tmp = 0;

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
<<<<<<< HEAD
        /*********
         * げっぱ※
         * Unityはフレームの更新速度が可変であるため、frameによる時間の算出は不適切かと
         * Timeクラスにdeltatimeパラメータがあり、前のフレームからの経過時間が取得できるためこちらを用いるとよい
         */
		float frame = Time.deltaTime;

=======
        //だいたい0.2(1/60)が出る
		frame = Time.deltaTime;
>>>>>>> 1fb94979eda0240c11bc1f4182df8e59b286becd
        /*
        //マウスの位置によって回転させる
        Vector3 pos = Input.mousePosition;
        transform.rotation = Quaternion.Euler(pos.x/2 , 0, 0);
        */
        /*
         * バットのIsKinematicをオフにしました。
         */
        //加速度による移動
        acc = SBehav.Accel;

        //スレッショルド補正するつもり

        //ローカルに力を加える
<<<<<<< HEAD
        rb.AddRelativeForce(acc, ForceMode.Acceleration);

        //ジャイロによる回転
        //        Quaternion gyro = Quaternion.Euler(SBehav.Gyro);
        Vector3 gyro = SBehav.Gyro;
        //スレッショルド補正するつもり

        //gyroの値分回転させる
        /*********
         * げっぱ※
         * rigidbodyのオブジェクトにはtransformの操作が利きません
         * rigidbodyのプロパティに回転をかけるか、AddRelativeTorqueの利用を検討してください
         */
        /* スナダ rigidbodyにインスタント速度変化を追加 */
        rb.AddRelativeTorque(gyro, ForceMode.VelocityChange);

        Debug.Log(acc.ToString() + "," + gyro.ToString());
=======
        rb.AddRelativeForce(acc*frame*frame, ForceMode.Acceleration);

        //ジャイロによる回転
        //        Quaternion gyro = Quaternion.Euler(SBehav.Gyro);
        gyro = SBehav.Gyro;
        
        //スレッショルド補正するつもり

        //gyroの値分回転させる
        // rigidbodyにインスタント速度変化を追加
        rb.AddRelativeTorque (gyro, ForceMode.VelocityChange);
        
>>>>>>> 1fb94979eda0240c11bc1f4182df8e59b286becd
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
