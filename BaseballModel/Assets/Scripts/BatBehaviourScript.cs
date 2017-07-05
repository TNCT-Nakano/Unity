using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class BatBehaviourScript : MonoBehaviour, INavigationHandler {

    private float x, y, z;
    private Rigidbody rigidbody;

    // Use this for initialization
    void Start () {
        rigidbody.centerOfMass = new Vector3(0, 0, 1);

        BlueComm bc = GetComponent<BlueComm>();

        //位置初期化
    }
	
	// Update is called once per frame
	void Update () {
        //マウスの位置によって回転させる
        Vector3 pos = Input.mousePosition;
        transform.rotation = Quaternion.Euler(pos.x/2 , 0, 0);

        //BlueNinjaからデータ取得
        BlueComm bc = GetComponent<BlueComm>();

        //加速度による移動
        /*var dir = Vector3.zero;
        dir = bc.Acceleration;
        if(dir != null)
        {
            dir.Normalize();
            transform.Translate(dir);
        }

        //ジャイロによる回転
        Quaternion gyro = Quaternion.Euler(bc.Gyro);
        if(gyro != null)
            transform.rotation = gyro;*/
        
    }

    public void OnNavigationStarted(NavigationEventData eventData)
    {
        x = eventData.CumulativeDelta.x * 90;
        y = eventData.CumulativeDelta.y * 90;
        z = eventData.CumulativeDelta.z * 90;
    }

    public void OnNavigationUpdated(NavigationEventData eventData)
    {
        x = eventData.CumulativeDelta.x * 90;
        y = eventData.CumulativeDelta.y * 90;
        z = eventData.CumulativeDelta.z * 90;
        Debug.Log("x:" + x + " y:" + y + " z:" + z);
    }

    public void OnNavigationCompleted(NavigationEventData eventData)
    {

    }

    public void OnNavigationCanceled(NavigationEventData eventData)
    {

    }
}
