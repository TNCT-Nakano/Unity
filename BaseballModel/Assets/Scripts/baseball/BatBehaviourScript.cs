using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviourScript : MonoBehaviour{

    public bool forInit = false;

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

    public AudioClip hit, swing;

    private GameObject player;

    // Use this for initialization
    void Start () {
        //rigidbody.centerOfMass = new Vector3(0, 0, 1);
        GameObject bt = GameObject.Find("BluetoothDevices");
        UHBehav = bt.GetComponent<UnlimitedHandBehaviour>();
        SBehav = bt.GetComponent<SensorBehaviour>();
        rb = GetComponent<Rigidbody>();
        player = transform.root.gameObject;
    }

	
	// Update is called once per frame
    //60fps
	void Update () {
        if (!forInit)
        {
            //だいたい0.2(1/60)が出る
            frame = Time.deltaTime;

            //角速度による回転
            gyro = SBehav.Gyro;
            //x = -x, y = -z, z = -y
            //transform.Rotate(gyro, Space.Self);
            player.transform.Rotate(-gyro.x, -gyro.z, -gyro.y, Space.Self);

            //加速度による移動
            acc = SBehav.Accel;
            acc.y = -acc.y;
            acc.z = -acc.z;

            //transform.Translate(Quaternion.Inverse(transform.rotation) * acc* frame * frame, Space.World);
            //Debug.Log(gyro);
            //Debug.Log(acc+","+ Quaternion.Inverse(transform.rotation) * acc);
        }
        player.GetComponent<Rigidbody>().position = Camera.main.transform.position - new Vector3(0,0.6f,0);
    }

    //衝突時
    private void OnCollisionEnter(Collision collision)
    {
        AudioSource audiosource = gameObject.GetComponent<AudioSource>();
        //スイング音
        audiosource.PlayOneShot(swing);

        //チャンネル部位、時間sec max200、電圧max12、鋭さmax20
        Debug.Log("Hit!!!");
        UHBehav.stimulate(0, 1, 12, 20);

        //打撃音
        audiosource.PlayOneShot(hit);

        //UnlimitedHandによる衝撃
        if (collision.gameObject.CompareTag("Ball"))
        {
            
        }

    }

}
